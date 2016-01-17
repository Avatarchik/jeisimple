using UnityEngine;
using System.Collections.Generic;

public class JVCMgr : MonoBehaviour
{
	[SerializeField]
	List<JViewController> viewQueue = new List<JViewController>(8);

	public GameObject blockWindow;

	//JViewController previous;
	JViewController current;

	[SerializeField]
	JViewController timerWindow;

	public static JVCMgr ins_;
	void Awake()
	{
		JVCMgr.ins_ = this;
	}
	void Update()
	{
	}

	public JViewController PushView(JViewController vc)
	{
		if (vc.isPopup)
		{
			if (blockWindow != null)
				blockWindow.SetActive(true);
		}
		else
		{
			if (current != null)
			{
				current.OnPause();
				current.gameObject.SetActive(false);
			}
		}
		//previous = current;
		current = vc;
		current.gameObject.SetActive(true);

		switch (vc.entryDirection)
		{
		case JActivityDirection.kCenter:
			break;
		}

		vc.OnResume();
		viewQueue.Add(vc);

		return vc;
	}

	public void PopView()
	{
		JViewController current = viewQueue[viewQueue.Count - 1];
		current.OnPause();
		current.gameObject.SetActive(false);

		JViewController previous = viewQueue.Count >= 2 ? viewQueue[viewQueue.Count - 2] : null;

		if (current.isPopup)
		{
			if (blockWindow != null)
				blockWindow.SetActive(false);
		}
		else
		{
			previous.OnResume();
			previous.gameObject.SetActive(true);
		}

		this.current = previous;

		viewQueue.RemoveAt(viewQueue.Count - 1);
	}

	public void PopupInTimer(JViewController window, float seconds)
	{
		timerWindow = window;
		timerWindow.gameObject.SetActive(true);

		this.Invoke("ResetTimerWindow", seconds);
	}

	void ResetTimerWindow()
	{
		timerWindow.gameObject.SetActive(false);
		timerWindow = null;
	}
}
