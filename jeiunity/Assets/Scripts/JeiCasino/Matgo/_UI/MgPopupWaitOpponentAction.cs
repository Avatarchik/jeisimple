using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public enum MgOpponentActions
{
	kStartGame,
	kSelectCard,
	kSelectGukjinPi,

}

public class MgPopupWaitOpponentAction : JViewController
{
	public string[] messages;

	public Text messageText;

	int currentAction;

	public void Popup(MgOpponentActions action)
	{
		gameObject.SetActive(true);

		currentAction = (int) action;

		messageText.text = messages[currentAction];
	}

	public void CloseWindow()
	{
		gameObject.SetActive(false);
	}
}
