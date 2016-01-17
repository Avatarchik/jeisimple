using UnityEngine;
using System.Collections;

public class JSlotSymbol : MonoBehaviour
{
	[SerializeField]
	int symbolIndex;
//	[SerializeField]
//	JSymbolType symbolType;
	[SerializeField]
	int stackCount = 1;

	public int index;
	public int slotIndex;

	public bool IsStacked() 
	{
		return stackCount > 1;
	}
	public bool IsNullSymbol()
	{
		return index == 0;
	}

	public void InitializeSymbol(int symbolIndex, JSymbolType symbolType, int stackCount)
	{
		//this.symbolType = symbolType;
		this.stackCount = stackCount;
	}
}
