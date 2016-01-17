using UnityEngine;
using System.Collections.Generic;
using jangi;
using System.Collections;
using System;
using System.Linq;
using DG.Tweening;


public partial class JgGame : MonoBehaviour
{
	public float cellWidth;
	public float cellHeight;
	public Vector3 offsetPosition;

	public Transform pieceContainer;

	public GameObject[] routes;

	public GameObject marked;
	public GameObject putOn;

	public event Action<int, int> OnPutPieceByInput;

	public bool appPlayerIsCho;

	public SpriteRenderer jangunSprite;


	[SerializeField]
	int currentPlayerId = -1;
	[SerializeField]
	int appPlayerId = -1;

	[SerializeField]
	int selectedPiece = -1;


	//int[] map;
	[SerializeField]
	JgPiece[] piecesInMap = new JgPiece[100];

	List<int> currentRoutes = new List<int>(20);

	//long appOwnerId = 1001;
	JgVCInGame ui;

	[SerializeField]
	int[] map0 = new int[100];

	JgNetClientMessageHandler msg;

//	void Start()
//	{
//		jangunSprite.gameObject.SetActive(true);
//		//jangunSprite.transform.localPosition = CalcPosition(target) + new Vector3(0, 0.8f, 0);
//		jangunSprite.transform.DOJump(new Vector3(0, 0f, 0), 0.5f, 100, 200f);
//	}
	void OnEnable()
	{
		msg = JgRefs.ins_.msg;
		//msg.onNtfSangcharim += OnNtfSangcharim;
		msg.onNtfChangeTurn += OnNtfChangeTurn;
		msg.onNtfMovePawn	+= OnNtfMovePawn;
	}
	void OnDisable()
	{
		//msg.onNtfSangcharim -= OnNtfSangcharim;
		msg.onNtfChangeTurn -= OnNtfChangeTurn;
		msg.onNtfMovePawn	-= OnNtfMovePawn;
	}
	void OnNtfChangeTurn(NtfChangeTurn ntf)
	{
		currentPlayerId = ntf.LocalId;
	}
	void OnNtfMovePawn(NtfMovePawn ntf)
	{
		currentPlayerId = Jg.OpponentId(ntf.LocalId);
	}

	public void InitGame(bool iAmCho, int appPlayerId) //, Sangcharim me, Sangcharim opponent)
	{
		Debug.Log("--------------------------------\n");
		Debug.Log(string.Format("AppGame's initializing a game .. iAmCho({0}) \n", iAmCho));

		this.appPlayerId = appPlayerId;
		this.appPlayerIsCho = iAmCho;

		gameObject.SetActive(true);
		JgRefs.ins_.vcInGame.gameObject.SetActive(true);

		InitMap();

		JgRouteUtils.TestPaths(map0);

		CreatePieces(map0, iAmCho);

		//SetSangcharim(iAmCho, me, opponent);
	}

	bool IsOpponentForce(int loc1, int loc2)
	{
		if (loc1 == -1 || loc2 == -1)
			return false;
		if (map0[loc1] < 10 && map0[loc2] > 10)
			return true;
		else if (map0[loc1] > 10 && map0[loc2] < 10)
			return true;
		return false;
	}
	public void OnSelectPiece(int loc)
	{
		Debug.Log(string.Format ("OnSelect loc({0}), piece({1}) \n", loc, map0[loc]));
		
		foreach (GameObject go in routes)
			go.SetActive(false);
		
		int piece = map0[loc];

		JgNation selectedNation = JgRouteUtils.FindNation(map0[loc]);
		JgNation currentTurnNation = (JgNation) (currentPlayerId + 1);

		bool pickable = piece > 0 && selectedNation == currentTurnNation;
		if (pickable)
		{
			bool selectedNewly = selectedPiece != loc;
			bool attackable = selectedNewly && IsOpponentForce(selectedPiece, loc);

			if (attackable)
			{
				bool isAppPlayer = this.appPlayerId == this.currentPlayerId;
				if (isAppPlayer)
					PutPieceByInput(selectedPiece, loc);
				else
					JgRefs.ins_.virtualPlayer.PutPieceByInput(selectedPiece, loc);
			}
			else if (selectedNewly)
			{
				selectedPiece = loc;
				
				ShowPossibleRoutes(loc);
				
				SetSelectedPiece(loc);
			}
		}
		else
		{
			if (CanMove(loc))
			{
				bool isAppPlayer = this.appPlayerId == this.currentPlayerId;
				if (isAppPlayer)
					PutPieceByInput(selectedPiece, loc);
				else
					JgRefs.ins_.virtualPlayer.PutPieceByInput(selectedPiece, loc);
			}
			else
			{
				marked.SetActive(false);
			}
			
			currentRoutes.Clear();
		}
	}

	public void SetSangcharim(bool iAmCho, Sangcharim me, Sangcharim opponent)
	{
		SetSangcharimImpl(iAmCho, me, opponent);
	}




	bool CanMove(int target)
	{
		foreach (int loc in currentRoutes)
		{
			if (loc == target)
				return true;
		}
		return false;
	}

	void ShowPossibleRoutes(int loc)
	{
		int[] paths = JgRouteUtils.FindPossibleRoute(map0, loc);

		currentRoutes.Clear();

		if (paths != null)
		{
			for (int i = 0; i < paths.Length; ++i)
			{
				routes[i].SetActive(true);
				routes[i].transform.localPosition = CalcPosition(paths[i]);

				currentRoutes.Add(paths[i]);
			}
		}
	}

	void SetSelectedPiece(int loc)
	{
		marked.SetActive(true);

		marked.transform.localPosition = CalcPosition(loc);
	}
	public void PutPieceByInput(int selected, int target)
	{
		PutPiece(selected, target);

		ReqMovePawn req = new ReqMovePawn();
		req.Location = selected;
		req.Target = target;
		JgRefs.ins_.msg.SendMessageToServer(JgC2S_Msgs.kMovePawn, req); //refactor:
	}

	public void PutPiece(int selected, int target)
	{
		Debug.Log(string.Format("PutInPiece ({0}) -> ({1}) \n", selected, target));

		JgPiece enemy = piecesInMap[target];
		bool enemyAlreadyExits = enemy != null;

		if (enemyAlreadyExits)
		{
			enemy.gameObject.SetActive(false);

			piecesInMap[target] = null;
		}

		map0[target] = map0[selected];
		map0[selected] = 0;

		piecesInMap[target] = piecesInMap[selected];
		piecesInMap[selected] = null;

		piecesInMap[target].transform.localPosition = CalcPosition(target);

		marked.SetActive(false);

		selectedPiece = -1;
		//JgRefs.ins_.server.NotifyPieceMoved(selected, target);

		if (jangunSprite.gameObject.activeInHierarchy)
		{
			jangunSprite.transform.DOKill();
			jangunSprite.gameObject.SetActive(false);
		}
		else if (JgRouteUtils.CheckCheckmate(map0, target))
		{
			Debug.Log("<color=orange> Checkmate !!!!</color>\n");

			jangunSprite.gameObject.SetActive(true);
			jangunSprite.transform.localPosition = CalcPosition(target) + new Vector3(0, 0.8f, 0);
			jangunSprite.transform.DOJump(new Vector3(0, 0f, 0), jangunSprite.transform.localPosition.y + 0.5f, 100, 200f).SetRelative(true);
			//jangunSprite.transform.DOKill();
		}
	}

}
