using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum EPlayerActions
{
	kWhetherShake,
}

public class MgPopupTimeredAsk : JViewController
{
	public Text messageText;
	public Slider timeSlider;

	public string[] messages;
	
	int currentAction;

	public void OnClick_Yes()
	{
		CloseWindow();
	}
	public void OnClick_No()
	{

		CloseWindow();
	}

	public void Popup(EPlayerActions action)
	{
		gameObject.SetActive(true);
		
		currentAction = (int) action;
		
		messageText.text = messages[currentAction];
	}


	void CloseWindow()
	{
		gameObject.SetActive(false);
	}
}
