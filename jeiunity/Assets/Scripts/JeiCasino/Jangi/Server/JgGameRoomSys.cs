using UnityEngine;
using System.Collections.Generic;
using Thrift.Protocol;


public partial class JgGameRoom : MonoBehaviour
{
//	byte[] writeBuffer = new byte[1024 * 1];

	// zero based index
	[SerializeField]
	int roomIndex;

	public JangiServer server;

	[SerializeField]
	List<long> participants = new List<long>();

	[SerializeField]
	List<JStateObject> states = new List<JStateObject>();

	[SerializeField]
	List<long> spectators = new List<long>();

	#region Room Interfaces
	public int RoomIndex()
	{
		return roomIndex;
	}
	public void SetRoomIndex(int roomIndex)
	{
		this.roomIndex = roomIndex;
	}
	public bool IsEmpty()
	{
		return participants.Count == 0;
	}
	public bool IsWaitingOpponent()
	{
		return participants.Count == 1;
	}
	public int ParticipantCount()
	{
		 return participants.Count;
	}
//	public long ParticiantGsn(int index)
//	{
//		return participants.Count > index ? participants[index] : -1;
//	}
	public void AddParticipant(JStateObject state)// long gsn)
	{
		participants.Add(state.gsn);
		states.Add(state);
		state.indexInRoom = states.Count - 1;
	}
	public void RemoveParticipant(JStateObject state)// long gsn)
	{
		participants.Remove(state.gsn);
		states.Remove(state);
	}

	public long GetParticipantByIndex(int index)
	{
		return participants[index];
	}
	#endregion

	#region Transport
	void SendMessageToClient(JStateObject state, byte messageType, TBase message)
	{
		int length = JThrift.Serialize(messageType, message, ref state.writeBuffer);
		server.Send(state.workSocket, state.writeBuffer, length);

		Debug.Log(string.Format("<color=green>R: ->C ({0}) - {1} </color> \n", state.name, message));
	}

	void NotifyMessageToAllClients(byte messageType, TBase message)
	{
		//int length = JThrift.Serialize(messageType, message, ref writeBuffer);
		Debug.Log(string.Format("<color=green>R: ->All_C ({0}) - {1} </color> \n", messageType, message));

		foreach (JStateObject state in states)
		{
			//System.Buffer.BlockCopy(writeBuffer, 0, state.writeBuffer, 0, length);
			int length = JThrift.Serialize(messageType, message, ref state.writeBuffer);
			server.Send(state.workSocket, state.writeBuffer, length);
		}
	}
	#endregion
}
