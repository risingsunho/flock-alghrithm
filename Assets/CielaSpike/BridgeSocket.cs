using System;
// using System.Linq;
using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CielaSpike;

// BridgeSocket: ROS Client -> Unity Server
public class BridgeSocket : MonoBehaviour
{

    public class point
    {
        public float x, y, z;
        public point() { x = y = z = 0.0f; }
    }

    public static point position, rotation;
    public float speed = 1;

    void Start()
    {
        Task task = null;
        ThreadNinjaMonoBehaviourExtensions.StartCoroutineAsync(this, AsyncCoroutine(), out task);
        position = new point();
        rotation = new point();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startposition = transform.position;
        Vector3 destination = new Vector3(position.x,position.y,position.z);
        Quaternion startrotation = transform.rotation;
        Quaternion desiredrotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

        transform.position = Vector3.Slerp(startposition,destination,Time.fixedDeltaTime*speed); 
        transform.rotation = Quaternion.Slerp(startrotation, desiredrotation, Time.fixedDeltaTime*speed);
    }

    IEnumerator AsyncCoroutine()
    {
        yield return Ninja.JumpBack;
        AsynchronousSocketListener.StartListening();
        yield return Ninja.JumpToUnity;
    }

    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client socket
        public Socket workSocket = null;
        // Size of receive buffer
        public const int BufferSize = 1024;
        // Receive buffer
        public byte[] buffer = new byte[BufferSize];
        // Received data string
        public StringBuilder sb = new StringBuilder();
    }

    public class AsynchronousSocketListener
    {
        // Thread signal
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public AsynchronousSocketListener() { }

        public static void StartListening()
        {
            int workerThreads, completionThreads;
            ThreadPool.GetMinThreads(out workerThreads, out completionThreads);
            ////Debug.Log("Worker: " + workerThreads + ", Completion: " + completionThreads);

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com"
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());   // IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            Debug.Log("IP_LIST_CHECK >>>>>>>>>>>>>>>>>>>>");
            foreach (var address in ipHostInfo.AddressList)
            {
                Debug.Log("ADDRESS: " + address);
            }
            Debug.Log(">>>>>>>>>>>>>>>>>>>> IP_LIST_CHECK\n");

            IPAddress ipAddress = ipHostInfo.AddressList[1]; // [0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 1234); // 9090);     // Sender socket (sender.py)
            // IPEndPoint localEndPoint2 = new IPEndPoint(ipAddress, 9091);    // Receiver socket (receiver.py)

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                /*
                Socket client = listener.Accept();
                client.Receive(bytes);
                //Debug.Log(Encoding.Default.GetString(bytes));
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                //Debug.Log("client closed...");
                */

                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                /*
                while (true)
                {
                    //Debug.Log("allDone.Reset();");
                    // Set the event to nonsignaled state
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    //Debug.Log("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    // Wait until a connection is made before continuing
                    allDone.WaitOne();
                }
                */

            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            Debug.Log("Client socket Accepted...");
            // Signal the main thread to continue
            allDone.Set();

            // Get the socket that handle the client request.
            Socket listener = ar.AsyncState as Socket;
            Socket handler = listener.EndAccept(ar);

            // Create the state object
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        /* CUSTOM_ACCEPT_CALLBACK_FUNCTION_FOR_SENDER
        public static void AcceptSenderCallback(IAsyncResult ar)
        {
            //Debug.Log("System >> sender.py socket accepted...");
            allDone.Set();

            Socket listener = ar.AsyncState as Socket;
            Socket handler = listener.EndAccept(ar);

            // Create the state object
            StateObject state = new StateObject();
            state.workSocket = handler;
            Send(state.workSocket, )
        }
        */

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handle socket
            // from the asynchronous state object.
            StateObject state = ar.AsyncState as StateObject;
            Socket handler = state.workSocket;

            // Read data from the client socket.
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead >= 130)
            {
                Debug.Log("OVERLOADED: " + Encoding.UTF8.GetString(state.buffer, 0, bytesRead));
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                return;
            }


            // JSONIFY
            string data = Encoding.UTF8.GetString(state.buffer, 0, bytesRead);
            Debug.Log("Data: " + data);
            string[] splitt = data.Split('\n');
            foreach (var s in splitt) Debug.Log("splitt: " + s);
            string[] values = splitt[1].Split('\t');
            foreach (var value in values) Debug.Log("value: " + value);
            position.x = (float)Double.Parse(values[0]);
            position.y = (float)Double.Parse(values[1]);
            position.z = (float)Double.Parse(values[2]);
            rotation.x = (float)Double.Parse(values[3]);
            rotation.y = (float)Double.Parse(values[4]);
            rotation.z = (float)Double.Parse(values[5]);

            if (Encoding.Default.GetString(state.buffer, 0, bytesRead).Equals("exit"))
            {
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                //Debug.Log("Socket closed... >> Exit");
                return;
            }


            if (bytesRead > 0)
            {
                // There might be more data, so store the data received so far.
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read more data.
                if (Encoding.UTF8.GetString(state.buffer).IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the client Display it on the console.
                    // Echo the data back to the client
                    Send(handler, content);
                }
                else
                {
                    // Not all data recevied Get more.
                    // Array.Clear(state.buffer, 0, StateObject.BufferSize);
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
            else
            {
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                //Debug.Log("Socket closed...");
            }

        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            //Debug.Log("Callback::Send");
            try
            {
                // Retrieve the socket from the state object
                Socket handler = ar.AsyncState as Socket;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                //Debug.Log("Send " + bytesSent + " bytes to client");

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                //Debug.Log(e.ToString());
            }
        }

        public static int Main(String[] args)
        {
            StartListening();
            return 0;
        }
    }
}