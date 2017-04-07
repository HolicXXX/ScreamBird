using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using cn.sharesdk.unity3d;

public class StartSceneCamere : MonoBehaviour {

	//public Text _testText;

	private float offsetX;

	public Text _name;

	public RawImage _exitMask;

	private ShareSDK ssdk;

	public GameObject _helpMask;

	public RawImage _networkMask;

	public RawImage _loadDataMask;

	void Awake(){
		Debug.Log ("StartSceneCamera");
		Messenger<JsonData>.AddListener (GameEvent.GET_USER_INFO, OnGetUserInfoCallBack);

		if (!PlayerPrefs.HasKey ("PlayerName") || PlayerPrefs.GetString ("PlayerName") == "Name") {

			PlayerPrefs.SetString ("PlayerName", "Name");
			PlayerPrefs.SetInt ("Money", 0);
			PlayerPrefs.SetInt ("TodayGainMoney", 0);
			PlayerPrefs.SetString ("ShareID", "");
			PlayerPrefs.SetFloat ("VolumNumber", 0.6f);
			PlayerPrefs.SetInt ("PassedStageNum", 0);
			PlayerPrefs.SetInt ("Music", 0);
			PlayerPrefs.SetInt ("Sound", 1);
			PlayerPrefs.SetFloat ("Stage_1", 0.0f);
			PlayerPrefs.SetFloat ("Stage_2", 0.0f);
			PlayerPrefs.SetFloat ("Stage_3", 0.0f);
			PlayerPrefs.SetInt ("AdsCount", 2);
			PlayerPrefs.SetInt ("CurrentSkin", 0);
			PlayerPrefs.SetInt ("SkinState_0", 1);
			PlayerPrefs.SetInt ("SkinState_1", 0);
			PlayerPrefs.SetInt ("SkinState_2", 0);
			PlayerPrefs.SetInt ("SkinState_3", 0);

			PlayerPrefs.SetInt ("SkinPrice_1", 999);
			PlayerPrefs.SetInt ("SkinPrice_2", 999);
			PlayerPrefs.SetInt ("SkinPrice_3", 999);

			_helpMask.SetActive (true);

			if (Application.internetReachability == NetworkReachability.NotReachable) {
				_networkMask.gameObject.SetActive (true);
			}
		}
	}

	public void OnRetryNetworkCallBack(){
		if (Application.internetReachability != NetworkReachability.NotReachable) {
			GetComponent<PlayerManager> ().StageInfo ();
			GetComponent<PlayerManager> ().GetUserInfo ();
			GetComponent<PlayerManager> ().ShopList ();
		}
	}

	void OnGetUserInfoCallBack(JsonData jsdArray){
		if (_networkMask.gameObject.activeInHierarchy) {
			_networkMask.gameObject.SetActive (false);
		}
		Debug.Log ("get user info call back");
		setUserinfo (jsdArray);
		Debug.Log (JsonMapper.ToJson (jsdArray));
		if ((int)(jsdArray ["error"]) == 0) {
			Debug.Log ("broadcast updateinfo");
			Messenger.Broadcast (GameEvent.UPDATA_USER_INFO);
		}
	}

