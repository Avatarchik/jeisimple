using UnityEngine;
using System.Collections;
//using Opencoding.Console;

public class VCLoginSignup : JViewController
{
	public GameObject debugConsole;

//	void Start()
//	{
//		Refs.appMgr.LoginFacebook(OnFacebookLogged);
//	}

	public void LoginFacebook()
	{
		Refs.appMgr.LoginFacebook(OnFacebookLogged);
	}
	public void LoginLater()
	{
		JVCMgr.ins_.PushView(Refs.ins_.slotLobby);
	}
	public void ShareOnFacebook()
	{
		FB.Feed(
			linkCaption: "I'm playing the JCasino",
			picture: "http://greyzoned.com/images/evilelf2_icon.png",
			linkName: "Check out this game",
			link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
			);
	}
	public void InviteFriends()
	{
		FB.AppRequest(
			message: "This game is awesome, join me now.",
			title: "Invite your friends to join you"
			);
	}

	void OnFacebookLogged()
	{
		JVCMgr.ins_.PushView(Refs.ins_.slotLobby);
	}

//	#region Facebook Login
//	void OnInitComplete()
//	{
//		Debug.Log("FB Init done.");
//		
//		if (FB.IsLoggedIn == false)
//			FB.Login("public_profile, email, user_friends", AuthCallback);
//	}
//	void OnHideUnity(bool isGameShown)
//	{
//		Time.timeScale = isGameShown ? 1f : 0f;
//	}
//
//	void AuthCallback(FBResult result)
//	{
//		if (FB.IsLoggedIn)
//		{
//			Debug.Log(string.Format("<color=white>FB Login worked! UserId({0}), AppId({1}), AccessToekn({2}) </color> \n", FB.UserId, FB.AppId, FB.AccessToken));
//		}
//		else
//		{
//			Debug.Log("<color=orange>FB Login failed </color> \n");
//		}
//	}
//	#endregion
}
