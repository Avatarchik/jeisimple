using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class JgRouteUtils
{

	// 1 steps
	// 1 straight 1 diagonal
	// 1 stragiht 2 diagonal
	// 1 jump
	// full straight 
	
	static int[] tempInts = new int[20];
	public static int[] CalcPath1Steps(int[] map, JgNation nation, int location, JgFilter filter)
	{
		//nation = FindNation(map[location]);
		
		List<int> locations = new List<int>();
		
		// 12 -> 12+1, 12+10, 12-1, 12-10
		tempInts[0] = location + 1;
		tempInts[1] = location + 10;
		tempInts[2] = location - 1;
		tempInts[3] = location - 10;
		//		int count = 0;
		for (int i = 0; i < 4; ++i)
			AddReachable(map, locations, nation, tempInts[i]);
		
		List<int> locations2 = new List<int>();
		
		switch (filter)
		{
		case JgFilter.kNoFilter:
			foreach (int loc in locations)
				locations2.Add(loc);
			break;
		case JgFilter.kOurSide:
			foreach (int loc in locations)
			{
				if (IsOurSide(nation, map[loc]) == false)
					locations2.Add(loc);
			}
			break;
		case JgFilter.kEnemySide:
			foreach (int loc in locations)
			{
				if (IsEnemy(nation, map[loc]) == false)
					locations2.Add(loc);
			}
			break;
		case JgFilter.kAllSide:
			foreach (int loc in locations)
			{
				if (map[loc] == 0)
					locations2.Add(loc);
			}
			break;
		}
		return locations2.ToArray();
	}
	
	public static int[] CalcPath1StraightXDiagonal_1(int[] map, int location)
	{
		JgNation nation = FindNation(map[location]);
		
		int[] firstPaths = CalcPath1Steps(map, nation, location, JgFilter.kAllSide);
		
		List<int> locations = new List<int>();

		for (int i = 0; i < firstPaths.Length; ++i)
		{
			if (firstPaths[i] >= 0)
			{
				bool upDir = (firstPaths[i] - location) == 10;
				bool downDir = (firstPaths[i] - location) == -10;
				bool leftDir = (firstPaths[i] - location) == -1;
				bool rightDir = (firstPaths[i] - location) == 1;
				
				if (upDir)
				{
					// 43 -> 53 ( 53+10-1, 53+10+1 )
					AddReachable(map, locations, nation, firstPaths[i] + 10 * 1 - 1);
					AddReachable(map, locations, nation, firstPaths[i] + 10 * 1 + 1);
				}
				else if (downDir)
				{
					// 43 -> 33 ( 33-10-1, 33-10+1 )
					AddReachable(map, locations, nation, firstPaths[i] - 10 * 1 - 1);
					AddReachable(map, locations, nation, firstPaths[i] - 10 * 1 + 1);
				}
				else if (leftDir)
				{
					// 43 -> 42 ( 42-10-1, 41+10-1 )
					AddReachable(map, locations, nation, firstPaths[i] - 10 * 1 - 1);
					AddReachable(map, locations, nation, firstPaths[i] + 10 * 1 - 1);
				}
				else if (rightDir)
				{
					// 43 -> 44 ( 44-10+1, 44+10+1 )
					AddReachable(map, locations, nation, firstPaths[i] - 10 * 1 + 1);
					AddReachable(map, locations, nation, firstPaths[i] + 10 * 1 + 1);
				}
			}
		}
		return locations.ToArray();
	}

	public static int[] CalcPath1StraightXDiagonal_2(int[] map, int location)
	{
		JgNation nation = FindNation(map[location]);
		
		int[] firstPaths = CalcPath1Steps(map, nation, location, JgFilter.kAllSide);
		
		List<int> locations = new List<int>();
		
		for (int i = 0; i < firstPaths.Length; ++i)
		{
			if (firstPaths[i] >= 0)
			{
				bool upDir = (firstPaths[i] - location) == 10;
				bool downDir = (firstPaths[i] - location) == -10;
				bool leftDir = (firstPaths[i] - location) == -1;
				bool rightDir = (firstPaths[i] - location) == 1;
				
				if (upDir)
				{
					//AddReachable(map, locations, nation, firstPaths[i] + 10 * 1 - 1);
					//AddReachable(map, locations, nation, firstPaths[i] + 10 * 1 + 1);
					//IsReachable(map, nation,
					if (CanPassThrough(map, firstPaths[i] + 10 * 1 - 1))
						AddReachable(map, locations, nation, firstPaths[i] + 10 * 2 - 2);
					if (CanPassThrough(map, firstPaths[i] + 10 * 1 + 1))
						AddReachable(map, locations, nation, firstPaths[i] + 10 * 2 + 2);
				}
				else if (downDir)
				{
					// 43 -> 33 ( 33-10-1, 33-10+1 )
					//AddReachable(map, locations, nation, firstPaths[i] - 10 * 1 - 1);
					//AddReachable(map, locations, nation, firstPaths[i] - 10 * 1 + 1);
					if (CanPassThrough(map, firstPaths[i] - 10 * 1 - 1))
						AddReachable(map, locations, nation, firstPaths[i] - 10 * 2 - 2);
					if (CanPassThrough(map, firstPaths[i] - 10 * 1 + 1))
						AddReachable(map, locations, nation, firstPaths[i] - 10 * 2 + 2);
				}
				else if (leftDir)
				{
					// 43 -> 42 ( 42-10-1, 41+10-1 )
					//AddReachable(map, locations, nation, firstPaths[i] - 10 * 1 - 1);
					//AddReachable(map, locations, nation, firstPaths[i] + 10 * 1 - 1);
					if (CanPassThrough(map, firstPaths[i] - 10 * 1 - 1))
						AddReachable(map, locations, nation, firstPaths[i] - 10 * 2 - 2);
					if (CanPassThrough(map, firstPaths[i] + 10 * 1 - 1))
						AddReachable(map, locations, nation, firstPaths[i] + 10 * 2 - 2);
				}
				else if (rightDir)
				{
					// 43 -> 44 ( 44-10+1, 44+10+1 )
					//AddReachable(map, locations, nation, firstPaths[i] - 10 * 1 + 1);
					//AddReachable(map, locations, nation, firstPaths[i] + 10 * 1 + 1);
					if (CanPassThrough(map, firstPaths[i] - 10 * 1 + 1))
						AddReachable(map, locations, nation, firstPaths[i] - 10 * 2 + 2);
					if (CanPassThrough(map, firstPaths[i] + 10 * 1 + 1))
						AddReachable(map, locations, nation, firstPaths[i] + 10 * 2 + 2);
				}
			}
		}
		return locations.ToArray();
	}
	public static int[] CalcPathFullStraight(int[] map, int location)
	{
		JgNation nation = FindNation(map[location]);
		
		List<int> locations = new List<int>(16);
		
		// left
		int horizontalNumber = location % 10;
		for (int i = 1; i <= horizontalNumber; ++i)
		{
			int loc = location - i;
			if (IsReachable(map, nation, loc))
			{
				locations.Add(loc);
				if (IsEnemy(nation, map[loc]))
					break;
			}
			else break;
		}
		// right
		int count = 1;
		for (int i = horizontalNumber + 1; i < 9; ++i, ++count)
		{
			int loc = location + count;
			if (IsReachable(map, nation, loc))
			{
				locations.Add(loc);
				if (IsEnemy(nation, map[loc]))
					break;
			}
			else break;
		}

		// up
		for (int i = 1; i < 10; ++i)
		{
			int loc = location + i * 10;
			if (loc < 100 && IsReachable(map, nation, loc))
			{
				locations.Add(loc);
				if (IsEnemy(nation, map[loc]))
					break;
			}
			else
				break;
		}
		
		// down
		for (int i = 1; i < 10; ++i)
		{
			int loc = location - i * 10;
			if (loc >= 0 && IsReachable(map, nation, loc))
			{
				locations.Add(loc);
				if (IsEnemy(nation, map[loc]))
					break;
			}
			else
				break;
		}
		
		return locations.ToArray();
	}
	public static int[] CalcPathJumpableStraight(int[] map, int location)
	{
		JgNation nation = FindNation(map[location]);
		
		List<int> locations = new List<int>(16);
		
		// left
		int hIndex = location % 10;
		int row = location / 10;
		for (int i = hIndex - 1; i >= 0; --i)
		{
			int num = row * 10 + i;
			
			if ((map[num] % 10) == Jg.ChoPo)
				break;
			else if (map[num] > 0)
			{
				for (int j = i-1; j >= 0; --j)
				{
					int foundLoc = row * 10 + j;
					
					AddReachablePo(map, locations, nation, foundLoc);
					
					if (map[foundLoc] > 0)
						break;
				}
				break;
			}
		}
		// right
		for (int i = hIndex + 1; i < 9; ++i)
		{
			int num = row * 10 + i;
			
			if ((map[num] % 10) == Jg.ChoPo)
				break;
			else if (map[num] > 0)
			{
				for (int j = i+1; j < 9; ++j)
				{
					int foundLoc = row * 10 + j;
					
					AddReachablePo(map, locations, nation, foundLoc);
					
					if (map[foundLoc] > 0)
						break;
				}
				break;
			}
		}
		
		// up
		for (int i = 1; i < 10; ++i)
		{
			int loc = location + i * 10;
			
			if (loc > 99)
				break;
			
			if ((map[loc] % 10) == Jg.ChoPo)
				break;
			else if (map[loc] > 0)
			{
				for (int j = loc + 10; j < 100; j += 10)
				{
					AddReachablePo(map, locations, nation, j);
					if (map[j] > 0)
						break;
				}
				break;
			}
		}
		// down
		for (int i = 1; i < 10; ++i)
		{
			int loc = location - i * 10;
			
			if (loc < 0)
				break;
			
			if ((map[loc] % 10) == Jg.ChoPo)
				break;
			else if (map[loc] > 0)
			{
				for (int j = loc - 10; j >= 0; j -= 10)
				{
					AddReachablePo(map, locations, nation, j);
					if (map[j] > 0)
						break;
				}
				break;
			}
		}
		
		return locations.ToArray();
	}
	public static int[] CalcPathJumpableInCastle(int[] map, int location)
	{
		JgNation nation = FindNation(map[location]);

		List<int> locations = new List<int>();
		switch (location)
		{
		case 3:
			AddReachablePoInCastle(map, nation, 14, 25, locations); break;
		case 5:
			AddReachablePoInCastle(map, nation, 14, 23, locations); break;
		case 23:
			AddReachablePoInCastle(map, nation, 14,  5, locations); break;
		case 25:
			AddReachablePoInCastle(map, nation, 14,  3, locations); break;
		}
		return locations.ToArray();
	}
	public static int[] CalcPath1StepsNoBack(int[] map, int location)
	{
		JgNation nation = FindNation(map[location]);
		
		List<int> locations = new List<int>();
		
		// left, right
		AddReachable(map, locations, nation, location - 1);
		AddReachable(map, locations, nation, location + 1);
		
		// forward
		if (nation == JgNation.kCho)
			AddReachable(map, locations, nation, location + 10);
		else
			AddReachable(map, locations, nation, location - 10);
		
		// in the castle
		switch (location)
		{
		case 14:
			AddReachable(map, locations, nation, 3);
			AddReachable(map, locations, nation, 5);
			break;
		case 23:
		case 25:
			AddReachable(map, locations, nation, 14);
			break;
		case 73:
		case 75:
			AddReachable(map, locations, nation, 94);
			break;
		case 84:
			AddReachable(map, locations, nation, 93);
			AddReachable(map, locations, nation, 95);
			break;
		}
		
		return locations.ToArray();
	}
	
	public static int[] CalcPath1StepsInCastle(int[] map, int location)
	{
		JgNation nation = FindNation(map[location]);
		
		List<int> locations = new List<int>();
		
		bool lowerSide = location < 30;
		if (lowerSide)
		{
			switch (location)
			{
			case 3:
				AddReachable(map, locations, nation, 4);
				AddReachable(map, locations, nation, 13);
				AddReachable(map, locations, nation, 14);
				break;
			case 4:
				AddReachable(map, locations, nation, 3);
				AddReachable(map, locations, nation, 5);
				AddReachable(map, locations, nation, 14);
				break;
			case 5:
				AddReachable(map, locations, nation, 4);
				AddReachable(map, locations, nation, 14);
				AddReachable(map, locations, nation, 15);
				break;
			case 13:
				AddReachable(map, locations, nation, 3);
				AddReachable(map, locations, nation, 14);
				AddReachable(map, locations, nation, 23);
				break;
			case 14:
				AddReachable(map, locations, nation, 3);
				AddReachable(map, locations, nation, 4);
				AddReachable(map, locations, nation, 5);
				AddReachable(map, locations, nation, 13);
				AddReachable(map, locations, nation, 14);
				AddReachable(map, locations, nation, 15);
				AddReachable(map, locations, nation, 23);
				AddReachable(map, locations, nation, 24);
				AddReachable(map, locations, nation, 25);
				break;
			case 15:
				AddReachable(map, locations, nation, 5);
				AddReachable(map, locations, nation, 14);
				AddReachable(map, locations, nation, 25);
				break;
			case 23:
				AddReachable(map, locations, nation, 13);
				AddReachable(map, locations, nation, 14);
				AddReachable(map, locations, nation, 24);
				break;
			case 24:
				AddReachable(map, locations, nation, 23);
				AddReachable(map, locations, nation, 25);
				AddReachable(map, locations, nation, 14);
				break;
			case 25:
				AddReachable(map, locations, nation, 24);
				AddReachable(map, locations, nation, 14);
				AddReachable(map, locations, nation, 15);
				break;
			}
		}
		else
		{
			switch (location)
			{
			case 73:
				AddReachable(map, locations, nation, 74);
				AddReachable(map, locations, nation, 83);
				AddReachable(map, locations, nation, 84);
				break;
			case 74:
				AddReachable(map, locations, nation, 73);
				AddReachable(map, locations, nation, 75);
				AddReachable(map, locations, nation, 84);
				break;
			case 75:
				AddReachable(map, locations, nation, 74);
				AddReachable(map, locations, nation, 84);
				AddReachable(map, locations, nation, 85);
				break;
			case 83:
				AddReachable(map, locations, nation, 73);
				AddReachable(map, locations, nation, 84);
				AddReachable(map, locations, nation, 93);
				break;
			case 84:
				AddReachable(map, locations, nation, 73);
				AddReachable(map, locations, nation, 74);
				AddReachable(map, locations, nation, 75);
				AddReachable(map, locations, nation, 83);
				AddReachable(map, locations, nation, 84);
				AddReachable(map, locations, nation, 85);
				AddReachable(map, locations, nation, 93);
				AddReachable(map, locations, nation, 94);
				AddReachable(map, locations, nation, 95);
				break;
			case 85:
				AddReachable(map, locations, nation, 75);
				AddReachable(map, locations, nation, 84);
				AddReachable(map, locations, nation, 95);
				break;
			case 93:
				AddReachable(map, locations, nation, 83);
				AddReachable(map, locations, nation, 84);
				AddReachable(map, locations, nation, 94);
				break;
			case 94:
				AddReachable(map, locations, nation, 93);
				AddReachable(map, locations, nation, 95);
				AddReachable(map, locations, nation, 84);
				break;
			case 95:
				AddReachable(map, locations, nation, 94);
				AddReachable(map, locations, nation, 84);
				AddReachable(map, locations, nation, 85);
				break;
			}
		}
		
		return locations.ToArray();
	}

	public static int[] FindPossibleRoute(int[] map0, int loc)
	{
		int piece = map0[loc];
		int pieceIndex = piece % 10;
		
		int[] paths = null;
		
		switch (pieceIndex)
		{
		case Jg.ChoJang:
			paths = JgRouteUtils.CalcPath1StepsInCastle(map0, loc);
			break;
		case Jg.ChoCha:
			paths = JgRouteUtils.CalcPathFullStraight(map0, loc);
			break;
		case Jg.ChoPo:
			int[] paths0 = JgRouteUtils.CalcPathJumpableStraight(map0, loc);
			int[] paths1 = JgRouteUtils.CalcPathJumpableInCastle(map0, loc);
			paths = paths0.Union(paths1).ToArray();
			break;
		case Jg.ChoMa:
			paths = JgRouteUtils.CalcPath1StraightXDiagonal_1(map0, loc);
			break;
		case Jg.ChoSang:
			paths = JgRouteUtils.CalcPath1StraightXDiagonal_2(map0, loc);
			break;
		case Jg.ChoSa:
			paths = JgRouteUtils.CalcPath1StepsInCastle(map0, loc);
			break;
		case Jg.ChoJol:
			paths = JgRouteUtils.CalcPath1StepsNoBack(map0, loc);
			break;
		}
		return paths;
	}
	public static bool CheckCheckmate(int[] map, int loc)
	{
		int[] possibleRoutes = FindPossibleRoute(map, loc);
		foreach(int point in possibleRoutes)
		{
			if (map[point] % 10 == Jg.JANG)
				return true;
		}
		return false;
	}

	#region Utility Funcs
	public static JgNation FindNation(int piece)
	{
		if (piece <= 0)
			return JgNation.kUnknown;
		
		return piece > 10 ? JgNation.kHan : JgNation.kCho;
	}
	public static bool IsEnemy(JgNation nation, int piece)
	{
		if (nation == JgNation.kCho && piece >= Jg.HanJang)
			return true;
		else if (nation == JgNation.kHan && piece > 0 && piece < 10)
			return true;
		return false;
		
	}
	public static bool IsOurSide(JgNation nation, int piece)
	{
		if (nation == JgNation.kCho && piece < 10 && piece > 0)
			return true;
		else if (nation == JgNation.kHan && piece > 10)
			return true;
		return false;
	}
	
	public static bool IsValidLocation(int location)
	{
		return location >= 0 && location < 99 && (location % 10 < 9);
	}
	public static bool IsReachable(int[] map, JgNation nation, int location)
	{
		return IsValidLocation(location) && IsOurSide(nation, map[location]) == false;
	}
	public static bool CanPassThrough(int[] map, int location)
	{
		return IsValidLocation(location) && map[location] == Jg.EmptyCell;
	}
	static void AddReachable(int[] map, List<int> locations, JgNation nation, int target)
	{
		if (IsReachable(map, nation, target))
			locations.Add(target);
	}
	static void AddReachablePo(int[] map, List<int> locations, JgNation nation, int target)
	{
		if (IsReachable(map, nation, target) && (map[target] % 10) != Jg.ChoPo)
			locations.Add(target);
	}
	static void AddReachablePoInCastle(int[] map, JgNation nation, int occulusion, int target, List<int> locations)
	{
		bool canPass = map[occulusion] % 10 != Jg.PO && map[occulusion] != 0;
		if (canPass && IsOurSide(nation, map[target]) == false)
			locations.Add(target);
	}
	#endregion
	
	#region Test
	public static void TestPaths(int[] map)
	{
		int[] paths;
		
		paths = CalcPath1Steps(map, JgNation.kCho, 1, JgFilter.kNoFilter);
		PrintLog("CalcPath1Steps NoFilter " , 1, paths);
		
		paths = CalcPath1Steps(map, JgNation.kCho, 1, JgFilter.kOurSide);
		PrintLog("CalcPath1Steps Filter(OurSide) " , 1, paths);
		
		paths = CalcPath1Steps(map, JgNation.kCho, 1, JgFilter.kEnemySide);
		PrintLog("CalcPath1Steps Filter(Enemy) " , 1, paths);
		
		paths = CalcPath1Steps(map, JgNation.kCho, 1, JgFilter.kAllSide);
		PrintLog("CalcPath1Steps Filter(AllSide) " , 1, paths);
	}
	
	static void PrintLog(string desc, int location, int[] paths)
	{
		StringBuilder sb = new StringBuilder(32);
		sb.Append(desc);
		sb.Append(string.Format("Location({0}) : ( ", location));
		for (int i = 0; i < paths.Length; ++i)
		{
			if (paths[i] >= 0)
			{
				sb.Append(paths[i].ToString());
				
				if (i < paths.Length - 1)
					sb.Append(", ");
			}
		}
		
		sb.Append(" ) \n");
		Debug.Log(sb.ToString());
	}
	#endregion


	#region Sangcharim

	#endregion
}
