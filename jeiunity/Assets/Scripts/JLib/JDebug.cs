using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class JDebug : MonoBehaviour
{
	static List<string> logs = new List<string>();

	void Update()
	{
		if (logs.Count > 0)
		{
			foreach (string str in logs)
				Debug.Log(str);

			logs.Clear();
		}
	}

	public static void Log(string str)
	{
		logs.Add(str);
		//Debug.Log(str);
	}


}
