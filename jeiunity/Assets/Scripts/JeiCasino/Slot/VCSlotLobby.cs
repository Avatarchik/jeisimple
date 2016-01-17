using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class VCSlotLobby : JViewController
{
	public Text fpsText;
	int numFrames = 0;
	float accum = 0;


	void Update () 
	{
		CalculateFPS();
	}
	
	void CalculateFPS()
	{
		++numFrames;
		accum += Time.deltaTime;
		
		if (accum >= 1.0f)
		{
			//fpsLabel.text = numFrames.ToString();
			fpsText.text = numFrames.ToString();
			
			numFrames = 0;
			accum = 0;
		}
	}

	public void MoveToTourneyLobby()
	{
		Debug.Log("MoveToTourneyLobby \n");

		JVCMgr.ins_.PushView(Refs.ins_.slotTourneyLobby);
	}
	public void GetGifts()
	{
		Debug.Log("GetGifts \n");
		JVCMgr.ins_.PushView(Refs.ins_.gifts);
	}

	public void RunGame(int index)
	{
		switch (index)
		{
		case 0:
			Application.LoadLevel(1);
			break;
		case 1:
			Application.LoadLevel(2);
			break;
		}
	}
}
