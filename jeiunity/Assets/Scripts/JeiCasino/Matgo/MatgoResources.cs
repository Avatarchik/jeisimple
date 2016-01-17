using UnityEngine;
using System.Collections;

public class MatgoResources : MonoBehaviour
{
	public Sprite[] bonusCards;
	public Sprite cardBack;
	public Sprite[] cards;



	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();
	}
}
