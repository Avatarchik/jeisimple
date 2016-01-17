using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;


public class AsyncServer : MonoBehaviour 
{
	public Dictionary<long, JStateObject> clients;


	// Thread signal.
	public static ManualResetEvent allDone = new ManualResetEvent(false);
	
	void Start()
	{
		Thread serverThread = new Thread(new ThreadStart(StartListening));
		serverThread.Start();
	}
	
	public static void StartListening() 
	{
		// Establish the local endpoint for the socket.
		// The DNS name of the computer
		// running the listener is "host.contoso.com".
		IPHostEntry ipHostInfo = Dns.GetHostEntry("127.0.0.1");
		IPAddress ipAddress = ipHostInfo.AddressList[0];
		IPEndPoint localEndPoint = new IPEndPoint(ipAddress, TestServer.PORT);
		
		// Create a TCP/IP socket.
		Socket listener = new Socket(AddressFamily.InterNetwork,
		                             SocketType.Stream, ProtocolType.Tcp );
		
		// Bind the socket to the local endpoint and listen for incoming connections.
		try {
			listener.Bind(localEndPoint);
			listener.Listen(100);
			
			while (true)
			{
				// Set the event to nonsignaled state.
				allDone.Reset();
				
				// Start an asynchronous socket to listen for connections.
				//Console.WriteLine("Waiting for a connection...");
				Debug.Log("S: Waiting for a connection.. \n");
				listener.BeginAccept( 
				                     new AsyncCallback(AcceptCallback),
				                     listener );
				
				// Wait until a connection is made before continuing.
				allDone.WaitOne();
			}
			
		} catch (Exception e) {
			Console.WriteLine(e.ToString());
		}
	}

	static JStateObject mState = new JStateObject();
	public static void AcceptCallback(IAsyncResult ar)
	{
		// Signal the main thread to continue.
		allDone.Set();
		
		// Get the socket that handles the client request.
		Socket listener = (Socket) ar.AsyncState;
		Socket handler = listener.EndAccept(ar);

		Debug.Log("S: accepted \n");

		// Create the state object.
		//StateObject mState = new StateObject();
		mState.workSocket = handler;
		mState.bytesRead = 0;
		mState.workSocket.BeginReceive( mState.readBuffer, 0, JStateObject.BufferSize, 0,
		                     new AsyncCallback(ReadCallback), mState);
	}
	static void Receive(JStateObject state)
	{
		state.bytesRead = 0;
		state.workSocket.BeginReceive( state.readBuffer, 0, JStateObject.BufferSize, 0,
		                              new AsyncCallback(ReadCallback), state);
	}

