using UnityEngine;
using System.Collections;
using jangi;


public class JgAppMgr : MonoBehaviour
{
	public bool realRemoteServer = true;
	public string serverIP = "127.0.0.1";
	public int serverPort = 9001;
	public bool routeTestEnabled;
	//public bool networkEnabled;

	void Awake()
	{
		Screen.orientation = ScreenOrientation.Portrait;
	}

	IEnumerator Start()
	{
		JgRefs.ins_.vcLobby.gameObject.SetActive(false);
		JgRefs.ins_.vcInGame.gameObject.SetActive(false);
		JgRefs.ins_.game.gameObject.SetActive(false);

		yield return null;

		if (routeTestEnabled)
		{
			JgRefs.ins_.game.InitGame(true, 0);
		}
		else
			JVCMgr.ins_.PushView(JgRefs.ins_.vcLobby);
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();
		if (Input.GetKey(KeyCode.M))
		{
			JVCMgr.ins_.PushView(JgRefs.ins_.vcDevTest);
		}
	}
}
