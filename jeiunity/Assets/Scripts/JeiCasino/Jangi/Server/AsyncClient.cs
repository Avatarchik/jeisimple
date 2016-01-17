using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections;
#if JDEBUG
using Debug = JDebug;
#endif

public class AsyncClient : MonoBehaviour
{
	Socket client;
	JStateObject mState;
	int readCount = 0;

	public event Action<byte[]> OnRead;


	bool Log_Enabled = false;

	public JStateObject StateObject()
	{
		return mState;
	}

	#region Interfaces
	public void Connect(string hostEntry, int port) 
	{
		// Connect to a remote device.
		try {
			// Establish the remote endpoint for the socket.
			// The name of the 
			// remote device is "host.contoso.com".
			//IPHostEntry ipHostInfo = Dns.Resolve("host.contoso.com");
			IPHostEntry ipHostInfo = Dns.GetHostEntry(hostEntry);
			IPAddress ipAddress = ipHostInfo.AddressList[0];
			IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
			
			// Create a TCP/IP socket.
			client = new Socket(AddressFamily.InterNetwork,
			                           SocketType.Stream, ProtocolType.Tcp);
			
			// Connect to the remote endpoint.
			client.BeginConnect( remoteEP, 
			                    new AsyncCallback(ConnectCallback), client);
			
		} catch (Exception e) {
			Console.WriteLine(e.ToString());
		}
	}
	public void Disconnect()
	{
		// Release the socket.
		Debug.Log("C: released the socket \n");
		client.Shutdown(SocketShutdown.Both);
		client.Close();
		client = null;
	}
	public void Write(byte[] data, int length)
	{
		try {
			// Begin sending the data to the remote device.
			client.BeginSend(data, 0, length, 0,
			                 new AsyncCallback(WriteCallback), client);
		}
		catch (Exception e) {
			Debug.Log("C: Send " + e.ToString());
		}
	}
	private void Read(Socket client, bool beginNewPhase)
	{
		try {
			// Begin receiving the data from the remote device.
			if (beginNewPhase)
			{
				mState.bytesRead = 0;
				client.BeginReceive( mState.readBuffer, 0, JStateObject.BufferSize,
				                    0,
				                    new AsyncCallback(ReadCallback), mState);
			}
			else
			{
				client.BeginReceive( mState.readBuffer, 0, JStateObject.BufferSize - mState.bytesRead,
				                    0,
				                    new AsyncCallback(ReadCallback), mState);
			}
		} catch (Exception e) {
			//Console.WriteLine(e.ToString());
			Debug.Log("C: Receive " + e.ToString());
		}
	}
	public bool IsBound()
	{
		return this.client != null && this.client.IsBound;
	}
	#endregion

	private void ConnectCallback(IAsyncResult ar)
	{
		try
		{
			// Retrieve the socket from the state object.
			Socket client = (Socket) ar.AsyncState;
			
			// Complete the connection.
			client.EndConnect(ar);

			mState = new JStateObject(client);

			Debug.Log(string.Format("C: Socket has connected to {0} \n", client.RemoteEndPoint.ToString()));
			Read(client, true);
		} 
		catch (Exception e)
		{
			Debug.Log("C: ConnectCallback " + e.ToString());
		}
	}

	private void WriteCallback(IAsyncResult ar)
	{
		try
		{
			// Retrieve the socket from the state object.
			Socket client = (Socket) ar.AsyncState;
			
			// Complete sending the data to the remote device.
			int bytesSent = client.EndSend(ar);
			if (Log_Enabled)
				Debug.Log(string.Format("C: Sent {0} bytes to server.", bytesSent));
		} 
		catch (Exception e)
		{
			Debug.Log("C: SendCallback " + e.ToString());
		}
	}

	private void ReadCallback( IAsyncResult ar )
	{
		++readCount;

		if (Log_Enabled)
			Debug.Log(string.Format("C: ReadCallback readCount({0}) \n", readCount));

		try {
			JStateObject state = (JStateObject) ar.AsyncState;
			
			// Read data from the remote device. // 보류 중인 비동기 읽기를 끝냅니다.
			int bytesRead = client.EndReceive(ar); 
			int previousBytes = state.bytesRead;
			state.bytesRead += bytesRead;

			if (bytesRead > 0)
			{
				bool newPhase = previousBytes == 0;
				if (newPhase)
				{
					state.ProcessHeader();

					bool completedDataTrans = state.bytesRead == state.lengthInHeader;
					if (completedDataTrans)
					{
						//JgNetMessageMgr.ins_.OnRead(state.buffer);
						OnRead(state.readBuffer);
					}
				
					Read(state.workSocket, completedDataTrans);
				}
				else
				{
					Debug.Log("<color=red>newPhase is false !!! </color>\n");
				}

				if (Log_Enabled)
				{
					Debug.Log(string.Format("C: bytesRead({0}), state.bytesRead({1}) \n", bytesRead, state.bytesRead));
					PrintBytes(state.readBuffer);
				}
			} 
			else
			{
				Debug.Log(string.Format("C: total received({0}) sockt returned({1}) \n", state.bytesRead, bytesRead));

				//System.Threading.Timer timer = 
				new Timer( obj => { Disconnect(); }, 
				null, 100, System.Threading.Timeout.Infinite );
			}
		} catch (Exception e) {
			//Console.WriteLine(e.ToString());
			Debug.Log("C: ReceiveCallback " + e.ToString());
		}
	}
	void PrintBytes(byte[] bytes)
	{
		System.Text.StringBuilder sb = new StringBuilder();
		foreach (byte b in bytes)
		{
			sb.Append(b.ToString());
			sb.Append(",");
		}
		Debug.Log(sb.ToString());
	}
}
