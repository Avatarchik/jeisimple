using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Facebook.MiniJSON;


public struct FbUser
{
	public string id;
	public string pictureUrl;
	public string firstName;
	public string name;
}

public class JFacebook : MonoBehaviour
{
	FbUser player = new FbUser();
	List<FbUser> appUsers;
	List<FbUser> invitableUsers;


	public Image fbProfilePic;
	public Text fbUserName; 

	public RawImage testRawImage;

	public InputField inputText;
	public Text outputText;
	public Text outputText2;
	
	public static JFacebook ins_;
//	void Awake()
//	{
//		JFacebook.ins_ = this;
//		FB.Init(OnInitComplete, OnHideUnity);
//	}

	public void InitializeFacebook()
	{
		FB.Init(OnInitComplete, OnHideUnity);
	}

	void OnInitComplete()
	{
		Debug.Log("FB Init done.");

		if (FB.IsLoggedIn == false)
			FBLogin();
	}

	void OnHideUnity(bool isGameShown)
	{
		if (!isGameShown)
		{
			Time.timeScale = 0f;
		}
		else 
		{
			Time.timeScale = 1f;
		}
	}

	void FBLogin()
	{
		//FB.Login("user_about_me, user_birthday", AuthCallback);
		FB.Login("public_profile, email, user_friends", AuthCallback);
	}

	void AuthCallback(FBResult result)
	{
		if (FB.IsLoggedIn)
		{
			Debug.Log(string.Format("<color=white>FB Login worked! UserId({0}), AppId({1}), AccessToekn({2}) </color> \n", FB.UserId, FB.AppId, FB.AccessToken));
		}
		else
		{
			Debug.Log("<color=orange>FB Login failed </color> \n");
		}
	}

	public void LoginFacebook()
	{
		if (FB.IsLoggedIn)
		{
			Debug.Log("FB Logged In");
		}
		else
		{
			FBLogin();
		}
	}

	#region Me
	const int Texture_Size = 100;
	public void GetMyData()
	{
		string queryString = JFBUtil.GetPictureURL("me", Texture_Size, Texture_Size);
		Debug.Log("queryString: \n" + queryString);

		FB.API(queryString, Facebook.HttpMethod.GET, DealWithProfilePicture);
	}
	 
	void DealWithProfilePicture(FBResult result)
	{
		if (result.Error != null)
		{
			Debug.Log("Problem with getting profile picture.");

			FB.API(JFBUtil.GetPictureURL("me", Texture_Size, Texture_Size), Facebook.HttpMethod.GET, DealWithProfilePicture);
			return;
		}
		Debug.Log(string.Format("result: {0} \n", result.Text));
		outputText.text = result.Text;

		Debug.Log(string.Format("Texture w({0}), h({1}), texelSize({2})", result.Texture.width, result.Texture.height, result.Texture.texelSize));

		//fbProfilePic.sprite = Sprite.Create(result.Texture, new Rect(0,0,Texture_Size,Texture_Size), new Vector2(0,0));

		testRawImage.texture = result.Texture;

		ParsePlayerData(result.Text);

		outputText.text = "player PicUrl: " + player.pictureUrl;
	}
	void ParsePlayerData(string jsonString)
	{
		var responseObject = Json.Deserialize(jsonString) as Dictionary<string, object>;
		object dataH;
		if (responseObject.TryGetValue("data", out dataH))
		{
			player.pictureUrl = (dataH as Dictionary<string, object>)["url"] as string;
		}

		JUrlPictureMgr.ins_.GetTexture("me", player.pictureUrl, OnCompleteMyProfilePic);
	}
	#endregion

	void OnCompleteMyProfilePic(string id, Texture2D tex)
	{
		//fbProfilePic.sprite = Sprite.Create(tex, new Rect(0, 0, 128,128), new Vector2(0, 0));

		testRawImage.texture = tex;
	}

