using UnityEngine;
using System.Collections;

public class VCIntro : JViewController
{
	IEnumerator Start()
	{
		yield return null;

		if (FB.IsLoggedIn)
		{
		}
		else
		{
			//JFacebook.ins_.InitializeFacebook();
		}

		//Refs.ins_.slotLobby.gameObject.SetActive(false);

		yield return new WaitForSeconds(1f);

		if (Refs.appMgr.facebookEnabled)
			JVCMgr.ins_.PushView(Refs.ins_.loginSignup);
		else
			JVCMgr.ins_.PushView(Refs.ins_.slotLobby);

//		if (FB.IsLoggedIn)
//		{
//			JVCMgr.ins_.PushView(Refs.ins_.loginSignup);
//		}
//		else
//		{
//			JVCMgr.ins_.PushView(Refs.ins_.dailyBonus);
//		}
	}


}
