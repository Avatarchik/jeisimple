using UnityEngine;
using System.Collections.Generic;



public enum MgRules
{
	kChongtong,

	kGoBak,
	kPiBak,
	kDokBak,
	kGwangBak,
	kMeongBak,
	kShake,

	kGodori,
	kHongdan,
	kCheongdan,
	kChodan,


}

[System.Serializable]
public class MgPlayerHands
{
	public List<int> gwangs; // 광
	public List<int> animals; // 열끗
	public List<int> bands; // 띠
	public List<int> hulls; // 껍데기 

	public int goCount;
	public int shakeCount;
	public int perkCount;

	public int scores;

	public bool Enabled(MgRules item)
	{
		return false;
	}
	public int CalcScores()
	{
		return scores;
	}
	public bool Achieved()
	{
		return scores >= 3;
	}
	public bool CanUseStop()
	{
		return scores >= 7;
	}
}
