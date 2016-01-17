using UnityEngine;
using System.Collections;

public class JgVCDevTest : JViewController
{

	public void OnClick_RunServer()
	{
		JgRefs.ins_.testServer.RunServer();
	}
	public void OnClick_RunClient()
	{
		JgRefs.ins_.testAsyncClient.Connect("127.0.0.1", TestServer.PORT);
	}
}
