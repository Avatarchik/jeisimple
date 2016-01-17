using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;


public class JSlotReel2 : MonoBehaviour
{
//	[SerializeField]
//	int reelIndex;
	
	JSlotGame slot;
	int reelLength; // included head and tail
	[SerializeField]
	List<GameObject> symbols;
	float cellHeight;
	
	#region Spin States
	int cellMoveCount;
	float cellTweenTime = 0.1f;
	//Ease firstMoveEase;
	Ease lastMoveEase;
	
	bool stopped;
	int cellsToMove;
	#endregion
	
	Transform myTransform;

	float speed = 1.1f;

	float yPosition;

	#region Test
	int symbolIndexer;
	bool manualSpinning;
	float spinTimer;
	void Update()
	{
		if (manualSpinning)
		{
			spinTimer += Time.deltaTime;

			float yTrans = Time.deltaTime * speed;
			yPosition += yTrans;
			myTransform.Translate(0f, -yTrans, 0f);

			if (yPosition >= cellHeight)
			{
				yPosition -= cellHeight;
				OnNextSymbol();
			}
		}
	}
	#endregion
	
	#region Initializaiton
	public void CreateReelSymbols(JSlotGame slot, int reelIndex, int reelHeight)
	{
		this.slot = slot;
		//this.reelIndex = reelIndex;
		this.reelLength = reelHeight + 2;
		this.cellHeight = slot.maker.cellHeight;
		
		this.cellMoveCount = slot.maker.cellsSpinCount[reelIndex];
		this.cellTweenTime = slot.maker.cellTweenTime;
		//this.firstMoveEase = slot.maker.firstMoveEase;
		this.lastMoveEase = slot.maker.lastMoveEase;

		this.speed = slot.maker.reel2Speed;

		symbols = new List<GameObject>(8);
		for (int i = 0; i < reelLength; ++i)
			//CreateSymbolInQueue(i);
		{
			int symbolIndex = GetSymbolIndex();
			GameObject newOne = CreateSymbol(i, symbolIndex);
			symbols.Add(newOne);
		}

		this.myTransform = this.transform;

		ResetReelPosition();
	}
	
	// slotIndex: Up to Down
	void CreateSymbolInQueue(int slotIndex)
	{
		int symbolIndex = GetSymbolIndex();
		GameObject newOne = CreateSymbol(slotIndex, symbolIndex);
		symbols.Insert(0, newOne);
	}
	
	GameObject CreateSymbol(int slotIndex, int symbolIndex)
	{
		GameObject newOne = null;
		
		if (slot.maker.usePool)
		{
		}
		else
		{
			newOne = (GameObject) GameObject.Instantiate(slot.maker.symbolPrefabs[symbolIndex]);
		}
		
		newOne.SetActive(true);
		newOne.transform.parent = this.transform;
		newOne.transform.localScale = Vector3.one;
		float yPos = CalcYPositionOfSymbol(slotIndex);
		newOne.transform.localPosition = new Vector3(0, yPos, 0);
		
		JSlotSymbol symbol = newOne.GetComponent<JSlotSymbol>();
		symbol.InitializeSymbol(symbolIndex, 
		                        slot.maker.symbolInfo[symbolIndex].symbolType,
		                        slot.maker.symbolInfo[symbolIndex].stackCount);
		
		symbol.index = ++symbolIndexer;
		symbol.slotIndex = slotIndex;

		if (symbol.IsStacked())
		{

		}

		return newOne;
	}
	
	int GetSymbolIndex()
	{
		return Random.Range(0, slot.maker.SymbolCount);
	}
	
	float CalcYPositionOfSymbol(int slotIndex)
	{
		float length = cellHeight * ((float)reelLength - 1f);
		return length * 0.5f - cellHeight * (float)slotIndex - transform.localPosition.y;
	}
	#endregion
	
	
	#region Spin
	public void SpinReel()
	{
		this.enabled = true;

		manualSpinning = true;
		spinTimer = 0;
		cellsToMove = cellMoveCount;
		
		Debug.Log(string.Format("<color=blue>SpinReel time: {0} </color>\n", cellTweenTime * (float)cellsToMove));
	}
	void SpinReelOneCell(Ease easeType)
	{
		transform.DOLocalMoveY(-cellHeight, cellTweenTime).SetRelative(true)
			.OnComplete(OnNextSymbol).SetEase(Ease.Linear);
	}
	void OnNextSymbol()
	{
		EnqueueSymbol();
		
		stopped = --cellsToMove == 0;

		if (stopped)
		{
			ResetReelPosition();

			this.enabled = false;
			manualSpinning = false;

			Debug.Log(string.Format("<color=blue>spinTimer: {0} </color>\n", spinTimer));
		}
		else if (cellsToMove == 1)
		{
			manualSpinning = false;

			ResetReelPosition();

			SpinReelOneCell(lastMoveEase);

			Debug.Log(string.Format("SpinReelOneCell lastMoveEase({0}) \n", lastMoveEase));
		}
	}
	void EnqueueSymbol()
	{
		int toRemove = reelLength - 1;
		GameObject.Destroy(symbols[toRemove]);
		symbols.RemoveAt(toRemove);
		
		CreateSymbolInQueue(0);//reelLength - 1);
	}

	void ResetReelPosition()
	{
		Vector3 pos = myTransform.localPosition;
		myTransform.localPosition = Vector3.zero;

		Vector3 v3 = Vector3.zero;
		for (int i = 0; i < symbols.Count; ++i)
		{
			v3 = symbols[i].transform.localPosition + pos;
			v3.y = CalcYPositionOfSymbol(i);
			symbols[i].transform.localPosition = v3;
		}
	}
	#endregion
}
