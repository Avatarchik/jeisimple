using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using jangi;

    
public class JgVCInGame : JViewController
{
	public GameObject pickingContainer;
	public Image waitingSangcharim;
	public JgUIPlayerStatus playerStatusLeft;
	public JgUIPlayerStatus playerStatusRight;

	Sangcharim sangcharim;
	//bool appPlayerIsCho;
	int appPlayerId = -1;
	JgNetClientMessageHandler msg;
	//JgGame game;

	void OnEnable()
	{
		msg = JgRefs.ins_.msg;
		msg.onNtfSangcharim += OnNtfSangcharim;
		msg.onNtfChangeTurn += OnNtfChangeTurn;
		msg.onNtfMovePawn	+= OnNtfMovePawn;
	}
	void OnDisable()
	{
		msg.onNtfSangcharim -= OnNtfSangcharim;
		msg.onNtfChangeTurn -= OnNtfChangeTurn;
		msg.onNtfMovePawn	-= OnNtfMovePawn;
	}
	
	#region Message Handlers
	void OnNtfSangcharim(NtfSangcharim ntf)
	{
		this.appPlayerId = JgRefs.ins_.appClient.StateObject().indexInRoom;

		JVCMgr.ins_.PopView();

		StartGame();
		JgRefs.ins_.game.InitGame(JgRefs.ins_.game.appPlayerIsCho, appPlayerId);
	}
	void OnNtfChangeTurn(NtfChangeTurn ntf)
	{
		UpdateCurrentTurnUI(ntf.LocalId);

		bool isAppUser = appPlayerId == ntf.LocalId;

		SetUserInputActive(isAppUser);
	}
	void OnNtfMovePawn(NtfMovePawn ntf)
	{
		//Debug.Log("InGame.OnNtfMovePawn \n" + ntf.ToString() + "\n");
		//Debug.Log(string.Format("MovePawn {0} -> {1} \n", ntf.Location, ntf.Target));

		//game.PutInPiece(ntf.Location, ntf.Target);
		if (ntf.LocalId != appPlayerId)
		{
			JgRefs.ins_.game.PutPiece(ntf.Location, ntf.Target);
		}

		UpdateCurrentTurnUI(Jg.OpponentId(ntf.LocalId));
	}
	#endregion

	public 
	void Show(bool displayWaitingSangcharim)
	{
		JVCMgr.ins_.PushView(this);
		JgRefs.ins_.game.gameObject.SetActive(true);

		if (displayWaitingSangcharim)
		{
			JgRefs.ins_.vcWaitMessagePopup.ShowMessage("상대방이 상차림을 선택하는 중입니다.");
		}
	}

	void SetUserInputActive(bool active)
	{
		pickingContainer.SetActive(active);
	}

	void UpdateCurrentTurnUI(int currentId)
	{
		if (currentId == appPlayerId)
		{
			playerStatusLeft.SetTurnEnabled();
			playerStatusRight.SetTurnDisabled();
		}
		else
		{
			playerStatusLeft.SetTurnDisabled();
			playerStatusRight.SetTurnEnabled();
		}
	}

	void StartGame()
	{
//		JgRefs.ins_.game.gameObject.SetActive(true);
	}
}
