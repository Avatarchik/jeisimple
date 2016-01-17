using UnityEngine;
using System.Collections;

public class JCardCommon {}


public enum JCardGameType
{
	kUnknown,
	
	kBlackjack,
	kHoldem,
	kThreeCard,
	
	kVideoPoker,
	
	k7CardStud,
	kHiLoPoker,
	kBadugi,
	
	kBaccarat
}


#region Blackjack
public enum JBlackjackPlayerAction
{
}

#endregion


#region Texas Holdem
public enum JHoldemPlayerAction
{
	kDealer_DealThreeCards,
	kPlayer_Bet,
}
#endregion





