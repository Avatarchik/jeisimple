using UnityEngine;
using System.Collections.Generic;

public class MatgoCommon
{
}


enum EMatgoCardCategory
{
	kUnknown	= 0,
	kSongHak 	= 1,
	kMaeJo 		= 2,
	kBeotkot 	= 3,
	kHeukSaRi 	= 4,
	kNanCho 	= 5,
	kMoRan 		= 6,
	kHongSaRi 	= 7,
	kGongSan 	= 8,
	kGukJun 	= 9,
	kDanPung 	= 10,
	kTong 		= 11,
	kBi 		= 12
}

public class MgUserInfo
{
	public string userName;
	public long gameMoney;
	public long currentGross;

	public void SetValues(string userName, long gameMoney, long currentGross)
	{
		this.userName = userName;
		this.gameMoney = gameMoney;
		this.currentGross = currentGross;
	}
}

//public class MgPlayerHands
//{
//	public List<int> gwangs; // 광
//	public List<int> animals; // 열끗
//	public List<int> bands; // 띠
//	public List<int> hulls; // 껍데기 
//}




