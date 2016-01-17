using UnityEngine;
using System.Collections;

public class JgRefs : MonoBehaviour
{
	public JgAppMgr app;
	public JgRes res;
	public JgGame game;
	public JgGameLogic gameLogic;
	public JgNetClientMessageHandler msg;
	public AsyncClient appClient;

	public JangiDummyDb dummyDb;
	public JgGameRoom server;
	public TestServer testServer;
	public TestClient testClient;
	public AsyncClient testAsyncClient;
	public JgVirtualPlayer virtualPlayer;

	public JgVCDevTest vcDevTest;
	public JgVCLobby vcLobby;
	public JgVCInGame vcInGame;

	public JgVCPopupMessages vcWaitMessagePopup;

	public JgVCPopupWaitingMatch vcWaitingMatch;
	public JgVCPopupSangcharim vcSangcharimCho;
	public JgVCPopupSangcharim vcSangcharimHan;
	
	public static JgRefs ins_;

	void Awake()
	{
		JgRefs.ins_ = this;
	}
}