	public static void ReadCallback(IAsyncResult ar) 
	{
		// Retrieve the state object and the handler socket
		// from the asynchronous state object.
		JStateObject state = (JStateObject) ar.AsyncState;
		//Socket handler = state.workSocket;
		
		// Read data from the client socket. 보류 중인 비동기 읽기를 끝냅니다.
		int bytesRead = state.workSocket.EndReceive(ar);
		state.bytesRead += bytesRead;

		//if (state.bytesRead == 0 
		Debug.Log(string.Format("S: - ReadCallback bytesRead {0} \n", bytesRead));
		//state.workSocket.IsBound

		if (bytesRead > 0)
		{
			// There might be more data, so store the data received so far.
			//state.sb.Append(Encoding.ASCII.GetString(state.buffer,0,bytesRead));
			
			// Get the rest of the data.
			Receive(state);
//			state.workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
//			                              new AsyncCallback(ReadCallback), state);
		}
		else
		{
			Debug.Log("S: bytesRead <= 0 \n");
			Disconnect(state.workSocket);
		}
			
//		if (bytesRead > 0) 
//		{
//			// There  might be more data, so store the data received so far.
//			state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
//			
//			// Check for end-of-file tag. If it is not there, read 
//			// more data.
//			content = state.sb.ToString();
//			if (content.IndexOf("<EOF>") > -1)
//			{
//				// All the data has been read from the 
//				// client. Display it on the console.
////				Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
////				                  content.Length, content );
//				Debug.Log(string.Format("S: Read {0} bytes from socket. \n Data : {1} \n",
//				                        content.Length, content));
//
//				//Receive(state);
//
//				// Echo the data back to the client.
//				Send(state.workSocket, content);
////				Send(handler, content);
////
////				System.Threading.Timer timer = new Timer(
////					obj => { Send(handler, content); }, 
////					null, 1000, System.Threading.Timeout.Infinite);
//
////				System.Threading.Timer timer2 = new Timer(
////					obj => { Disconnect(state.workSocket); }, 
////				null, 5000, System.Threading.Timeout.Infinite);
//			} 
//			else 
//			{
//				Debug.Log("S: Not all data received. Get more.");
//				// Not all data received. Get more.
//				state.workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
//				                     new AsyncCallback(ReadCallback), state);
//			}
//		}
//		else
//		{
//			Debug.Log("S: bytesRead <= 0 \n");
//		}
	}
	public static void ReadCallback22(IAsyncResult ar) {
		String content = String.Empty;
		
		// Retrieve the state object and the handler socket
		// from the asynchronous state object.
		JStateObject state = (JStateObject) ar.AsyncState;
		Socket handler = state.workSocket;
		
		// Read data from the client socket. 보류 중인 비동기 읽기를 끝냅니다.
		int bytesRead = handler.EndReceive(ar);
		state.bytesRead += bytesRead;
		
		//if (state.bytesRead == 0 
		Debug.Log("S: - ReadCallback");
		//state.workSocket.IsBound
		
		if (bytesRead > 0) 
		{
			// There  might be more data, so store the data received so far.
			state.sb.Append(Encoding.ASCII.GetString(state.readBuffer, 0, bytesRead));
			
			// Check for end-of-file tag. If it is not there, read 
			// more data.
			content = state.sb.ToString();
			if (content.IndexOf("<EOF>") > -1)
			{
				// All the data has been read from the 
				// client. Display it on the console.
				//				Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
				//				                  content.Length, content );
				Debug.Log(string.Format("S: Read {0} bytes from socket. \n Data : {1} \n",
				                        content.Length, content));
				
				//Receive(state);
				
				// Echo the data back to the client.
				Send(handler, content);
				//				Send(handler, content);
				//
				//				System.Threading.Timer timer = new Timer(
				//					obj => { Send(handler, content); }, 
				//					null, 1000, System.Threading.Timeout.Infinite);
				
				//				System.Threading.Timer timer2 = new Timer(
				//					obj => { Disconnect(state.workSocket); }, 
				//				null, 5000, System.Threading.Timeout.Infinite);
			} 
			else 
			{
				Debug.Log("S: Not all data received. Get more.");
				// Not all data received. Get more.
				handler.BeginReceive(state.readBuffer, 0, JStateObject.BufferSize, 0,
				                     new AsyncCallback(ReadCallback), state);
			}
		}
		else
		{
			Debug.Log("S: bytesRead <= 0 \n");
		}
	}

	private static void Send(Socket handler, String data)
	{
		// Convert the string data to byte data using ASCII encoding.
		byte[] byteData = Encoding.ASCII.GetBytes(data);
		
		// Begin sending the data to the remote device.
		handler.BeginSend(byteData, 0, byteData.Length, 0,
		                  new AsyncCallback(SendCallback), handler);
	}
	
	private static void SendCallback(IAsyncResult ar) {
		try {
			// Retrieve the socket from the state object.
			Socket handler = (Socket) ar.AsyncState;
			
			// Complete sending the data to the remote device.
			int bytesSent = handler.EndSend(ar);
			//Console.WriteLine("Sent {0} bytes to client.", bytesSent);
			Debug.Log(string.Format("S: Sent {0} bytes to client. \n", bytesSent));

//			handler.Shutdown(SocketShutdown.Both);
//			handler.Close();
			
		} catch (Exception e) {
			Console.WriteLine(e.ToString());
		}
	}

	static void Disconnect(Socket socket)
	{
		socket.Shutdown(SocketShutdown.Both);
		socket.Close();
	}

}
