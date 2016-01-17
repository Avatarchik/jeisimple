using UnityEngine;
using System.Collections;
using System;



public class SevenCardMessageSerializer : MonoBehaviour
{
	public static SevenCardMessageSerializer ins_;

	#region Event Actions
	// JCardPlayer player, int[] cards
	public event Action<JCardPlayer, int[]> OnInitial4CardsDeal;

	// JCardPlayer player, int openedCard, int removedCard> OnInitial4CardsResult
	public event Action<JCardPlayer, int, int> OnInitial4CardsResult;

	// void OnCardDeal(int round, JCardPlayer[] players, int[] cardNumbers)
	public event Action<int, JCardPlayer[], int[]> OnCardDeal;

	// int round, JCardPlayer[] players, int[] cardNumbers
	public event Action<JCardPlayer, JSevenCardStudBetType> OnPlayerBet;

	// OnPlayersShowdown(JCardPlayer[] players, JSevenCardStudHand[] hands, string[] handDesc)
	public event Action<JCardPlayer[], JSevenCardStudHand[], string[]> OnPlayersShowdown;
	#endregion

	void OnEnable()
	{
	}
	void OnDisable()
	{
	}
	
}
