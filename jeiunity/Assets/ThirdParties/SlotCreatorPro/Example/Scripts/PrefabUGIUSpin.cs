using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PrefabUGIUSpin : MonoBehaviour {

	void Start () {
		//GameObject slotGO = (GameObject)Instantiate (Resources.Load ("5ReelSlot2D"));
		//slotGO.GetComponent<SlotSimpleGUI>().enabled = false;

		// Grab the spin button from your gui cache (read the comments at the top of GUIMgr.cs for explicit usage instructions)
		//Button spin = GUIMgr.ins.getButton("Spin");
		
		// Add the spin function of your slot to the OnClick listener for the button
		
	//	spin.onClick.AddListener(() => { Debug.Log ("Spinning"); slotGO.GetComponent<Slot>().spin (true); } );  
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
