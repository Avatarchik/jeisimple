using UnityEngine;
using System.Collections;


public enum EMatgoEvent
{
	kOpponentParticipateInGame,

	kDecideFirst,
	kDeciedFirst,
	kDealCardsInFirstTime,

	kShowRecommendableCards,
	kShowShakableCards,

	kPutCard,
	kTurnStackInsideOut,


	kShowCurrentTurn,
	kShowPlayerScore,
	kShowPpeok, // 뻑 
	kShowJaPpeok, // 자뻑
	kShowSsaDa, // 쌋어요

	kPopupTimer,
	kPopupMissionInfo, 
	kPopupSelectOneInTwoCards,

	kPopupOpponentDecidingStart, // 상대 게임시작 대기중..
	kPopupOpponentSelectingCard,
	kPopupOpponentSelectingGukjinPi, // 국진, 피 사용선택..

	kPopupWaitingForOpponentAction,

	kPopupAskPlayerShake, // 흔들었습니다
	kPopupNotifyPlayerShaked,
	kPopupSucceededInMission,
	kPopupOtherSideConsidering,
	kPopupOtherSideConsidering_WhetherGoOrStop,
	kPopupWhetherGoOrStop, // 고하시겠습니까?
	kPopupNthGo,
	kPopupGukJinToPi, // 국진을 피로 쓸까요?
	kPopupGukJinTo10Keot, //국진 열끗 위치 선택중..
	kPopupShowStop,
	kPopupGameResult,
	
	kShowNearAtCheongDan, // 청단 임박 표시
	kShowNearAtHongDan, // 홍단 임박 표시 
	kShowChodan,
	kShowCheongDan,
	kShowHongDan,
	kShowGoDoRi,
	kShowSamGwang,
	kShowSaGwang,
	kShowOGwang,

	kShowGoBak,
	kShowPiBak,
	//kShowDokBak,
	kShowGwangBak,
	kShowMeongBak,
	kShowShake, //흔들기

	kShowTsok, //쪽 


}

public class MatgoGame : MonoBehaviour
{
	IEnumerator Start()
	{
		yield return new WaitForSeconds(2f);

		MgUserInfo opponent = new MgUserInfo();
		opponent.SetValues("긔요미", 98341042300, 0);

		OpponentParticipateInGame(opponent);
	}

	public void OpponentParticipateInGame(MgUserInfo opponent)
	{
	}

	public void DoMatgoGameEvent(EMatgoEvent gameEvent, int arg0 = -1, int arg1 = -1)
	{
		switch (gameEvent)
		{
		case EMatgoEvent.kPopupTimer:
			//JVCMgr.ins_.PushView(MgRefs.ins_.popupTimer);
			MgRefs.ins_.popupTimer.EnableTimer(3);
				break;
		case EMatgoEvent.kPopupMissionInfo:
			JVCMgr.ins_.PopupInTimer(MgRefs.ins_.popupMissionInfo, 2f);
			break;
		case EMatgoEvent.kPopupSelectOneInTwoCards:
			JVCMgr.ins_.PushView(MgRefs.ins_.popupSelectOneCardInTwo);
			break;

		case EMatgoEvent.kPopupOpponentDecidingStart: // 상대 게임시작 대기중..
			MgRefs.ins_.popupWaitOpponentAction.Popup(MgOpponentActions.kStartGame);
			this.Invoke("TempDelayed", 5f);
			break;
		case EMatgoEvent.kPopupOpponentSelectingCard:
			MgRefs.ins_.popupWaitOpponentAction.Popup(MgOpponentActions.kSelectCard);
			this.Invoke("TempDelayed", 5f);
			break;
		case EMatgoEvent.kPopupOpponentSelectingGukjinPi:
			MgRefs.ins_.popupWaitOpponentAction.Popup(MgOpponentActions.kSelectGukjinPi);
			this.Invoke("TempDelayed", 5f);
			break;


		case EMatgoEvent.kPopupAskPlayerShake: // 흔드시겠습니까?
			MgRefs.ins_.popupTimeredAsk.Popup(EPlayerActions.kWhetherShake);
			break;

		case EMatgoEvent.kPopupNotifyPlayerShaked:
			MgRefs.ins_.popupNotifyPlayerAction.Popup(NotifcationPlayerActions.kPlayerShaked, 3f);
			break;

		case EMatgoEvent.kPopupSucceededInMission:
		case EMatgoEvent.kPopupOtherSideConsidering:
		case EMatgoEvent.kPopupOtherSideConsidering_WhetherGoOrStop:
		case EMatgoEvent.kPopupWhetherGoOrStop: // 고하시겠습니까?
		case EMatgoEvent.kPopupNthGo:
		case EMatgoEvent.kPopupGukJinToPi: // 국진을 피로 쓸까요?
		case EMatgoEvent.kPopupGukJinTo10Keot: //국진 열끗 위치 선택중..
		case EMatgoEvent.kPopupShowStop:
		case EMatgoEvent.kPopupGameResult:
			break;
		}
	}

	void TempDelayed()
	{
		MgRefs.ins_.popupWaitOpponentAction.CloseWindow();
	}
}


