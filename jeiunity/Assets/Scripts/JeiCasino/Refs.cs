using UnityEngine;
using System.Collections;

public class Refs : MonoBehaviour
{


	public VCIntro intro;
	public VCLoginSignup loginSignup;
	public VCDailyBonus dailyBonus;
	public VCCasinoLobby casinoLobby;
	public VCSlotLobby slotLobby;
	public VCSlotTourneyLobby slotTourneyLobby;
	public VCPopGifts gifts;

	public VCPopInvite invite;
	public VCPopSettings settings;

	public static AppMgr appMgr;
	public static Refs ins_;

	public Refs()
	{
		Refs.ins_ = this;
	}
	void Awake()
	{
		Refs.appMgr = AppMgr.ins_;
	}
}
