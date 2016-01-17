using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public enum E7CardStudFlow
{
	kGameStarted,
	kRoundStarted,
	kGameEnded
}

public class SevenCardStudRoomMockup : MonoBehaviour
{
	//JCardPlayer[] playerPool;
	JCardPlayer[] players = new JCardPlayer[Max_Users];// clock-wise


	public SevenCardStud tempMainUI;

	// Game, Round, Turn,

	public event Action<JCardPlayer> OnPlayerEnterRoom;
	public event Action<JCardPlayer> OnPlayerExitRoom;
	//void OnInitial4CardsDeal(JCardPlayer player, int[] cards)
	public event Action<JCardPlayer, int[]> OnInitial4CardsDeal;


	const int Max_Users = 5;

	#region MonoBehaviour
	void Start()
	{
		this.StartCoroutine(TestGameFlow());
	}
	#endregion

	void GameLoop()
	{
	}

	public void RequestGameStart()
	{
	}

	#region Events
	void OnGameStarted()
	{
	}
	void OnInitialFourCardsDealt()
	{
	}
	// 
	void OnInitialFourCardsTurnClosed()
	{
	}
	#endregion

	#region
	void OnInitialCardsSelected(int playerId, int opened, int fold)
	{
	}
	void OnPlayerBet(int playerId, JSevenCardStudBetType bet)
	{
	}
	#endregion

	#region Room System
	void OnPlayerEnteredRoom()
	{
	}
	void OnPlayerExitRoot()
	{
	}
	#endregion
	
	void StartGame()
	{
		int[] cards = { 1, 2, 3, 4 };

		for (int i = 0; i < players.Length; ++i)
		{
			DealInitialFourCards(players[i].LocalId, cards);
		}
	}

	void DealInitialFourCards(long playerId, int[] cards)
	{
	}

	#region -
	public void RequestEnterRoom(long playerId)
	{
	}
	#endregion


//	#region 'Seven Card Stud' Game Logic Events
//	void OnInitial4CardsDeal(JCardPlayer player, int[] cards)
//	{
//	}
//	void OnInitial4CardsResult(JCardPlayer player, int openedCard, int removedCard)
//	{
//	}
//	void OnCardDeal(int round, JCardPlayer[] players, int[] cardNumbers)
//	{
//	}
//	void OnPlayerBet(JCardPlayer player, JSevenCardStudBetType bet)
//	{
//	}
//	void OnPlayersShowdown(JCardPlayer[] players, JSevenCardStudHand[] hands, string[] handDesc)
//	{
//	}
//	#endregion

	#region Test Thrift

	void TestThrift()
	{
	}

	#endregion


