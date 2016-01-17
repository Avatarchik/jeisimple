using UnityEngine;
using System.Collections;
using Opencoding.CommandHandlerSystem;
using Opencoding.Console;


namespace Opencoding.Demo.CommandSecurity
{

	public class JgConsoleCommands : MonoBehaviour
	{
		void Awake()
		{
			CommandHandlers.RegisterCommandHandlers(this);
		}

		[CommandHandler(Description = "Server Data")]
		void svdata()
		{
			Debug.Log("printing svdata .. \n");
		}
	}
}
