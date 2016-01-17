using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using jangi;
using System.Collections.Generic;



public class JgVCPopupWaitingMatch : JViewController
{

	public void OnClick_CancelRequestMatch()
	{
		ReqCancelRequestMatch req = new ReqCancelRequestMatch();
		JgRefs.ins_.msg.SendMessageToServer(JgC2S_Msgs.kReqCancelMatch, req);

		JVCMgr.ins_.PopView();
	}
}
