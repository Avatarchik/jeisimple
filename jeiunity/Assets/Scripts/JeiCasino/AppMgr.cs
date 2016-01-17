using UnityEngine;
using System.Collections;
using Opencoding.Console;


public class AppMgr : MonoBehaviour
{
	public bool facebookEnabled;

	public delegate void OnFacebookLoggedDelegate();

	OnFacebookLoggedDelegate facebookLoginDelegate;

	public static AppMgr ins_;

	#region MonoBehaviour

	AppMgr()
	{
		AppMgr.ins_ = this;
	}
	void Start()
	{
		Refs.appMgr = this;
		JVCMgr.ins_.PushView(Refs.ins_.intro);

		//DebugConsole.IsVisible = true;

		Application.targetFrameRate = 45;
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
		else if (Input.GetKey(KeyCode.Menu))
		{
			DebugConsole.IsVisible = !DebugConsole.IsVisible;
			Debug.Log("DebugConsole.IsVisible: " + DebugConsole.IsVisible);
		}
	}
	#endregion

	IEnumerator StepForward()
	{
		yield return null;

		// First Execution (Welcome!)
		// Login
		// 
	}

	#region Facebook
	public void LoginFacebook(OnFacebookLoggedDelegate callback)
	{
		if (FB.IsInitialized == false)
		{
			FB.Init(OnInitComplete, OnHideUnity);
			facebookLoginDelegate = callback;
		}
		else
			Debug.Log("already logged in \n");
	}
	
	void OnInitComplete()
	{
		Debug.Log("FB Init done.");
		
		if (FB.IsLoggedIn == false)
			FB.Login("public_profile, email, user_friends", AuthCallback);
	}
	void OnHideUnity(bool isGameShown)
	{
		Time.timeScale = isGameShown ? 1f : 0f;
	}
	
	void AuthCallback(FBResult result)
	{
		if (FB.IsLoggedIn)
		{
			Debug.Log(string.Format("<color=white>FB Login worked! UserId({0}), AppId({1}), AccessToekn({2}) </color> \n", FB.UserId, FB.AppId, FB.AccessToken));
		
			facebookLoginDelegate();
		}
		else
		{
			Debug.Log("<color=orange>FB Login failed </color> \n");
		}
	}
	#endregion
}
