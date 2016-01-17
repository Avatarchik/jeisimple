using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MgPopupTimer : JViewController
{
	public Text timeText;

	int loadedTime;
	int leftSeconds;


	public void EnableTimer(int seconds)
	{
		gameObject.SetActive(true);
		loadedTime = seconds;
		leftSeconds = seconds;

		StartCoroutine(RunTimer());
	}

	IEnumerator RunTimer()
	{
		for (int i = 0; i < loadedTime; ++i)
		{
			timeText.text = leftSeconds.ToString();

			yield return new WaitForSeconds(1f);
			--leftSeconds;
		}
		timeText.text = leftSeconds.ToString();

		yield return new WaitForSeconds(0.4f);
		gameObject.SetActive(false);
	}
}
