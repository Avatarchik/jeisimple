using UnityEngine;
using System.Collections;

public class VCSlotTourneyLobby : JViewController
{
	public GameObject[] tabImages;
	public GameObject[] tabContents;

	const int kTournaments = 0;
	const int kHistory = 1;
	const int hInfomation = 2;

	public void BackToLobby()
	{
		JVCMgr.ins_.PopView();
	}
	public void ShowTabContent(int index)
	{
		Debug.Log("ShowTabContent " + index);
		foreach (GameObject go in tabImages)
			go.SetActive(false);
		tabImages[index].SetActive(true);

		foreach (GameObject go in tabContents)
			go.SetActive(false);
		tabContents[index].SetActive(true);
	}

}
