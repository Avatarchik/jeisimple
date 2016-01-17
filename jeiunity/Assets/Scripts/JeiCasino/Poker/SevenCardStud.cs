using UnityEngine;
using System.Collections;
using UnityEngine.UI;



// View Controller
public class SevenCardStud : MonoBehaviour
{
	JGameRoom room;
	SevenCardMessageSerializer messageSerializer;
	public SevenCardStudRoomMockup mockupServer;

	int myId;

	public PlayerUIControls[] playerUIs;

	public AudioSource audioEntry;


	#region MonoBehaviour
	void OnEnable()
	{
		mockupServer.OnPlayerEnterRoom += OnPlayerEnterRoom;
		mockupServer.OnPlayerExitRoom += OnPlayerExitRoom;

		mockupServer.OnInitial4CardsDeal += OnInitial4CardsDeal;
//		messageSerializer.OnInitial4CardsResult += OnInitial4CardsResult;
//		messageSerializer.OnCardDeal += OnCardDeal;
//		messageSerializer.OnPlayerBet += OnPlayerBet;
//		messageSerializer.OnPlayersShowdown += OnPlayersShowdown;
	}
	void OnDisable()
	{
		mockupServer.OnPlayerEnterRoom -= OnPlayerEnterRoom;
		mockupServer.OnPlayerExitRoom -= OnPlayerExitRoom;

		mockupServer.OnInitial4CardsDeal -= OnInitial4CardsDeal;
	}
	void Start()
	{
		InitScene();
	}
	void Update ()
	{
		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();
	}
	#endregion

	int FindPlayerScreenPosition(int pos)
	{
		return pos;
	}

	#region Game Room Events
	void OnPlayerEnterRoom(JCardPlayer player)
	{
		int seatLocation = FindPlayerScreenPosition(player.LocalId);

		playerUIs[seatLocation].userNameText.text = player.UserName;
		playerUIs[seatLocation].moneyText.text = player.Money.ToString();

		audioEntry.Play();
	}
	void OnPlayerExitRoom(JCardPlayer player)
	{
		int seatLocation = FindPlayerScreenPosition(player.LocalId);
		
		playerUIs[seatLocation].userNameText.text = string.Empty;
		playerUIs[seatLocation].moneyText.text = string.Empty;
	}
	void OnGameStart()
	{
	}
	void OnGameEnd()
	{
	}
	#endregion

	#region 'Seven Card Stud' Game Logic Events
	void OnInitial4CardsDeal(JCardPlayer player, int[] cards)
	{
	}
	void OnInitial4CardsResult(JCardPlayer player, int openedCard, int removedCard)
	{
	}
	void OnCardDeal(int round, JCardPlayer[] players, int[] cardNumbers)
	{
	}
	void OnPlayerBet(JCardPlayer player, JSevenCardStudBetType bet)
	{
	}
	void OnPlayersShowdown(JCardPlayer[] players, JSevenCardStudHand[] hands, string[] handDesc)
	{
	}
	#endregion

	void InitScene()
	{
		AudioListener.volume = 0.05f;

		foreach (PlayerUIControls playerUI in playerUIs)
		{
			playerUI.userNameText.text = string.Empty;
			playerUI.moneyText.text = string.Empty;
		}
	}
}

[System.Serializable]
public class PlayerUIControls
{
	public Text userNameText;
	public Text moneyText;

}


