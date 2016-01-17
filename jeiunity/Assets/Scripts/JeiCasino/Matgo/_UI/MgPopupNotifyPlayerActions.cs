using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public enum NotifcationPlayerActions
{
	kPlayerShaked,
}

public class MgPopupNotifyPlayerActions : MonoBehaviour
{
	public Text messageText;

	public string[] messages;
	
	int currentAction;

	public AudioSource[] voices;

	
	public void Popup(NotifcationPlayerActions action, float time)
	{
		gameObject.SetActive(true);
		
		currentAction = (int) action;
		
		messageText.text = messages[currentAction];

		this.Invoke("CloseWindow", time);

		if (voices[currentAction] != null)
			voices[currentAction].Play();
	}

	public void CloseWindow()
	{
		gameObject.SetActive(false);
	}
}
