using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using jangi;
using System.Collections.Generic;



public class JgVCLobby : JViewController 
{
	public Text nickName;
	public Text debugMessages;
	public Text reqNewGameText;

	AnsLogin ansLogin;
	NtfMatch ntfMatch;

	JgNetClientMessageHandler msg;

	#region MonoBehaviour
	void Start()
	{
//		this.StartCoroutine(StartAppPlayer());
	}
	void OnEnable()
	{
		msg = JgRefs.ins_.msg;
		msg.onNtfTestString += OnNtfTestString;
		msg.onAnsLogin += OnAnsLogin;
		msg.onNtfMatch += OnNtfMatch;
//		JgNetClientMessageHandler.ins_.onAnsCancelMatch += OnAnsCancelMatch;
	}
	void OnDisable()
	{
		msg.onNtfTestString -= OnNtfTestString;
		msg.onAnsLogin -= OnAnsLogin;
		msg.onNtfMatch -= OnNtfMatch;
//		JgNetClientMessageHandler.ins_.onAnsCancelMatch -= OnAnsCancelMatch;
	}
	#endregion

	public void LogIn()
	{
		Debug.Log("\n");
		Debug.Log("<color=magenta>------------------------------------------------------------\nAppClient trying to login</color>");

		if (JgRefs.ins_.appClient.IsBound() == false)
			return;

		ReqLogin req = new ReqLogin();
		req.UserName = "origin201@gmail.com";
		req.LoginPlatform = "facebook";
		req.DeviceDesc = "unknown device";

		JgRefs.ins_.msg.SendMessageToServer(JgC2S_Msgs.kLogin, req);
	}
	
	#region Answer handlers for the requests  ( Not Main Thread ) 
	void OnNtfTestString(NtfTestString ntf)
	{
//		Debug.Log("Lobby.OnNtfTestString \n" + ntf.ToString());
	}

	void OnAnsLogin(AnsLogin ans)
	{
		//Debug.Log(string.Format("Lobby.OnAnsLogin - {0} \n", ans.ToString()));
		bool isLoginOk = ans.Usn > 0;
		if (isLoginOk)
		{
			ansLogin = ans;

			UpdateUserInfo(ans);
		}
		else
		{
			Debug.Log("<color=red>failed to login </color>" + ans.ToString());
		}
	}
	void OnNtfMatch(NtfMatch ntf)
	{
		//Debug.Log("Lobby.OnNtfMatch " + ntf.ToString() + "\n");

		JgRefs.ins_.appClient.StateObject().indexInRoom = ntf.LocalId;

		JVCMgr.ins_.PopView();

		bool isCho = ntf.LocalId == 0;
		JgRefs.ins_.game.appPlayerIsCho = isCho;

		if (isCho)
			JgRefs.ins_.vcSangcharimCho.ShowPopup();
		else
			JgRefs.ins_.vcSangcharimHan.ShowPopup();
	}
	void OnAnsCancelMatch(AnsCancelRequestMatch ans)
	{
//		Debug.Log("Lobby.OnAnsCancelMatch " + ans.ToString());
	}
	#endregion

	#region Update UI
	void UpdateUserInfo(AnsLogin ans)
	{
		nickName.text = string.Format("{0} {1}급", ansLogin.NickName, ansLogin.Level);

		debugMessages.text = ans.Comment;
	}

	void UpdateChannel(AnsChannelInfo ans)
	{
	}
	#endregion

	#region UI Handlers
	public void OnClick_RequestMatch()
	{
		reqNewGameText.text = "대국 신청 중..";

//		ReqMatch req = new ReqMatch();
//		JgRefs.ins_.appMessageHandler.SendMessageToServer(JgC2S_MessageType.kReqMatch, req);
		this.Invoke("DelayedRequestMatch", 1f);

		JVCMgr.ins_.PushView(JgRefs.ins_.vcWaitingMatch);
	}
	void DelayedRequestMatch()
	{
		ReqMatch req = new ReqMatch();
		JgRefs.ins_.msg.SendMessageToServer(JgC2S_Msgs.kReqMatch, req);

	}
	#endregion

	public void OnClick_StartAppPlayer()
	{
		this.StartCoroutine(StartAppPlayer());
	}
	IEnumerator StartAppPlayer()
	{
		yield return null;

		JgRefs.ins_.appClient.Connect(JgRefs.ins_.app.serverIP, JgRefs.ins_.app.serverPort);

//		yield return new WaitForSeconds(0.5f);
//		JgRefs.ins_.vcLobby.DummyEntry();
	}
}
