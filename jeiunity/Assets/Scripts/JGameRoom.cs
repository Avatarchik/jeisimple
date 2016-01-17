using UnityEngine;
using System.Collections;
using System;


// Game Room System
public class JGameRoom : MonoBehaviour 
{
	public event Action<JCardPlayer> OnPlayerEnterRoom;
	public event Action<JCardPlayer> OnPlayerExitRoom;
}