	#region Me & Friends
	public void GetMeAndFriends()
	{
		// Reqest player info and profile picture                                                                           
		FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id,picture)", Facebook.HttpMethod.GET, MeAndFriendsCallback); 
	}
	void MeAndFriendsCallback(FBResult result)
	{                                                                                              
		if (result.Error != null)                                                                                                  
		{                                                                                                                          
			JFBUtil.LogError(result.Error);                                                                                           
			// Let's just try again                                                                                                
			FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, MeAndFriendsCallback);     
			return;                                                                                                                
		}      
		Debug.Log("result.Text: " + result.Text);
		outputText.text = result.Text;

		Dictionary<string, string> profileUrl = JFBUtil.DeserializeJSONProfile(result.Text); 
		List<object> friends = JFBUtil.DeserializeJSONFriends(result.Text);   

		//Json.Serialize(friends);
		Debug.Log("profileUrl: " + ToString(profileUrl));
		Debug.Log("friends: " +  Json.Serialize(friends));//ToString(friends));

		outputText2.text = Json.Serialize(friends);//ToString(friends);
	}

	#endregion

	string ToString(List<object> list)
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		foreach (var elem in list)
		{
			sb.Append(elem + ", ");
		}
		return sb.ToString();
	}
	string ToString(Dictionary<string, string> dic)
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		foreach (var pair in dic)
		{
			sb.Append(string.Format("({0}, {1}), ", pair.Key, pair.Value));
		}
		return sb.ToString();
	}
	string ToString(List<FbUser> users)
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		foreach (var user in users)
		{
			sb.Append(string.Format("({0}, {1}, {2}, {3}), \n", user.firstName, user.name, user.id, user.pictureUrl));
		}
		return sb.ToString();
	}
	#region Friends
	public void GetFriends()
	{
		string queryString = "/me/invitable_friends";
		Debug.Log("queryString: \n" + queryString);

		FB.API(queryString, Facebook.HttpMethod.GET, DealWithFriends);
	}
	void DealWithFriends(FBResult result)
	{
		Debug.Log(string.Format("result: {0} \n", result.Text));
		outputText.text = result.Text;

		List<object> friends = JFBUtil.DeserializeJSONFriends(result.Text);   
		outputText2.text = Json.Serialize(friends);

		ParseInvitableFriends(result.Text);
	}
	void ParseInvitableFriends(string jsonString)
	{
		var responseObject = Json.Deserialize(jsonString) as Dictionary<string, object>;

		object friendsH;
		var friends = new List<object>(64);
		if (responseObject.TryGetValue("data", out friendsH))
		{
			friends = (List<object>)friendsH;
		}
		ConstructInvitableUsers(friends, ref invitableUsers);

		outputText.text = ToString(invitableUsers);
		Debug.Log("invitableUsers: " + ToString(invitableUsers));
	}
	void ConstructInvitableUsers(List<object> usersData, ref List<FbUser> users)
	{
		users = new List<FbUser>(16);
		foreach (Dictionary<string, object> user in usersData)
		{
			FbUser newUser = new FbUser();
			newUser.name = user["name"] as string;
			newUser.id = user["id"] as string;
			newUser.pictureUrl = ((user["picture"] as Dictionary<string, object>)["data"] as Dictionary<string, object>)["url"] as string;
			
			users.Add(newUser);	
		}
	}
	#endregion

	#region AppUsers
	public void GetAppUsers()
	{
//		string queryString = "/me/friends?fields=installed";
//		FB.API(queryString, Facebook.HttpMethod.GET, DealWithFriends);
		//FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id,picture)", Facebook.HttpMethod.GET, DealWithAppUsers); 
		//FB.API("/me?fields(first_name,id,picture),friends.limit(100).fields(first_name,id,picture)", Facebook.HttpMethod.GET, DealWithAppUsers); 
	
		FB.API("/me?fields=id,first_name,picture,friends.limit(100).fields(first_name,id,picture)", Facebook.HttpMethod.GET, DealWithAppUsers); 
	}
	void DealWithAppUsers(FBResult result)
	{
		Debug.Log(string.Format("result: {0} \n", result.Text));
		outputText.text = result.Text;
		ParseMeAndAppFriends(result.Text);
	}
	public void ParseMeAndAppFriends(string jsongString)
	{
		var responseObject = Json.Deserialize(jsongString) as Dictionary<string, object>;

		object friendsH;
		var friends = new List<object>(16);
		if (responseObject.TryGetValue("friends", out friendsH))
		{
			friends = (List<object>)(((Dictionary<string, object>)friendsH)["data"]);
		}

		Debug.Log("Num App Users: " + friends.Count);

		appUsers = new List<FbUser>(16);
		ConstructUsers(friends, ref appUsers);
//		foreach (Dictionary<string, object> user in friends)
//		{
//			User newUser;
//			newUser.firstName = user["first_name"] as string;
//			newUser.id = user["id"] as string;
//			newUser.pictureUrl = ((user["picture"] as Dictionary<string, object>)["data"] as Dictionary<string, object>)["url"] as string;
//			
//			appUsers.Add(newUser);	
//		}

		Debug.Log(ToString(appUsers));
		outputText.text = ToString(appUsers);
	}
	#endregion

	void ConstructUsers(List<object> usersData, ref List<FbUser> users)
	{
		users = new List<FbUser>(16);
		foreach (Dictionary<string, object> user in usersData)
		{
			FbUser newUser = new FbUser();
			newUser.firstName = user["first_name"] as string;
			newUser.id = user["id"] as string;
			newUser.pictureUrl = ((user["picture"] as Dictionary<string, object>)["data"] as Dictionary<string, object>)["url"] as string;
			
			users.Add(newUser);	
		}
	}

	public void RunGraphApi()
	{
		string queryString = inputText.text;
		FB.API(queryString, Facebook.HttpMethod.GET, DealWithGraphApi);
	}

	void DealWithGraphApi(FBResult result)
	{
		Debug.Log(string.Format("result: {0} \n", result.Text));
		outputText.text = result.Text;
	}

	public void ClearOutputText()
	{
		outputText.text = string.Empty;
	}
}
