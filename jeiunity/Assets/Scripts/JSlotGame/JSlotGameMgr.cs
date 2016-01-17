using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class JSlotGameMgr : MonoBehaviour {

	public JSlotGame slot;
	public UILabel fpsLabel;
	public Text fpsText;
	int numFrames = 0;
	float accum = 0;
	

	void Start () 
	{
		Application.targetFrameRate = 40;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		DOTween.Init();

		slot.InitializeSlotGame();
	}
	// Update is called once per frame
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

	public void ExitToLobby()
	{
		Application.LoadLevel(slot.maker.slotLobbySceneName);
	}
}
