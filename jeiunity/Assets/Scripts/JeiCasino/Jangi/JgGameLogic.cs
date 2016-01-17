using UnityEngine;
using System.Collections;
using jangi;



public class JgGameLogic : MonoBehaviour 
{

	JgNetClientMessageHandler msg;
	
	void OnEnable()
	{
		msg = JgRefs.ins_.msg;
		msg.onNtfSangcharim += OnNtfSangcharim;
		msg.onNtfChangeTurn += OnNtfChangeTurn;
		msg.onNtfMovePawn	+= OnNtfMovePawn;
	}
	void OnDisable()
	{
		msg.onNtfSangcharim -= OnNtfSangcharim;
		msg.onNtfChangeTurn -= OnNtfChangeTurn;
		msg.onNtfMovePawn	-= OnNtfMovePawn;
	}
	
	void OnNtfSangcharim(NtfSangcharim ntf)
	{
	}
	void OnNtfChangeTurn(NtfChangeTurn ntf)
	{
	}
	void OnNtfMovePawn(NtfMovePawn ntf)
	{
	}
}
