using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Text;



public class JgCommon
{
}

public enum JgNation
{
	kUnknown,
	kCho,
	kHan
}
public enum JgPieceType
{
	kUnknown,
	kJang,
	kCha,
	kPo,
	kMa,
	kSang,
	kSa,
	kJol
}

public struct JgLocation
{
	public int row;
	public int col;
}

public enum JgFilter
{
	kNoFilter,
	kOurSide,
	kEnemySide,
	kAllSide,
}

public class Jg
{
	public const int JANG 		= 1;
	public const int CHA  		= 2;
	public const int PO 		= 3;
	public const int MA			= 4;
	public const int SANG		= 5;
	public const int SA			= 6;
	public const int JOL		= 7;
	
	public const int EmptyCell  = 0;
	public const int ChoJang	= 1;
	public const int ChoCha 	= 2;
	public const int ChoPo 		= 3;
	public const int ChoMa 		= 4;
	public const int ChoSang 	= 5;
	public const int ChoSa 		= 6;
	public const int ChoJol 	= 7;

	public const int HanJang 	= 11;
	public const int HanCha 	= 12;
	public const int HanPo 		= 13;
	public const int HanMa 		= 14;
	public const int HanSang 	= 15;
	public const int HanSa 		= 16;
	public const int HanJol 	= 17;

	public const int Thrift_Offset = 4;

	//public static int 
	public static int OpponentId(int localId)
	{
		return localId == 0 ? 1 : 0;
	}

}

// State object for receiving data from remote device.
[System.Serializable]
public class JStateObject
{
	public long gsn;
	public int roomIndex = -1;
	public int indexInRoom = -1;
	public string name;

	// Client socket.
	public Socket workSocket = null;
	// Size of receive buffer.
	public const int BufferSize = 256;
	// Receive buffer.
	public byte[] readBuffer = new byte[BufferSize];
	public byte[] writeBuffer = new byte[BufferSize];

	public int bytesRead = 0;
	
	// Received data string.
	public StringBuilder sb = new StringBuilder();
	
	public int lengthInHeader;
	public int messageType0;
	public int messageType1;
	
	public JStateObject()
	{}
	public JStateObject(Socket sock)
	{
		workSocket = sock;
	}
	
	public int SocketHandle {
		get { return workSocket != null ? workSocket.Handle.ToInt32() : 0; }
	}
	public void ProcessHeader()
	{
		lengthInHeader = System.BitConverter.ToInt16(readBuffer, 0);
		messageType0 = readBuffer[2];
		messageType1 = readBuffer[3];
	}
	
	public void Reset()
	{
		gsn = -1;
		roomIndex = -1;
		bytesRead = 0;
	}
}


public class JUserInfo
{
	public JStateObject state;
	public long gsn;
	public string userName;
	public string nickName;
}

public class JgUserInfo : JUserInfo
{
	public int gameLevel;
	public int victoryCount;
	public int defeatCount;

	public int leftTimeCount;
	public int leftSeconds;
	public JgNation nation;
}

