using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class JgUIPlayerStatus : MonoBehaviour
{
	public Image profilePic;
	public Text nickName;
	public Image turnFlag;
	public Text score;
	public Text time;
	public Text timeCount;
	public Image nationImage;

	[SerializeField]
	int leftSeconds = 60 * 5;


	public void SetTurnEnabled()
	{
		turnFlag.color = Color.white;

		this.InvokeRepeating("RepeatTimeCountOff", 1f, 1f);
	}
	public void SetTurnDisabled()
	{
		turnFlag.color = Color.gray;
		this.CancelInvoke("RepeatTimeCountOff");
	}

	void RepeatTimeCountOff()
	{
		if (leftSeconds > 0)
			--leftSeconds;

		int min = leftSeconds / 60;
		int sec = leftSeconds % 60;
		string timeString = string.Format("{0:00} : {1:00}", min, sec);
		time.text = timeString;
	}
}
