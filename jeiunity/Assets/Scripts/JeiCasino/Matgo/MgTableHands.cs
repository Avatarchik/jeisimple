using UnityEngine;
using System.Collections;




public enum MgTableRules
{
	kNagari,
}
[System.Serializable]
public class MgTableHands// : MonoBehaviour
{
	public MgPlayerHands[] players = new MgPlayerHands[2];

	bool CheckRules(MgTableRules item)
	{
		return false;
	}
}
