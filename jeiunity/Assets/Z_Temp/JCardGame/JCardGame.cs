using UnityEngine;
using System.Collections;


// View Controller

public class JCardGame : MonoBehaviour
{
	JCardPlayer[] players;


	#region Public Interfaces

	// Seven card stud
	void Ante()
	{
	}
	void DoThirdStreet()
	{
	}
	void BringIn()
	{
	}

	public void HandleAction(int playerId, J7CardStudPlayerAction action)
	{
	}
//	public void HandleBet(int playerId, JCardBetType betType)
//	{
//	}
	#endregion
}
