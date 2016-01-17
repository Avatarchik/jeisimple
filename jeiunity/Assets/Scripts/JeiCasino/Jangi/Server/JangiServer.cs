using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Thrift.Protocol;
using Thrift.Transport;
using jangi;
#if JDEBUG
using Debug = JDebug;
#endif


public class JangiServer : MonoBehaviour
{
	///public JgGameRoom serverLogic;

	#region Server System
	///[SerializeField]
	public List<JgGameRoom> rooms;// = new List<JgGameRoom>(1024);
	Dictionary<int,  JStateObject> peers = new Dictionary<int,  JStateObject>();

	[SerializeField]
	List<int> socketsJustDebug = new List<int>();

	byte[] writeBuffer = new byte[1024];
	#endregion

	#region Logic
	Dictionary<long, JgUserInfo> users = new Dictionary<long, JgUserInfo>();

	ReqLogin 			reqLogin 		= new ReqLogin();
	ReqChannelInfo 		reqChannelInfo 	= new ReqChannelInfo();
	ReqMatch			reqMatch 		= new ReqMatch();
//	ReqSangcharim		reqSangcharim	= new ReqSangcharim();
	#endregion

	bool LOG_ENABLED = false;
	// Thread signal.
	public static ManualResetEvent allDone = new ManualResetEvent(false);
	Thread serverThread;

	void Start()
	{
		serverThread = new Thread(new ThreadStart(StartListening));
		serverThread.Start();

		InitRooms();

		this.Invoke("TestMessageTransport", 2f);
	}
	void OnApplicationQuit()
	{
		Debug.Log("<color=green>JangiServer.OnApplicationQuit </color>\n");
		serverThread.Abort();
	}

	void Disconnect(JStateObject state) //Socket socket)
	{
		RemovePeer(state);

		state.workSocket.Shutdown(SocketShutdown.Both);
		state.workSocket.Close();
		state.workSocket = null;
	}

	public void StartListening() 
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
				Debug.Log("<color=green>S: Waiting for connections .. </color>\n");
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
	
	//static StateObject mState = new StateObject();
	public void AcceptCallback(IAsyncResult ar)
	{
		// Signal the main thread to continue.
		allDone.Set();
		
		// Get the socket that handles the client request.
		Socket listener = (Socket) ar.AsyncState;
		Socket handler = listener.EndAccept(ar);
		//handler.Handle.ToInt32();
		Debug.Log("<color=green>S: accepted </color>\n");
		
		// Create the state object.
		JStateObject newClient = new JStateObject(handler);

		AddPear(newClient);

		Receive(newClient);
	}

	void Receive(JStateObject state)
	{
		state.bytesRead = 0;
		state.workSocket.BeginReceive( state.readBuffer, 0, JStateObject.BufferSize, 0,
		                              new AsyncCallback(ReadCallback), state);
	}

	public void ReadCallback(IAsyncResult ar)
	{
		// Retrieve the state object and the handler socket
		// from the asynchronous state object.
		JStateObject state = (JStateObject) ar.AsyncState;
		//Socket handler = state.workSocket;
		
		// Read data from the client socket. 보류 중인 비동기 읽기를 끝냅니다.
		int bytesRead = state.workSocket.EndReceive(ar);
		int previousBytes = state.bytesRead;
		state.bytesRead += bytesRead;

		if (LOG_ENABLED)
			Debug.Log(string.Format("<color=green>S: - ReadCallback bytesRead({0}) </color>\n", bytesRead));

		if (bytesRead > 0)
		{
			bool newPhase = previousBytes == 0;
			if (newPhase)
			{
				state.ProcessHeader();
				
				bool completedDataTrans = state.bytesRead == state.lengthInHeader;
				if (completedDataTrans)
				{
					OnReadMessage(state);
				}
				
				Receive(state);
			}
			else
			{
				Debug.Log("<color=green>newPhase is false !!! </color>\n");
			}
			
			//Debug.Log(string.Format("S: bytesRead({0}), state.bytesRead({1}) \n", bytesRead, state.bytesRead));
		} 
		else
		{
			Debug.Log(string.Format("<color=green>C: total received({0}) sockt returned({1}) </color>\n", state.bytesRead, bytesRead));
			
			Disconnect(state);
		}
	}

