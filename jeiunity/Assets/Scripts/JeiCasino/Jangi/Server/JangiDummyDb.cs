using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DummyUserInfo
{
	public long gsn;
	public string email;
	public string nickName;
	public string password;

	public int gameLevel;
	public int victoryCount;
	public int defeatCount;
}

public class JangiDummyDb : MonoBehaviour
{
	public List<DummyUserInfo> users;

	[SerializeField]
	Dictionary<long, DummyUserInfo> usersByGsn;


	void Start()
	{
		InitData();
	}

	void InitData()
	{
		usersByGsn = new Dictionary<long, DummyUserInfo>();
		foreach (DummyUserInfo user in users)
		{
			usersByGsn.Add(user.gsn, user);
		}
	}

	public DummyUserInfo GetUserInfo(string email)
	{
		foreach (DummyUserInfo user in users)
		{
			if (user.email == email)
				return user;
		}
		return null;
	}
}
