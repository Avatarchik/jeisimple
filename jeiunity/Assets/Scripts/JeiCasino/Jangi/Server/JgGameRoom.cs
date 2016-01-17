using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Thrift.Protocol;
using Thrift.Transport;
using jangi;
using System;



public partial class JgGameRoom : MonoBehaviour
{
	#region Content
	[SerializeField]
	int[] map = new int[100];

	[SerializeField]
	JStateObject choState;
	[SerializeField]
	JStateObject hanState;

	Sangcharim[] sangcharims = new Sangcharim[2];
	#endregion

	List<Action> workQueue = new List<Action>();

	const int CHO = 0;
	const int HAN = 1;

	void FixedUpdate()
	{
		if (workQueue.Count > 0)
		{
//			foreach (Action act in workQueue)
//				act();
//			workQueue.Clear();
			workQueue[0]();
			workQueue.RemoveAt(0);
		}
	}

	public JgGameRoom()
	{
		InitMap();
	}
	public void InitializeRoom()
	{
		roomIndex = -1;
		participants.Clear();
		spectators.Clear();

		InitMap();
		choState = null;
		hanState = null;
		sangcharims[0] = Sangcharim.kUnknown;
		sangcharims[1] = Sangcharim.kUnknown;
	}

	#region Init Maps
	void InitMap()
	{
		map[0]  = Jg.ChoCha;
		map[1]  = Jg.ChoMa;
		map[2]  = Jg.ChoSang;
		map[3]  = Jg.ChoSa;
		map[5]  = Jg.ChoSa;
		map[6]  = Jg.ChoMa;
		map[7]  = Jg.ChoSang;
		map[8]  = Jg.ChoCha;
		map[14] = Jg.ChoJang;
		map[21] = Jg.ChoPo;
		map[27] = Jg.ChoPo;
		map[30] = Jg.ChoJol;
		map[32] = Jg.ChoJol;
		map[34] = Jg.ChoJol;
		map[36] = Jg.ChoJol;
		map[38] = Jg.ChoJol;

		map[90] = Jg.HanCha;
		map[91] = Jg.HanMa;
		map[92] = Jg.HanSang;
		map[93] = Jg.HanSa;
		map[95] = Jg.HanSa;
		map[96] = Jg.HanMa;
		map[97] = Jg.HanSang;
		map[98] = Jg.HanCha;
		map[84] = Jg.HanJang;
		map[71] = Jg.HanPo;
		map[77] = Jg.HanPo;
		map[60] = Jg.HanJol;
		map[62] = Jg.HanJol;
		map[64] = Jg.HanJol;
		map[66] = Jg.HanJol;
		map[68] = Jg.HanJol;
	}
	#endregion
	
	#region Server works
	ReqSangcharim		reqSangcharim 	= new ReqSangcharim();
	ReqMovePawn			reqMovePawn 	= new ReqMovePawn();

	public void OnReadMessage(JStateObject state)
	{
		JgC2S_Msgs messageType = (JgC2S_Msgs) state.readBuffer[2];

		Debug.Log(string.Format("<color=green>R: <-C ({0}) - {1} </color> \n", state.name, messageType));

		switch (messageType)
		{
		case JgC2S_Msgs.kSangcharim:
			JThrift.Deserialize(state.readBuffer, Jg.Thrift_Offset, reqSangcharim);
			HandleMessage(state, reqSangcharim);
			break;
		case JgC2S_Msgs.kMovePawn:
			JThrift.Deserialize(state.readBuffer, Jg.Thrift_Offset, reqMovePawn);
			HandleMessage(state, reqMovePawn);
			break;

		default:
			Debug.Log(string.Format("<color=red>couldn't find a given case ( {0} )  </color>", messageType));
			break;
		}
	}
	#endregion

