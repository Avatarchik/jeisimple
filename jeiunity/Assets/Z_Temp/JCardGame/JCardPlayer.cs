using UnityEngine;
using System.Collections;


public class JCardPlayer
{
	[SerializeField]
	private int localId; // local player id
	public string userName;
	public long money;

	
	public int LocalId { get { return this.localId; } }
	public long Money { get { return this.money; } }
	public string UserName { get { return this.userName; } }

	public void SetBasicInfo(int playerId, string userName, long money)
	{
		this.localId = playerId;
		this.userName = userName;
		this.money = money;
	}
}
