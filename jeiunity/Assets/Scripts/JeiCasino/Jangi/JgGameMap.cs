using UnityEngine;
using System.Collections;
using jangi;
using System;


public partial class JgGame : MonoBehaviour
{

	#region Construction Map
	void InitMap() 
	{
		map0[0] = Jg.ChoCha;
		map0[1] = Jg.ChoSang; //
		map0[2] = Jg.ChoMa; //
		map0[3] = Jg.ChoSa;
		map0[5] = Jg.ChoSa;
		map0[6] = Jg.ChoSang; //
		map0[7] = Jg.ChoMa; //
		map0[8] = Jg.ChoCha;
		map0[14] = Jg.ChoJang;
		map0[21] = Jg.ChoPo;
		map0[27] = Jg.ChoPo;
		map0[30] = Jg.ChoJol;
		map0[32] = Jg.ChoJol;
		map0[34] = Jg.ChoJol;
		map0[36] = Jg.ChoJol;
		map0[38] = Jg.ChoJol;
		
		map0[90] = Jg.HanCha;
		map0[91] = Jg.HanSang; //
		map0[92] = Jg.HanMa; //
		map0[93] = Jg.HanSa;
		map0[95] = Jg.HanSa;
		map0[96] = Jg.HanSang; //
		map0[97] = Jg.HanMa; //
		map0[98] = Jg.HanCha;
		map0[84] = Jg.HanJang;
		map0[71] = Jg.HanPo;
		map0[77] = Jg.HanPo;
		map0[60] = Jg.HanJol;
		map0[62] = Jg.HanJol;
		map0[64] = Jg.HanJol;
		map0[66] = Jg.HanJol;
		map0[68] = Jg.HanJol;
	}