	public void Send(Socket socket, byte[] data, int length)
	{
		try {
			socket.BeginSend(data, 0, length, 0,
			                 new AsyncCallback(SendCallback), socket);
		}
		catch (Exception e) {
			Debug.Log("<color=green>S: Send </color>" + e.ToString());
		}
	}
	public void Send(Socket socket, byte messageType, TBase message)
	{
		int length = JThrift.Serialize(messageType, message, ref writeBuffer);

		Send(socket, writeBuffer, length);
	}

	private void Send(Socket handler, String data)
	{
		// Convert the string data to byte data using ASCII encoding.
		byte[] byteData = Encoding.ASCII.GetBytes(data);
		
		// Begin sending the data to the remote device.
		handler.BeginSend(byteData, 0, byteData.Length, 0,
		                  new AsyncCallback(SendCallback), handler);
	}
	void SendToAllPeers(byte messageType, TBase message)
	{
		foreach (KeyValuePair<int, JStateObject> state in peers)
		{
			int length = JThrift.Serialize(messageType, message, ref writeBuffer);
			Send(state.Value.workSocket, writeBuffer, length);
			break;
		}
	}

	private void SendCallback(IAsyncResult ar) {
		try {
			// Retrieve the socket from the state object.
			Socket handler = (Socket) ar.AsyncState;
			
			// Complete sending the data to the remote device.
			int bytesSent = handler.EndSend(ar);
			//Console.WriteLine("Sent {0} bytes to client.", bytesSent);

			if (LOG_ENABLED)
				Debug.Log(string.Format("<color=green>S: Sent {0} bytes to the client. </color>\n", bytesSent));
			
			//			handler.Shutdown(SocketShutdown.Both);
			//			handler.Close();
			
		} catch (Exception e) {
			Console.WriteLine(e.ToString());
		}
	}

	#region Logic
	public void OnReadMessage(JStateObject state)
	{
		JgC2S_Msgs messageType = (JgC2S_Msgs) state.readBuffer[2];// fromClient[2];

		if (LOG_ENABLED)
			Debug.Log(string.Format("<color=green>Server recieved </color>\n", messageType));
		
		switch (messageType)
		{
		case JgC2S_Msgs.kLogin:
			JThrift.Deserialize(state.readBuffer, Jg.Thrift_Offset, reqLogin);
			HandleMessage(state, reqLogin);
			break;
		case JgC2S_Msgs.kChannelInfo:
			JThrift.Deserialize(state.readBuffer, Jg.Thrift_Offset, reqChannelInfo);
			HandleMessage(state, reqChannelInfo);
			break;
		case JgC2S_Msgs.kReqMatch:
			JThrift.Deserialize(state.readBuffer, Jg.Thrift_Offset, reqMatch);
			HandleMessage(state, reqMatch);
			break;

		// Domain of the Room ------------------------------------------------------
		default:
			int roomIndex = state.roomIndex;
			rooms[roomIndex].OnReadMessage(state);
			break;
		}
	}
	#endregion
	#region Handle Messages
	void HandleMessage(JStateObject state, ReqLogin req)
	{
		Debug.Log(string.Format("<color=green>HandleMessage-ReqLogin : </color>",req.ToString()));
		AnsLogin ans = new AnsLogin();

		DummyUserInfo dummyUserInfo = JgRefs.ins_.dummyDb.GetUserInfo(req.UserName);

		bool isLoginOk = ans.Usn > 0;
		if (isLoginOk)
		{
			state.gsn = dummyUserInfo.gsn;
			state.name = dummyUserInfo.nickName;

			ans.NickName = dummyUserInfo.nickName;
			ans.Level = dummyUserInfo.gameLevel;
			ans.VictoryCount = dummyUserInfo.victoryCount;
			ans.DefeatCount = dummyUserInfo.defeatCount;
		}
		else
		{
			ans.NickName = string.Empty;
		}

		ans.Comment = "I'm a server.";

		if (isLoginOk) 
		{
			AddUser(state, dummyUserInfo);
		}

		Send(state.workSocket, (byte)JgS2C_Msgs.kLogin, ans);
	}

