using UnityEngine;
using System.Collections;
using System.IO;


public class JUrlPictureMgr : MonoBehaviour
{
	string sourcePath;

	public delegate void FileIODelegate(string id, Texture2D tex);
	//public delegate void FileIODelegate2(string id, Texture2D tex);


	public static JUrlPictureMgr ins_;
	public JUrlPictureMgr()
	{
		JUrlPictureMgr.ins_ = this;
		sourcePath = Application.dataPath;
		Debug.Log("persistentDataPath " + sourcePath);
	}

	public void GetTexture(string id, string url, FileIODelegate ioCallback)
	{
		if (string.IsNullOrEmpty(sourcePath))
		{
			sourcePath = Application.persistentDataPath;
			Debug.Log("persistentDataPath " + sourcePath);
		}

		string filePath = sourcePath + "/" + id;
		if (File.Exists(filePath))
		{
			Debug.Log("Reading file for texture " + filePath);

#if !UNITY_WEBPLAYER	
			var bytes = File.ReadAllBytes(filePath);
			var tex = new Texture2D(1, 1);
			tex.LoadImage(bytes);
			ioCallback(id, tex);
#endif
		}
		else
			DownloadPicture(id, url, ioCallback);
	}

	void DownloadPicture(string id, string url, FileIODelegate ioCallback)
	{
		this.StartCoroutine(GetPicture(id, url, ioCallback));
	}

	IEnumerator GetPicture(string id, string url, FileIODelegate ioCallback)
	{
		WWW loader = new WWW(url);

		yield return loader;

#if UNITY_WEBPLAYER
		ioCallback(id, loader.texture);
#else
		string filePath = sourcePath + "/" + id;
		File.WriteAllBytes(filePath, loader.bytes);

		GetTexture(id, url, ioCallback);
#endif
	}
}
