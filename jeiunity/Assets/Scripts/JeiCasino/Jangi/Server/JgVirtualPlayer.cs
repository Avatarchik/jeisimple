using UnityEngine;
using System.Collections;
using jangi;
using Thrift.Protocol;
using System.Collections.Generic;


public class JgVirtualPlayer : MonoBehaviour
{
	public AsyncClient client;
	public JgNetClientMessageHandler msg;

	AnsLogin ansLogin;
	NtfMatch ntfMatch;
	NtfSangcharim ntfSangcharim;
	NtfChangeTurn ntfChangeTurn;

	public JgNation nation;
	public int localId;

	IEnumerator Start()
	{
		yield return null;

		client.Connect(JgRefs.ins_.app.serverIP, JgRefs.ins_.app.serverPort);

		yield return new WaitForSeconds(0.2f);

		DummyEntry();

		yield return new WaitForSeconds(0.2f);

		// Request a match
		if (ansLogin != null && ansLogin.Usn > 0)
		{
			ReqMatch req = new ReqMatch();

			msg.WriteToServer(JgC2S_Msgs.kReqMatch, req, OnNtfMatch);
		}

//		yield return new WaitForSeconds(1);
//
//		yield return new WaitForSeconds(1);
	}
	void OnEnable()
	{
		msg.onNtfTestString += OnNtfTestString;
		msg.onAnsLogin 		+= OnAnsLogin;
		msg.onNtfMatch 		+= OnNtfMatch;
		msg.onNtfSangcharim	+= OnNtfSangcharim;
		msg.onNtfChangeTurn += OnNtfChangeTurn;
		msg.onNtfMovePawn	+= OnNtfMovePawn;
	}
	void OnDisable()
	{
		msg.onNtfTestString -= OnNtfTestString;
		msg.onAnsLogin 		-= OnAnsLogin;
		msg.onNtfMatch 		-= OnNtfMatch;
		msg.onNtfSangcharim	-= OnNtfSangcharim;
		msg.onNtfChangeTurn -= OnNtfChangeTurn;
		msg.onNtfMovePawn	-= OnNtfMovePawn;
	}
	void FixedUpdate()
	{
	}

	IEnumerator TestFlow()
	{
		yield return null;
	}

	public void DummyEntry()
	{
		ReqLogin req = new ReqLogin();
		req.UserName = "serenitii@naver.com";
		req.LoginPlatform = "facebook";
		req.DeviceDesc = "unknown device";

		msg.SendMessageToServer(JgC2S_Msgs.kLogin, req);
	}

	public void PutPieceByInput(int selected, int target)
	{
		ReqMovePawn req = new ReqMovePawn();
		req.Location = selected;
		req.Target = target;
		msg.SendMessageToServer(JgC2S_Msgs.kMovePawn, req);
	}

	#region - 
	void OnNtfTestString(NtfTestString ntf)
	{
		Debug.Log(string.Format("VirtualPlayer.OnNtfTestString {0} \n",ntf.ToString()));
	}

	void OnAnsLogin(AnsLogin ans)
	{
		bool isLoginOk = ans.Usn > 0;
		if (isLoginOk)
		{	
			ansLogin = ans;

//			UpdateUserInfo(ans);
		}
		else
		{
			Debug.Log("failed to login " + ans.ToString());
		}
	}
	void OnNtfMatch(TBase message)
	{
//		Debug.Log("VirtualPlayer.OnNtfMatch \n" + message.ToString());

		NtfMatch msg = message as NtfMatch;
		//Debug.Log(string.Format("{0} got a match. localId({1}), \n{2}", ansLogin.NickName, msg.LocalId, message.ToString()));

		nation = msg.LocalId == 0 ? JgNation.kCho : JgNation.kHan;
		localId = msg.LocalId;

		this.Invoke("SendSangcharim", 1f);
	}
	void OnAnsCancelMatch(AnsCancelRequestMatch ans)
	{
//		Debug.Log("VirtualPlayer.OnAnsCancelMatch ");
	}
	void OnNtfSangcharim(NtfSangcharim ntf)
	{
//		Debug.Log("VirtualPlayer.OnNtfSangcharim \n" + ntf.ToString());
	}
	void OnNtfChangeTurn(NtfChangeTurn ntf)
	{
//		Debug.Log("VirtualPlayer.OnNtfChangeTurn \n" + ntf.ToString());

		OnTurnChanged(Jg.OpponentId(ntf.LocalId));
	}
	void OnNtfMovePawn(NtfMovePawn ntf)
	{
//		Debug.Log("VirtualPlayer.OnNtfMovePawn \n" + ntf.ToString());

		bool isMe = ntf.LocalId == localId;

		if (isMe)
		{
		}
		else
		{
			OnTurnChanged(localId); //Jg.OpponentId(ntf.LocalId));
		}
	}
	#endregion

	void SendSangcharim()
	{
		//if (nation == JgNation.kCho)
		{
			ReqSangcharim req = new ReqSangcharim();
			req.Sangcharim = Sangcharim.kMSMS;
			msg.SendMessageToServer(JgC2S_Msgs.kSangcharim, req);
		}
	}

	void RequestNextPawnMove()
	{
		ReqMovePawn req = new ReqMovePawn();
		req.Location = 11;
		req.Target = 22;
		msg.SendMessageToServer(JgC2S_Msgs.kMovePawn, req);
	}

	void OnTurnChanged(int localId)
	{
		bool isMe = this.localId == localId;
		if (isMe)
		{
//			RequestNextPawnMove();
		}
	}

}
