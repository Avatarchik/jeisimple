using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;



public class JSlotMaker : MonoBehaviour
{
	public bool debugMode = true;
	public int numberOfReels = 1;
	public int reelHeight = 3;
	public float cellWidth = 1f;
	public float cellHeight = 1f;

	public int maxStackCount = 1;

	public Sprite[] symbols;

	public JSlotSymbolInfo[] symbolInfo;
	public GameObject[] symbolPrefabs;

	public bool usePool;
	
	// Runtime issues
	public float cellTweenTime = 0.2f;
	public int[] cellsSpinCount;
	public Ease firstMoveEase = Ease.OutElastic;
	public Ease lastMoveEase = Ease.OutElastic;


	public float reel2Speed;

	public string slotLobbySceneName;


	public int SymbolCount
	{
		get { return symbolPrefabs.Length; }
	}
}
