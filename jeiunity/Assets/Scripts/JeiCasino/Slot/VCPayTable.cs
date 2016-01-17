using UnityEngine;
using System.Collections;

public class VCPayTable : JViewController
{
	public GameObject[] pages;


	public void LeftPage()
	{
		Debug.Log("LeftPage \n");
	}
	public void RightPage()
	{
		Debug.Log("RightPage \n");
	}
	public void BackToGame()
	{
		Debug.Log("BackToGame \n");
		JVCMgr.ins_.PopView();
	}
}
