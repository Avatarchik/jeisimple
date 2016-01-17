using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;


public class TestClient : MonoBehaviour 
{
	public string message = "Hello server, I'm a client.\n";

	char[] buffer = new char[1024];

	//byte[] header = new byte[4];

	TcpClient client = null;
	StreamReader readerStream;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1f);
		RunClient();
	}

//	void FixedUpdate()
//	{
//		if (readerStream == null) return;
//
//		int received = readerStream.Read(buffer, 0, 4);
//		Debug.Log(string.Format("received bytes: ({0}) \n", received));
//		if (received > 0)
//		{
//			System.Buffer.BlockCopy(buffer, 0, header, 0, 4);
//
//			short length = System.BitConverter.ToInt16(header, 0);
//
//			if (length > 4)
//			{
//				received = readerStream.Read(buffer, 4, length - 4);
//
//				Debug.Log(string.Format("total received: ({0}) \n", length + 4));
//			}
//		}
//		else
//			Debug.Log("received nothing \n");
//	}

	public void RunClient()
	{
		//Console.WriteLine ("Hello World!");
		
		try
		{
			//LocalHost에 지정포트로 TCP Connection생성 후 데이터 송수신 스트림 얻음
			client = new TcpClient();
			client.Connect("127.0.0.1", TestServer.PORT);
			NetworkStream writeStream = client.GetStream();

			//NetworkStream stream = client.GetStream();

			Encoding encode = System.Text.Encoding.UTF8; //.GetEncoding("ks_c_5601-1987");
			readerStream = new StreamReader(writeStream, encode);
			
			//보낼 데이터를 읽어 Default형식의 바이트 스트림으로 변환
			string dataToSend = message; //Console.ReadLine();
			byte[] data = Encoding.Default.GetBytes(dataToSend);

//			while (true)
//			{
//				//readerStream.BaseStream
//			}

			//while (true)
			{
				dataToSend += "\r\n";
				data = Encoding.Default.GetBytes(dataToSend);
				writeStream.Write(data, 0, data.Length);
				
				
//				if (dataToSend.IndexOf("<EOF>") > -1)
//					break;
				
//				string returnData;
				//returnData = readerStream.ReadLine();
				int received = readerStream.Read(buffer, 0, 4);
				Debug.Log(string.Format("received bytes: ({0}) \n", received));

				//Console.WriteLine("server : " + returnData);
				//Debug.Log("from server : " + returnData);

				//dataToSend = Console.ReadLine();
			}
		}
		
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
		}
		finally
		{
			client.Close();
		}
	}
}
