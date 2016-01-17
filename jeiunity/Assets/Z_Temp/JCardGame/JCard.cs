using UnityEngine;
using System.Collections;

public class JCard : MonoBehaviour 
{
	Rank rank;
	Suit suit;
	bool isFaceUp;

	// The rank of suits, from lowest to hightest:
	public const int Club = 0;
	public const int Diamond = 1;
	public const int Heart = 2;
	public const int Spade = 3;

	public enum Rank { ACE = 1, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING };
	public enum Suit { CLUB, DIAMOND, HEART, SPADE };

//	//returns the value of a card, 1 - 11
//	public int GetValue()
//	{
//		int value = (int)this.rank;
//		return value > 10
//	}
}
