using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class JgVCPopupMessages : JViewController 
{
	public Text messageText;


	public void ShowMessage(string message)
	{
		messageText.text = message;
		JVCMgr.ins_.PushView(this);
	}
}