	#region Handle Messages Form Clients
	void HandleMessage(JStateObject state, ReqSangcharim req)
	{
		sangcharims[state.indexInRoom] = req.Sangcharim;

		Debug.Log(string.Format("<color=green>R: ({0}-{1}) has sent {2} </color> \n",
		                        state.indexInRoom, state.name, req.Sangcharim));

		if (sangcharims[0] != Sangcharim.kUnknown 
		    && sangcharims[1] != Sangcharim.kUnknown)
		{
			NtfSangcharim ntf = new NtfSangcharim();
			ntf.Cho = sangcharims[0];
			ntf.Han = sangcharims[1];
			
			NotifyMessageToAllClients((byte)JgS2C_Msgs.kNtfSangcharim, ntf);
			
			UpdateMapForSangcharim(ntf.Cho, ntf.Han);

			//this.Invoke("StartGame", 0.5f);
			//StartGame();
			workQueue.Add(StartGame);
			//this.Invoke("StartGame", 1f);
		}
		else if (state.indexInRoom == 1) // Is Han
		{
			NtfSangcharimHan ntf = new NtfSangcharimHan();
			ntf.Han = sangcharims[HAN];

			SendMessageToClient(choState, (byte)JgS2C_Msgs.kNtfSangcharimHan, ntf);
		}
	}
	void HandleMessage(JStateObject state, ReqMovePawn req)
	{
		NtfMovePawn ntf = new NtfMovePawn();
		ntf.LocalId 	= state.indexInRoom;
		ntf.Location 	= req.Location;
		ntf.Target 		= req.Target;
		ntf.Dummy		= 9999;

		NotifyMessageToAllClients((byte)JgS2C_Msgs.kNtfMovePawn, ntf);
	}
	#endregion

	//public JgNation virtualPlayerNation;
	public void SetPlayerNationsAndNewMatch()
	{
		NtfMatch ntf = new NtfMatch();
		bool isCho = false;//(new System.Random()).Next() % 2 == 0;
		//if (virtualPlayerNation == JgNation.kCho)

		if (isCho)
		{
			choState = states[0];
			hanState = states[1];
		}
		else
		{
			choState = states[1];
			hanState = states[0];
		}

		choState.indexInRoom = 0;
		hanState.indexInRoom = 1;

		ntf.LocalId = choState.indexInRoom;
		SendMessageToClient(choState, (byte)JgS2C_Msgs.kNtfMatch, ntf);

		ntf.LocalId = hanState.indexInRoom;
		SendMessageToClient(hanState, (byte)JgS2C_Msgs.kNtfMatch, ntf);
	}

	void StartGame()
	{
		Debug.Log("<color=green>R: -------------------</color> \n");
		Debug.Log("<color=green>R: StartGame          </color> \n");

		NtfChangeTurn ntf = new NtfChangeTurn();
		ntf.LocalId = choState.indexInRoom;
		NotifyMessageToAllClients((byte)JgS2C_Msgs.kNtfChangeTurn, ntf);

//		ntf.LocalId = choState.indexInRoom;
//		SendMessageToClient(choState, (byte)JgS2C_MessageType.kNtfChangeTurn, ntf);
//		ntf.LocalId = hanState.indexInRoom;
//		SendMessageToClient(hanState, (byte)JgS2C_MessageType.kNtfChangeTurn, ntf);
	}

	void UpdateMapForSangcharim(Sangcharim cho, Sangcharim han)
	{
		switch (cho)
		{
		case Sangcharim.kSMSM:
			map[1] = Jg.ChoSang;
			map[2] = Jg.ChoMa;
			map[6] = Jg.ChoSang;
			map[7] = Jg.ChoMa;
			break;
		case Sangcharim.kMSMS:
			map[1] = Jg.ChoMa;
			map[2] = Jg.ChoSang;
			map[6] = Jg.ChoMa;
			map[7] = Jg.ChoSang;
			break;
		case Sangcharim.kMSSM:
			map[1] = Jg.ChoMa;
			map[2] = Jg.ChoSang;
			map[6] = Jg.ChoSang;
			map[7] = Jg.ChoMa;
			break;
		case Sangcharim.kSMMS:
			map[1] = Jg.ChoSang;
			map[2] = Jg.ChoMa;
			map[6] = Jg.ChoMa;
			map[7] = Jg.ChoSang;
			break;
		}
		switch (han)
		{
		case Sangcharim.kSMSM:
			map[91] = Jg.ChoSang+10;
			map[92] = Jg.ChoMa+10;
			map[96] = Jg.ChoSang+10;
			map[97] = Jg.ChoMa+10;
			break;
		case Sangcharim.kMSMS:
			map[91] = Jg.ChoMa+10;
			map[92] = Jg.ChoSang+10;
			map[96] = Jg.ChoMa+10;
			map[97] = Jg.ChoSang+10;
			break;
		case Sangcharim.kMSSM:
			map[91] = Jg.ChoMa+10;
			map[92] = Jg.ChoSang+10;
			map[96] = Jg.ChoSang+10;
			map[97] = Jg.ChoMa+10;
			break;
		case Sangcharim.kSMMS:
			map[91] = Jg.ChoSang+10;
			map[92] = Jg.ChoMa+10;
			map[96] = Jg.ChoMa+10;
			map[97] = Jg.ChoSang+10;
			break;
		}
	}

	void SetNtfChangeTurn(int localId)
	{
	}
}
