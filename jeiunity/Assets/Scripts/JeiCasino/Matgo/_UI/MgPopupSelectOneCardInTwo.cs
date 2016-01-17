using UnityEngine;
using System.Collections;

public class MgPopupSelectOneCardInTwo : JViewController
{

	public void OnClick_SelectCard(int index)
	{
		Debug.Log(string.Format("{0} chosen \n", index));

		//gameObject.SetActive(false);

		this.Invoke("CloseWindow", 0.12f);
	}

	void CloseWindow()
	{
		JVCMgr.ins_.PopView();
	}
}
