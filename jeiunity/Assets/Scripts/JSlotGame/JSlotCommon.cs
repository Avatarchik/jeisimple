using UnityEngine;
using System.Collections.Generic;

public class JSlotCommon {}

public enum JSymbolType
{
	kNormal,
	kWild,
	kBonus,
	kScatter
}

public enum JSlotState
{
	kReady,
	kSpinning,
	kSnapping,
	kPlayingWins,
	kUnskippablePlayingWinsDone,
	kPlayingWinsDone,
	kPlayingWinsCleared
}

[System.Serializable]
public class JSlotSymbolInfo
{
	public JSymbolType symbolType;
	public int stackCount = 1;

	List<GameObject> linkedSymbol;
	public bool active = true;
	public int link;
	public string linkName;
	public bool linked;
	public LinkPosition linkPosition;

	public int clampPerReel;
	public int clampTotal;
	public int symbolIndex;
	public bool perReelFrequency;
}