	#region Test
	IEnumerator TestGameFlow()
	{
		{
			yield return new WaitForSeconds(1f);
			PutPlayerIntoRoom(0, "IJJ", 32320000);
			
			yield return new WaitForSeconds(1f);
			PutPlayerIntoRoom(1, "Jane", 983183320000);
		}

		List<jcasino.HandInfo> firstPlayerHands = new List<jcasino.HandInfo>(8);
		firstPlayerHands.Add(new jcasino.HandInfo());
		firstPlayerHands[0].PlayerLocalId = 0;
		firstPlayerHands[0].Hand = new List<int>();
		firstPlayerHands[0].Hand.Add(1);
		firstPlayerHands[0].Hand.Add(11);
		firstPlayerHands[0].Hand.Add(12);
		firstPlayerHands[0].Hand.Add(13);

		firstPlayerHands.Add(new jcasino.HandInfo());
		firstPlayerHands[1].PlayerLocalId = 1;
		firstPlayerHands[1].Hand = new List<int>();
		firstPlayerHands[1].Hand.Add(20);
		firstPlayerHands[1].Hand.Add(21);
		firstPlayerHands[1].Hand.Add(22);
		firstPlayerHands[1].Hand.Add(23);
		{
			yield return new WaitForSeconds(1f);
			DealInitial4Cards(firstPlayerHands[0]);

			yield return new WaitForSeconds(1f);
			DealInitial4Cards(firstPlayerHands[1]);
		}

		// user-input
		yield return new WaitForSeconds(1f);
		{
			yield return new WaitForSeconds(1f);
			SetPlayerResultInitial4Cards(0, 11, 12);

			yield return new WaitForSeconds(1f);
			SetPlayerResultInitial4Cards(1, 22, 23);
		}

//		List<JCardHandData> secondPlayerHands = new List<JCardHandData>(8);
//		// 2장받고 보스부터 베팅 
//		{
//			yield return new WaitForSeconds(1f);
//			DealSecond2Cards(secondPlayerHands[0]);
//
//			yield return new WaitForSeconds(1f);
//			DealSecond2Cards(secondPlayerHands[1]);
//		}
//
//		// 2개 라운드 베팅, 2번째 라운드 마지막은 no-bet(?)
//		{
//			yield return new WaitForSeconds(1f);
//			Bet(0, JSevenCardStudBetType.kHalf);
//			yield return new WaitForSeconds(1f);
//			Bet(1, JSevenCardStudBetType.kHalf);
//
//			yield return new WaitForSeconds(1f);
//			Bet(0, JSevenCardStudBetType.kCall);
//			yield return new WaitForSeconds(1f);
//			Bet(1, JSevenCardStudBetType.kCall);
//		}
//
//		List<JCardHandData> thirdPlayerHands = new List<JCardHandData>(8);
//		// 1장받고
//		{
//			yield return new WaitForSeconds(1f);
//			DealThird1Cards(thirdPlayerHands[0]);
//
//			yield return new WaitForSeconds(1f);
//			DealThird1Cards(thirdPlayerHands[1]);
//		}
//		{
//			yield return new WaitForSeconds(1f);
//			Bet(0, JSevenCardStudBetType.kCheck);
//			yield return new WaitForSeconds(1f);
//			Bet(1, JSevenCardStudBetType.kCall);
//
//			yield return new WaitForSeconds(1f);
//			Bet(0, JSevenCardStudBetType.kCheck);
//			yield return new WaitForSeconds(1f);
//			Bet(1, JSevenCardStudBetType.kCall);
//		}
//
//		List<JCardHandData> fourthPlayerHands = new List<JCardHandData>(8);
//		{
//			yield return new WaitForSeconds(1f);
//			DealFourth1Cards(fourthPlayerHands[0]);
//			
//			yield return new WaitForSeconds(1f);
//			DealFourth1Cards(fourthPlayerHands[1]);
//		}
//
//		// last bet
//		{
//			yield return new WaitForSeconds(1f);
//			Bet(0, JSevenCardStudBetType.kCheck);
//			yield return new WaitForSeconds(1f);
//			Bet(1, JSevenCardStudBetType.kCall);
//		}
//
//		// showdown
//		List<JCardHandData> showdownHands = new List<JCardHandData>(8);
//
//		// 앉은 자리 기준, 시계 방향으로
//		{
//			yield return new WaitForSeconds(1f);
//			Showdown(showdownHands[0]);
//
//			yield return new WaitForSeconds(1f);
//			Showdown(showdownHands[1]);
//		}
//
//		SetGameEnded();
	}

	void PutPlayerIntoRoom(int playerLocalId, string userName, long money)
	{
		players[playerLocalId] = new JCardPlayer();
		players[playerLocalId].SetBasicInfo(playerLocalId, userName, money);

		OnPlayerEnterRoom(players[playerLocalId]);
	}
	void BringUpPlayer(int playerLocalId)
	{
		OnPlayerExitRoom(players[playerLocalId]);

		players[playerLocalId] = null;
	}

	void DealInitial4Cards(jcasino.HandInfo handInfo)
	{
		Debug.Log("DealInitial4Cards - " + handInfo.ToString() + " \n");

		int[] cards = null;
		OnInitial4CardsDeal(null, cards);
	}
	void SetPlayerResultInitial4Cards(int playerLocalId, int openCard, int removeCard)
	{
		Debug.Log(string.Format("SetPlayerResultInitial4Cards playerLocalId({0}), openCard({1}), removeCard({2}) \n", 
		                        playerLocalId, openCard, removeCard));
	}
	
	void DealSecond2Cards(JCardHandData handData)
	{

	}

	void Bet(int playerLocalId, JSevenCardStudBetType bet)
	{
	}

	void DealThird1Cards(JCardHandData handData)
	{
	}

	void DealFourth1Cards(JCardHandData handData)
	{
	}

	void Showdown(JCardHandData handData)
	{
	}

	void SetGameEnded()
	{
	}
	#endregion
}
