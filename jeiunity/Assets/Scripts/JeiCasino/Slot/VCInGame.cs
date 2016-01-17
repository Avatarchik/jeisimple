using UnityEngine;
using System.Collections;

public class VCInGame : JViewController
{
	public VCPayTable payTable;


	public void ShowPayTable()
	{
		JVCMgr.ins_.PushView(payTable);
	}
}
