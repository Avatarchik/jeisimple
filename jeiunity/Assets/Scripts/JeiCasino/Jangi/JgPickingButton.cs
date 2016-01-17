using UnityEngine;
using System.Collections;

public class JgPickingButton : MonoBehaviour
{
	public int row;
	public int col;


	public void OnClick_PButton()
	{
		//Debug.Log(string.Format("picked ({0}, {1}) \n", row, col));

		JgRefs.ins_.game.OnSelectPiece(row * 10 + col);
	}
}
