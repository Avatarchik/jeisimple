using UnityEngine;
using System.Collections;

public class MgVCInGame : JViewController
{
	public EMatgoEvent testEvent;

	public void OnClick_TestMatgoEvent()
	{
		MgRefs.ins_.matgoGame.DoMatgoGameEvent(testEvent);
	}
}
