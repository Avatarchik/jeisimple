using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


//[CustomEditor ( typeof(JSlot))]
[System.Serializable]
public class JSlotMakerEd : Editor
{

//	JSlot slot;

	void OnEnable()
	{
//		slot = (JSlot)target;
	}
	#region Main GUI
	public override void OnInspectorGUI()
	{
		EditorGUILayout.BeginVertical("Box");
		//GUI.color = Color.blue;
//		EditorGUILayout.LabelField("Reels:" + (slot.reelHeight - (slot.reelIndent * 2)).ToString() + "x" + slot.numberOfReels + " " +
//		                           "Symbols:" + slot.symbolPrefabs.Count + " " +
//		                           "Pays:" + slot.symbolSets.Count + " " +
//		                           "Lines:" + slot.lines.Count + " " +
//		                           "(" + (slot.edsave.returnPercent * 100) + "%)"
//		                           , EditorStyles.miniLabel);

//		EditorGUILayout.LabelField("Reels:" + slot.reelHeight);

		//GUI.color = Color.white;
		EditorGUILayout.EndVertical();
	}
	#endregion
}