	void SetSangcharimImpl(bool iAmCho, Sangcharim me, Sangcharim opponent)
	{
		Debug.Log(string.Format("SetSangcharim {0}/{1} \n", me, opponent));
		
		GameObject.Destroy(piecesInMap[1].gameObject);
		GameObject.Destroy(piecesInMap[2].gameObject);
		GameObject.Destroy(piecesInMap[6].gameObject);
		GameObject.Destroy(piecesInMap[7].gameObject);
		
		if (iAmCho)
		{
			//			if (me == Sangcharim.kSMSM && opponent == Sangcharim.kSMSM)
			//			else
			{
				switch (me)
				{
				case Sangcharim.kSMSM:
					piecesInMap[1] = CreatePawn(Jg.ChoSang, 1);
					piecesInMap[2] = CreatePawn(Jg.ChoMa, 2);
					piecesInMap[6] = CreatePawn(Jg.ChoSang, 6);
					piecesInMap[7] = CreatePawn(Jg.ChoMa, 7);
					break;
				case Sangcharim.kMSMS:
					piecesInMap[1] = CreatePawn(Jg.ChoMa, 1);
					piecesInMap[2] = CreatePawn(Jg.ChoSang, 2);
					piecesInMap[6] = CreatePawn(Jg.ChoMa, 6);
					piecesInMap[7] = CreatePawn(Jg.ChoSang, 7);
					break;
				case Sangcharim.kMSSM:
					piecesInMap[1] = CreatePawn(Jg.ChoMa, 1);
					piecesInMap[2] = CreatePawn(Jg.ChoSang, 2);
					piecesInMap[6] = CreatePawn(Jg.ChoSang, 6);
					piecesInMap[7] = CreatePawn(Jg.ChoMa, 7);
					break;
				case Sangcharim.kSMMS:
					piecesInMap[1] = CreatePawn(Jg.ChoSang, 1);
					piecesInMap[2] = CreatePawn(Jg.ChoMa, 2);
					piecesInMap[6] = CreatePawn(Jg.ChoMa, 6);
					piecesInMap[7] = CreatePawn(Jg.ChoSang, 7);
					break;
				}
				switch (opponent)
				{
				case Sangcharim.kSMSM:
					piecesInMap[91] = CreatePawn(Jg.HanSang, 91);
					piecesInMap[92] = CreatePawn(Jg.HanMa, 92);
					piecesInMap[96] = CreatePawn(Jg.HanSang, 96);
					piecesInMap[97] = CreatePawn(Jg.HanMa, 97);
					break;
				case Sangcharim.kMSMS:
					piecesInMap[91] = CreatePawn(Jg.HanMa, 91);
					piecesInMap[92] = CreatePawn(Jg.HanSang, 92);
					piecesInMap[96] = CreatePawn(Jg.HanMa, 96);
					piecesInMap[97] = CreatePawn(Jg.HanSang, 97);
					break;
				case Sangcharim.kMSSM:
					piecesInMap[91] = CreatePawn(Jg.HanMa, 91);
					piecesInMap[92] = CreatePawn(Jg.HanSang, 92);
					piecesInMap[96] = CreatePawn(Jg.HanSang, 96);
					piecesInMap[97] = CreatePawn(Jg.HanMa, 97);
					break;
				case Sangcharim.kSMMS:
					piecesInMap[91] = CreatePawn(Jg.HanSang, 91);
					piecesInMap[92] = CreatePawn(Jg.HanMa, 92);
					piecesInMap[96] = CreatePawn(Jg.HanMa, 96);
					piecesInMap[97] = CreatePawn(Jg.HanSang, 97);
					break;
				}
			}
		}
		else
		{
			if (me == Sangcharim.kSMSM && opponent == Sangcharim.kSMSM)
			{
			}
			else
			{
				switch (me)
				{
				case Sangcharim.kSMSM:
					piecesInMap[91] = CreatePawn(Jg.ChoSang+10, 91);
					piecesInMap[92] = CreatePawn(Jg.ChoMa+10, 92);
					piecesInMap[96] = CreatePawn(Jg.ChoSang+10, 96);
					piecesInMap[97] = CreatePawn(Jg.ChoMa+10, 97);
					break;
				case Sangcharim.kMSMS:
					piecesInMap[91] = CreatePawn(Jg.ChoMa+10, 91);
					piecesInMap[92] = CreatePawn(Jg.ChoSang+10, 92);
					piecesInMap[96] = CreatePawn(Jg.ChoMa+10, 96);
					piecesInMap[97] = CreatePawn(Jg.ChoSang+10, 97);
					break;
				case Sangcharim.kMSSM:
					piecesInMap[91] = CreatePawn(Jg.ChoMa+10, 91);
					piecesInMap[92] = CreatePawn(Jg.ChoSang+10, 92);
					piecesInMap[96] = CreatePawn(Jg.ChoSang+10, 96);
					piecesInMap[97] = CreatePawn(Jg.ChoMa+10, 97);
					break;
				case Sangcharim.kSMMS:
					piecesInMap[91] = CreatePawn(Jg.ChoSang+10, 91);
					piecesInMap[92] = CreatePawn(Jg.ChoMa+10, 92);
					piecesInMap[96] = CreatePawn(Jg.ChoMa+10, 96);
					piecesInMap[97] = CreatePawn(Jg.ChoSang+10, 97);
					break;
				}
				switch (opponent)
				{
				case Sangcharim.kSMSM:
					piecesInMap[91] = CreatePawn(Jg.HanSang-10, 91);
					piecesInMap[92] = CreatePawn(Jg.HanMa-10, 92);
					piecesInMap[96] = CreatePawn(Jg.HanSang, 96);
					piecesInMap[97] = CreatePawn(Jg.HanMa, 97);
					break;
				case Sangcharim.kMSMS:
					piecesInMap[91] = CreatePawn(Jg.HanMa, 91);
					piecesInMap[92] = CreatePawn(Jg.HanSang, 92);
					piecesInMap[96] = CreatePawn(Jg.HanMa, 96);
					piecesInMap[97] = CreatePawn(Jg.HanSang, 97);
					break;
				case Sangcharim.kMSSM:
					piecesInMap[91] = CreatePawn(Jg.HanMa, 91);
					piecesInMap[92] = CreatePawn(Jg.HanSang, 92);
					piecesInMap[96] = CreatePawn(Jg.HanSang, 96);
					piecesInMap[97] = CreatePawn(Jg.HanMa, 97);
					break;
				case Sangcharim.kSMMS:
					piecesInMap[91] = CreatePawn(Jg.HanSang, 91);
					piecesInMap[92] = CreatePawn(Jg.HanMa, 92);
					piecesInMap[96] = CreatePawn(Jg.HanMa, 96);
					piecesInMap[97] = CreatePawn(Jg.HanSang, 97);
					break;
				}
			}
		}
	}
	
	void CreatePieces(int[] map, bool isCho)
	{
		for (int i = 0; i < map.Length; ++i)
		{
			if (map[i] > 0)
			{
				int pawn = map[i];
				if (isCho == false)
				{
					pawn += pawn < 10 ? 10 : -10;
				}
				
				piecesInMap[i] = CreatePawn(pawn, i);
			}
		}
	}
	
	JgPiece CreatePawn(int pawn, int location)
	{
		map0[location] = pawn;
		
		GameObject newOne = (GameObject) GameObject.Instantiate(JgRefs.ins_.res.piecePrefabs[pawn]);
		
		newOne.transform.parent = pieceContainer;
		
		newOne.transform.localPosition = CalcPosition(location);
		JgPiece piece = newOne.GetComponent<JgPiece>();
		return piece;
	}
	
	Vector3 CalcPosition(int loc)
	{
		float px = cellWidth * (float)(loc % 10);
		float py = cellHeight * (float)(loc / 10);
		Vector3 pos = new Vector3(px, py, 0f);
		pos += offsetPosition;
		return pos;
	}
	#endregion

}