	void setUserinfo(JsonData jsdArray){
		DataManager._passedNum = PlayerPrefs.GetInt ("PassedStageNum");
		DataManager._needUpdateRank = true;

		//JsonData jsdArray = NetUtil.RequestMsg (NetUtil.MsgGetUserInfo, new Dictionary<string, string>());
		Debug.Log ("jsdArray: " + jsdArray["error"]);
		Debug.Log ("name: " + jsdArray ["data"] ["nickname"].ToString());
		if ((int)(jsdArray ["error"]) == 0) {
			PlayerPrefs.SetString ("PlayerName", jsdArray ["data"]["nickname"].ToString());
			try
			{
				PlayerPrefs.SetInt ("Money", int.Parse(jsdArray ["data"] ["money"].ToString()));
			}
			catch(KeyNotFoundException ex){
				PlayerPrefs.SetInt ("Money", 0);
			}
			try{
				PlayerPrefs.SetInt ("TodayGainMoney", int.Parse(jsdArray ["data"] ["today_gain_money"].ToString()));
			}catch(KeyNotFoundException ex){
				PlayerPrefs.SetInt ("TodayGainMoney", 0);
			}

			PlayerPrefs.SetString ("ShareID", jsdArray ["data"] ["share_id"].ToString ());
			try{
				PlayerPrefs.SetInt ("AdsCount", int.Parse(jsdArray ["data"] ["ads_param"].ToString()));
			}catch(KeyNotFoundException ex){
				PlayerPrefs.SetInt ("AdsCount", 3);
			}
			//skin state
			try{
				Debug.Log(jsdArray ["data"] ["my_skin"].ToString());
				JsonData jd = jsdArray ["data"] ["my_skin"];
				Debug.Log("IsArray: " + jd.IsArray);
				for(int i = 0;i< jd.Count;++i){
					Debug.Log(jd[i].ToString());
					PlayerPrefs.SetInt(string.Format("SkinState_{0}", int.Parse(jd[i].ToString())),1);

				}
			}catch(KeyNotFoundException ex){
				PlayerPrefs.SetInt ("SkinState_1", 0);
				PlayerPrefs.SetInt ("SkinState_2", 0);
				PlayerPrefs.SetInt ("SkinState_3", 0);
			}

		} else {
			PlayerPrefs.SetString ("PlayerName", "Name");
			PlayerPrefs.SetInt ("Money", 0);
			PlayerPrefs.SetInt ("TodayGainMoney", 0);
			PlayerPrefs.SetString ("ShareID", "");
			PlayerPrefs.SetInt ("AdsCount", 3);
			PlayerPrefs.SetInt ("SkinState_1", 0);
			PlayerPrefs.SetInt ("SkinState_2", 0);
			PlayerPrefs.SetInt ("SkinState_3", 0);
		}

		PlayerPrefs.Save ();
	}

	// Use this for initialization
	void Start () {
		//gameObject.GetComponent<Camera> ().orthographicSize = Screen.height / 100.0f / 2.0f * Screen.width / 100.0f / 2.0f / 3.75f;
		//offsetX = transform.position.x - _player.transform.position.x;

		ssdk = gameObject.GetComponent<ShareSDK>();
		ssdk.authHandler = OnAuthResultHandler;
		ssdk.shareHandler = OnShareResultHandler;
		ssdk.showUserHandler = OnGetUserInfoResultHandler;
		ssdk.getFriendsHandler = OnGetFriendsResultHandler;
		ssdk.followFriendHandler = OnFollowFriendResultHandler;

		if (DataManager._isFirstEnterStartScene) {
			DataManager._isFirstEnterStartScene = false;
			DataManager._deathCount = 0;
			GetComponent<PlayerManager> ().StageInfo ();
		}

		Debug.Log ("get user info");
		GetComponent<PlayerManager> ().GetUserInfo ();
		GetComponent<PlayerManager> ().ShopList ();
	}

	void OnDestroy(){
		Messenger<JsonData>.RemoveListener (GameEvent.GET_USER_INFO, OnGetUserInfoCallBack);
	}

	// Update is called once per frame
	void Update () {
		//transform.position = new Vector2 (_player.transform.x + offsetX, transform.position.y);
		if(Input.GetKeyDown(KeyCode.Escape)){
			if (_exitMask != null) {
				_exitMask.gameObject.SetActive (true);
			}
		}
	}

	void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("authorize success !" + "Platform :" + type);
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel!");
		}
	}

	void OnGetUserInfoResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("get user info result :");
			print (MiniJSON.jsonEncode(result));
			print ("Get userInfo success !Platform :" + type );
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void OnShareResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("share successfully - share result :");
			print (MiniJSON.jsonEncode(result));
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void OnGetFriendsResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{			
			print ("get friend list result :");
			print (MiniJSON.jsonEncode(result));
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void OnFollowFriendResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("Follow friend successfully !");
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

}
