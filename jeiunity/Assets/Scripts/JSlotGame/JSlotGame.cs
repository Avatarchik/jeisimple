using UnityEngine;
using System.Collections;



public class JSlotGame : MonoBehaviour
{
	public JSlotMaker maker;
	
	int numberOfReels;
//	int reelHeight;

	GameObject reelContainer;
	JSlotReel2[] reels;


	#region Initialization
	public void InitializeSlotGame()
	{
		reelContainer = new GameObject("Reels");
		reelContainer.transform.parent = this.transform;
		reelContainer.transform.localPosition = Vector3.zero;

		InitWithMaker();
		CreateReels();
	}

	void InitWithMaker()
	{
		this.numberOfReels = maker.numberOfReels;
//		this.reelHeight = maker.reelHeight;
	}

	void CreateReels()
	{
		reels = new JSlotReel2[numberOfReels];

		for (int i = 0; i < numberOfReels; ++i)
		{
			GameObject newOne = new GameObject("Reel-" + i);
			newOne.transform.parent = reelContainer.transform;
			float xPos = CalcXPositionOfReel(i);
			newOne.transform.localPosition = new Vector3(xPos, 0, 0);

			newOne.SetActive(false);
			newOne.AddComponent(typeof(JSlotReel2));
			newOne.SetActive(true);

			JSlotReel2 reelScript = newOne.GetComponent<JSlotReel2>();
			reelScript.CreateReelSymbols(this, i, maker.reelHeight);
			reels[i] = reelScript;
		}
	}

	float CalcXPositionOfReel(int reelIndex)
	{
		float length = maker.cellWidth * ((float)numberOfReels - 1f);
		return -length * 0.5f + maker.cellWidth * (float)reelIndex;
	}
	#endregion

	#region Spin
	public void Spin()
	{
		SpinImpl();
	}
	void SpinImpl()
	{
		SetState(JSlotState.kSpinning);

		for (int i = 0; i < reels.Length; ++i)
		{
			reels[i].SpinReel();
		}
	}
	#endregion

	void SetState(JSlotState state)
	{
	}
}
