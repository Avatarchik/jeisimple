using UnityEngine;
using System.Collections;
using Thrift;
using Thrift.Protocol;
using Thrift.Transport;
using System.IO;

public class JThrift : MonoBehaviour
{
	void Start()
	{
		TestThrift();
	}


	public static void TestThrift()
	{
		TestStruct1 test1Message = new TestStruct1();
		test1Message.Id = 1487;
		test1Message.UserName = "Jane";

		byte[] buffer = new byte[1024];
		Serialize(test1Message, ref buffer);

		TestStruct1 outMessage1 = new TestStruct1();
		Deserialize(buffer, outMessage1);

		Debug.Log(outMessage1.ToString());

		// refactor: use the assert of the 'unit test'
	}

//	public static void Serialize(TBase message, ref byte[] buffer)
//	{
//		MemoryStream outputStream = new MemoryStream(buffer);
//
//		TStreamTransport transport = new TStreamTransport(null, outputStream);
//		TBinaryProtocol binaryProtocol = new TBinaryProtocol(transport);
//
//		message.Write(binaryProtocol);
//	}
//	public static long Serialize(TBase message, ref byte[] buffer, int offset)
//	{
//		MemoryStream outputStream = new MemoryStream(buffer, offset, buffer.Length - offset);
//		
//		TStreamTransport transport = new TStreamTransport(null, outputStream);
//		TBinaryProtocol binaryProtocol = new TBinaryProtocol(transport);
//	
//		message.Write(binaryProtocol);
//
//		return outputStream.Length;
//	}

	/// <summary>
	/// Serialize the specified message and buffer.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="buffer">Buffer.</param>
	public static long Serialize(TBase message, ref byte[] buffer)
	{
		MemoryStream outputStream = new MemoryStream(buffer);
		
		TStreamTransport transport = new TStreamTransport(null, outputStream);
		TBinaryProtocol binaryProtocol = new TBinaryProtocol(transport);

		message.Write(binaryProtocol);

		return outputStream.Position;
	}
	public static long Serialize(TBase message, ref byte[] buffer, int offset)
	{
		MemoryStream outputStream = new MemoryStream(buffer, offset, buffer.Length - offset);
		
		TStreamTransport transport = new TStreamTransport(null, outputStream);
		TBinaryProtocol binaryProtocol = new TBinaryProtocol(transport);
		
		message.Write(binaryProtocol);
		
		return outputStream.Position;
	}
	
	public static int Serialize(byte messageType, TBase message, ref byte[] buffer)
	{
		long thriftLength = JThrift.Serialize(message, ref buffer, 4);

		short totalLength = (short) (thriftLength + 4);
		byte[] lengthBytes = System.BitConverter.GetBytes(totalLength);

		buffer[0] = lengthBytes[0];
		buffer[1] = lengthBytes[1];
		buffer[2] = (byte)messageType;

		return totalLength;
	}


	public static void Deserialize(byte[] buffer, TBase message)
	{
		MemoryStream inputStream = new MemoryStream(buffer);
		
		TStreamTransport transport = new TStreamTransport(inputStream, null);
		TBinaryProtocol binaryProtocol = new TBinaryProtocol(transport);

		message.Read(binaryProtocol);
	}

	public static void Deserialize(byte[] buffer, int offset, TBase message)
	{
		MemoryStream inputStream = new MemoryStream(buffer, offset, buffer.Length - offset);
		
		TStreamTransport transport = new TStreamTransport(inputStream, null);
		TBinaryProtocol binaryProtocol = new TBinaryProtocol(transport);
		
		message.Read(binaryProtocol);
	}
}
