using UnityEngine;
using System.Collections;

public class MgRefs : MonoBehaviour
{
	public MatgoGame matgoGame;
	//public MatgoRules matgoRules;

	public JViewController popupMissionInfo;
	public MgPopupTimer popupTimer;
	public JViewController popupSelectOneCardInTwo;
	public MgPopupWaitOpponentAction popupWaitOpponentAction;
	public MgPopupTimeredAsk popupTimeredAsk;
	public MgPopupNotifyPlayerActions popupNotifyPlayerAction;


	public static MgRefs ins_;
	void Awake()
	{
		MgRefs.ins_ = this;
	}
}
