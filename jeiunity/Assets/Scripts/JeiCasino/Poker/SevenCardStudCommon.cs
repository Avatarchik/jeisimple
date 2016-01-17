using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SevenCardStudCommon{}


public enum J7CardStudPlayerAction
{
	kIntialDropAndOpen,
	kInitialOpen,
	kBet,
}

public enum JSevenCardStudBetType
{
	kUnknown,
	kDie,
	kPping,
	kTadang,
	kCall,
	kCheck,
	kQuater,
	kHalf
}

public enum JSevenCardStudHand
{
	kTop,
	kOnePair,
	kTwoPair,
	kThreeCard,
	kStraight,
	kMountain,
	kFullHouse
}


public class JCardHand
{
	int playerLocalId;
	int[] cards;
}

public struct JCardHandData
{
	// playerLocalId, card-0, card-1, card-2, card-3, ...
	int [] handData;
}
