using UnityEngine;
using System.Collections;

public class JgFlowTester : MonoBehaviour 
{

	void Start () 
	{
		if (JgRefs.ins_.app.routeTestEnabled == false)
			StartCoroutine(this.TestFlow());
	}
	
	IEnumerator TestFlow()
	{
		yield return null;
		JgRefs.ins_.appClient.Connect(JgRefs.ins_.app.serverIP, JgRefs.ins_.app.serverPort);

		yield return new WaitForSeconds(2f);
		yield return new WaitForSeconds(0.5f);
		JgRefs.ins_.vcLobby.LogIn();
	}
}