	void HandleMessage(JStateObject state, ReqChannelInfo req)
	{
	}

	void HandleMessage(JStateObject state, ReqMatch req)
	{
		JgUserInfo userInfo = users[state.gsn];

		JgGameRoom room = FindRoom(userInfo.gameLevel);
		if (room == null)
		{
			Debug.Log("Couldn't find a room . \n");
		}
		else
		{
			state.roomIndex = room.RoomIndex();

			room.AddParticipant(state);

			if (room.ParticipantCount() == 2)
			{
				room.SetPlayerNationsAndNewMatch();
			}
		}
	}
	#endregion
	void TestMessageTransport()
	{
		NtfTestString ntf = new NtfTestString();
		ntf.Message = "hello, I'm a server. (- -)/";
	
		SendToAllPeers((byte)JgS2C_Msgs.kNtfTestString, ntf);
	}
	
	void AddPear(JStateObject newClient)
	{
		peers.Add(newClient.SocketHandle, newClient);
		socketsJustDebug.Add(newClient.SocketHandle);
	}
	void RemovePeer(JStateObject client)
	{
		bool removed = peers.Remove(client.SocketHandle);
		socketsJustDebug.Remove(client.SocketHandle);
		
		Debug.Log(string.Format("S: removed({0}) : Handle({1}), GSN({2}) \n",
		                        removed, client.SocketHandle, client.gsn));
	}
	void AddUser(JStateObject state, DummyUserInfo dummyInfo)
	{
		state.gsn = dummyInfo.gsn;

		JgUserInfo userInfo = new JgUserInfo();

		userInfo.state = state;
		userInfo.gsn = dummyInfo.gsn;
		userInfo.userName = dummyInfo.email;
		userInfo.nickName = dummyInfo.nickName;
		userInfo.gameLevel = dummyInfo.gameLevel;
		userInfo.victoryCount = dummyInfo.victoryCount;
		userInfo.defeatCount = dummyInfo.defeatCount;

		users.Add(state.gsn, userInfo);
	}
	void RemoveUser(JStateObject user)
	{
		users.Remove(user.gsn);
	}


//	JgUserInfo UserInfoByGsn(long gsn)
//	{
//		return userInfos.ContainsKey(gsn) ? userInfos[gsn] : null;
//	}

	long FindMatch(long gsn)
	{
		return 0;
	}

	bool MatchingPlusMinusOne(int level)
	{
		return true;
	}
	void InitRooms()
	{
		for (int i = 0; i < rooms.Count; ++i)
		{
//			JgGameRoom room = new JgGameRoom();
//			room.SetRoomIndex(i);
//			rooms.Add(room);

			rooms[i].SetRoomIndex(i);
		}

		//Debug.Log("rooms[1].index " + rooms[1].RoomIndex);
	}
	JgGameRoom FindRoom(int gameLevel)
	{
		foreach (JgGameRoom room in rooms)
		{
			//JgGameRoom room = rooms[roomIndex];

			if (room.IsWaitingOpponent())
			{
//				long gsn = room.ParticiantGsn(0);
//				JgUserInfo userInfo = userInfos[gsn];
//
//				if (MatchingPlusMinusOne(userInfo.gameLevel))
//					return roomIndex;
				return room;
			}
		}
		foreach (JgGameRoom room in rooms)
		{
			if (room.IsEmpty())
			{
				return room;
			}
		}
		return null;
	}

	public void PrintServerStatus()
	{
		StringBuilder sb = new StringBuilder(2048);



		Debug.Log(sb.ToString());
	}
}
