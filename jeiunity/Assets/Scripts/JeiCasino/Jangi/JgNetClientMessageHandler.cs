using UnityEngine;
using System.Collections;
using System;
using jangi;
using Thrift.Protocol;
using Thrift.Transport;
using System.Collections.Generic;


public class JgNetClientMessageHandler : MonoBehaviour
{
	byte[] writeBuffer = new byte[1024 * 1];
	public AsyncClient transport;

	string dbgUserName;
	//JgNation nation;

	public event Action<NtfTestString>			onNtfTestString;
	public event Action<AnsLogin>				onAnsLogin;
	public event Action<NtfMatch>				onNtfMatch;
	public event Action<AnsCancelRequestMatch>	onAnsCancelMatch;
	public event Action<NtfSangcharimHan>		onNtfSancharimHan;
	public event Action<NtfSangcharim>			onNtfSangcharim;
	
	public event Action<NtfChangeTurn>			onNtfChangeTurn;
	public event Action<NtfMovePawn> 			onNtfMovePawn;
	
	//public JgGameRoom serverLogic;
	
	//Action<AnsLogin> ansLoginHandler;
	List<Action<TBase>> messageCallbacks = new List<Action<TBase>>(32);

	NtfTestString			ntfTestString = new NtfTestString();
	AnsLogin 				ansLogin = new AnsLogin();
	NtfMatch 				ntfMatch = new NtfMatch();
	AnsCancelRequestMatch	ansCancelMatch = new AnsCancelRequestMatch();

	NtfSangcharimHan 	ntfSangcharimHan = new NtfSangcharimHan();
	NtfSangcharim		ntfSangcharim = new NtfSangcharim();
	NtfChangeTurn		ntfChangeTurn = new NtfChangeTurn();
	NtfMovePawn			ntfMovePawn = new NtfMovePawn();

	List<JgS2C_Msgs> messageQueue = new List<JgS2C_Msgs>();
	
	#region MonoBehaviour
	void Awake()
	{
		transport.OnRead += OnRead;

		for (int i = 0; i < 32; ++i)
			messageCallbacks.Add(null);
	}
	// for working in main thread
	void Update()
	{
		if (messageQueue.Count > 0)
		{
			foreach (JgS2C_Msgs messageType in messageQueue)
			{
				//Debug.Log(string.Format("C({0}): <-S ({1}) ... \n", dbgUserName, messageType));

				switch (messageType)
				{
				case JgS2C_Msgs.kNtfTestString:
					Debug.Log(string.Format("C({0}): <-S ({1}) - {2} \n", dbgUserName, messageType, ntfTestString));
					//Debug.Log(ntfTestString.ToString());
					if (onNtfTestString != null)
						onNtfTestString(ntfTestString);
					break;

				case JgS2C_Msgs.kLogin:
					dbgUserName = ansLogin.NickName;
					//Debug.Log(ansLogin.ToString());
					Debug.Log(string.Format("C({0}): <-S ({1}) - {2} \n", dbgUserName, messageType, ansLogin));
					onAnsLogin(ansLogin);
					break;
				case JgS2C_Msgs.kNtfMatch:
					//nation = ntfMatch.IsCho ? JgNation.kCho : JgNation.kHan;
					//Debug.Log(ntfMatch.ToString());
					Debug.Log(string.Format("C({0}): <-S ({1}) - {2} \n", dbgUserName, messageType, ntfMatch));
					onNtfMatch(ntfMatch);
					break;
				case JgS2C_Msgs.kCancelMatch:
					//Debug.Log(ansCancelMatch.ToString());
					Debug.Log(string.Format("C({0}): <-S ({1}) - {2} \n", dbgUserName, messageType, ansCancelMatch));
					onAnsCancelMatch(ansCancelMatch);
					break;
				case JgS2C_Msgs.kNtfSangcharimHan:
					//Debug.Log(ntfSangcharimHan.ToString());
					Debug.Log(string.Format("C({0}): <-S ({1}) - {2} \n", dbgUserName, messageType, ntfSangcharimHan));
					onNtfSancharimHan(ntfSangcharimHan);
					break;
				case JgS2C_Msgs.kNtfSangcharim:
					//Debug.Log(ntfSangcharim.ToString());
					Debug.Log(string.Format("C({0}): <-S ({1}) - {2} \n", dbgUserName, messageType, ntfSangcharim));
					onNtfSangcharim(ntfSangcharim);
					break;

				case JgS2C_Msgs.kNtfChangeTurn:
					//Debug.Log(ntfChangeTurn.ToString());
					Debug.Log(string.Format("C({0}): <-S ({1}) - {2} \n", dbgUserName, messageType, ntfChangeTurn));
					onNtfChangeTurn(ntfChangeTurn);
					break;
				case JgS2C_Msgs.kNtfMovePawn:
					//Debug.Log(ntfMovePawn.ToString());
					Debug.Log(string.Format("C({0}): <-S ({1}) - {2} \n", dbgUserName, messageType, ntfMovePawn));
					onNtfMovePawn(ntfMovePawn);
					break;

				default:
					Debug.Log(string.Format("<color=red>couldn't find a given case ( {0} )  </color>", messageType));
					break;
				}
			}
			messageQueue.Clear();
		}
	}
	#endregion
	
	public void SendMessageToServer(JgC2S_Msgs messageType, TBase message)
	{
		if (transport.IsBound() == false) return;

		Debug.Log(string.Format("C({0}): ->S ({1}) - {2} \n", dbgUserName, messageType, message));

		int length = JThrift.Serialize((byte)messageType, message, ref writeBuffer);

		transport.Write(writeBuffer, length);
	}
	public void WriteToServer(JgC2S_Msgs messageType, TBase message, Action<TBase> callback)
	{
		if (transport.IsBound() == false) return;

		int messageIndex = (int)messageType;
		messageCallbacks[messageIndex] = callback;

		SendMessageToServer(messageType, message);
	}
	
	public void OnRead(byte[] buffer)
	{
		JgS2C_Msgs messageType = (JgS2C_Msgs) buffer[2];
		
		switch (messageType)
		{
		case JgS2C_Msgs.kNtfTestString:
			JThrift.Deserialize(buffer, 4, ntfTestString);
			break;
		case JgS2C_Msgs.kLogin:
			JThrift.Deserialize(buffer, 4, ansLogin);
			break;
		case JgS2C_Msgs.kNtfMatch:
			JThrift.Deserialize(buffer, 4, ntfMatch);
			break;
		case JgS2C_Msgs.kNtfSangcharimHan:
			JThrift.Deserialize(buffer, 4, ntfSangcharimHan);
			break;
		case JgS2C_Msgs.kNtfSangcharim:
			JThrift.Deserialize(buffer, 4, ntfSangcharim);
			break;
		case JgS2C_Msgs.kNtfChangeTurn:
			JThrift.Deserialize(buffer, 4, ntfChangeTurn);
			break;
		case JgS2C_Msgs.kNtfMovePawn:
			JThrift.Deserialize(buffer, 4, ntfMovePawn);
			break;
				

		default:
			Debug.Log(string.Format("<color=orange>couldn't find a given case ( {0} )  </color>", messageType));
			break;
		}
		
		messageQueue.Add(messageType);
	}

	void PrintBytes(byte[] bytes)
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		foreach (byte b in bytes)
		{
			sb.Append(b.ToString());
			sb.Append(",");
		}
		Debug.Log(sb.ToString());
	}
}
