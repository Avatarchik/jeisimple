using UnityEngine;
using System.Collections;

public enum JActivityDirection
{
	kCenter,
	kLeft,
	kRight,
	kUp,
	kDown,
	kNone
}

public class JViewController : MonoBehaviour
{
	public bool isPopup;
	public bool pausedOnPopup;
	public bool backButtonEnabled = true;

	public JActivityDirection entryDirection;

	public virtual void OnResume()
	{
	}
	public virtual void OnPause()
	{
	}
}
