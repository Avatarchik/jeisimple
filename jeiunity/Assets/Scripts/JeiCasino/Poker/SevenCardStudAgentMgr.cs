using UnityEngine;
using System.Collections;



// control agents whose act enter, exit, start game as a boss, 
public class SevenCardStudAgentMgr : MonoBehaviour 
{

	public static SevenCardStudAgentMgr ins_;

//	SevenCardStudAgent[] agents;

	void Start()
	{
		//StartCoroutine(SetupAgentEntry());
	}

//	public SevenCardStudAgent GetAgent(int index)
//	{
//		return agents[index];
//	}

	IEnumerator SetupAgentEntry()
	{
		yield return new WaitForSeconds(1f);
//		agents[0].RequestEnterRoom();
//
//		yield return new WaitForSeconds(1f);
//		agents[1].RequestEnterRoom();
//
//		yield return new WaitForSeconds(1f);
//		agents[2].RequestEnterRoom();
//
//		yield return new WaitForSeconds(1f);
//		agents[3].RequestEnterRoom();

	}
}
