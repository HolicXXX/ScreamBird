using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using com.mob;
using cn.sharesdk.unity3d;

public class CameraMove : MonoBehaviour {

	public GameObject _player;
	//private Vector3 offset;
	private float offsetX;

	public TimeCount _timeText;
	public AddBlock _blocks;

	public RawImage _winMask;
	public Text _winTimeCount;

	public RawImage _failedMask;
	public RawImage _rebirthMask;

	private bool _iscalled;

	public GameObject _backGroundOrigin;
	public GameObject _backGroundPrefab;

	private ShareSDK ssdk;

	void Awake(){
		DataManager._isPause = false;
		DataManager._isFailed = false;
		DataManager._isOver = false;
		DataManager._timeCount = 0.0f;
		DataManager._volumNum = PlayerPrefs.GetFloat ("VolumNumber");//LoadJson.LoadSavedUserInfo () ["VolumNumber"].ToString();
		DataManager._didRec = true;
		DataManager._isRec = true;
		DataManager._waitForRebirth = false;
		DataManager._isUsedRebirth = false;
		DataManager._waitOperate = false;

		//if((int)(LoadJson.LoadSavedUserInfo()["Music"]) == 1)
		if(PlayerPrefs.GetInt("Music") == 1)
			GetComponent<AudioSource> ().Play ();

		Messenger<JsonData>.AddListener (GameEvent.UPDATA_SCORE, OnUpdataScoreCallBack);

		Debug.Log (AdsManager.Instance.name);
	}

	// Use this for initialization
	void Start () {
		//offset = transform.position - _player.transform.position;
		offsetX = transform.position.x - _player.transform.position.x;

		//gameObject.GetComponent<Camera> ().orthographicSize = Screen.height / 100.0f / 2.0f * Screen.width / 100.0f / 2.0f / 3.75f;
		_iscalled = false;

		CallNextBackGround (transform.position.x - 20.48f);

		ShareREC.registerApp("1bd5d128e1a4d");

//		ssdk = gameObject.GetComponent<ShareSDK>();
//		ssdk.authHandler = OnAuthResultHandler;
//		ssdk.shareHandler = OnShareResultHandler;
//		ssdk.showUserHandler = OnGetUserInfoResultHandler;
//		ssdk.getFriendsHandler = OnGetFriendsResultHandler;
//		ssdk.followFriendHandler = OnFollowFriendResultHandler;
	}

	void OnDestroy(){
		Messenger<JsonData>.RemoveListener (GameEvent.UPDATA_SCORE, OnUpdataScoreCallBack);
	}

	void OnUpdataScoreCallBack(JsonData data){
		if ((int)(data ["error"]) == 0) {
			Debug.Log ("update success");
			//jd ["Stage_" + DataManager._currentStage.ToString ()] = new JsonData (Mathf.FloorToInt (_timeText._timeCount * 1000.0f));
			PlayerPrefs.SetFloat ("Stage_" + DataManager._currentStage.ToString (), _timeText._timeCount * 1000.0f);
		}
	}

	// Update is called once per frame
	void Update () {
		Vector3 movement = new Vector3 (_player.transform.position.x + offsetX, transform.position.y, transform.position.z);
		transform.position = movement;
		if (DataManager._isOver && !_iscalled) {
			//JsonData jd = LoadJson.LoadSavedUserInfo ();
			if (_winMask != null) {
				Debug.Log ("Call win mask");
				_winMask.gameObject.SetActive (true);
				string tc = TimeCount.stringCut (_timeText._timeCount.ToString ());
				_winTimeCount.text = "Time\n" + tc + "s";
				float lateScore = PlayerPrefs.GetFloat ("Stage_" + DataManager._currentStage.ToString ()) / 1000.0f;
				//float lateScore = (int)(jd["Stage_" + DataManager._currentStage.ToString ()]) / 1000.0f;
				//Debug.Log (lateScore.ToString () + " - " + _timeText._timeCount.ToString ());
				if (lateScore == 0.0f || lateScore > _timeText._timeCount) {
					Debug.Log ("try update");
//					Dictionary<string,string> dic = new Dictionary<string,string> ();
//					dic.Add ("stage", DataManager._currentStage.ToString ());
//					dic.Add ("time", Mathf.FloorToInt (_timeText._timeCount * 1000.0f).ToString ());
//					JsonData data = NetUtil.RequestMsg (NetUtil.MsgUpdateScore, dic);
//
					GetComponent<PlayerManager> ().UpdateScore (DataManager._currentStage.ToString (),Mathf.FloorToInt (_timeText._timeCount * 1000.0f).ToString ());
				}
				GetComponent<AudioSource> ().Stop ();
				//Debug.Log (DataManager._currentStage + " " + PlayerPrefs.GetInt ("PassedStageNum"));
				if (DataManager._currentStage != 3 && DataManager._currentStage > PlayerPrefs.GetInt ("PassedStageNum")) {//PlayerPrefs.GetInt("PassedStageNum")
					//Debug.Log (DataManager._currentStage + " " + PlayerPrefs.GetInt ("PassedStageNum"));
					PlayerPrefs.SetInt ("PassedStageNum", PlayerPrefs.GetInt ("PassedStageNum") + 1);
					//jd ["PassedStageNum"] = new JsonData ((int)(jd ["PassedStageNum"]) + 1);
					DataManager._passedNum += 1;
					if (DataManager._passedNum > 3)
						DataManager._passedNum = 3;
				}
			}
			_iscalled = true;
			DataManager._needUpdateRank = true;
			PlayerPrefs.Save ();
			//LoadJson.SaveUserInfo (jd);
			return;
		}
		if (DataManager._isFailed && !_iscalled) {
			if (_failedMask != null) {
				//check rebirth times
				Debug.Log ("Call failed mask");
				_failedMask.gameObject.SetActive (true);
				GetComponent<AudioSource> ().Stop ();
			}
			_iscalled = true;
			DataManager._needUpdateRank = true;
			return;
		}

		if (DataManager._waitForRebirth) {
			if (!_rebirthMask.gameObject.activeInHierarchy) {
				_rebirthMask.gameObject.SetActive (true);
			}
			return;
		}

		if (_player.transform.position.y - transform.position.y < -Screen.height / 200.0f && !DataManager._isFailed) {
			Debug.Log ("out of sight");
			_player.GetComponent<PlayerController> ().SetDead ();
//			_player.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
//			_player.GetComponent<Rigidbody2D> ().isKinematic = true;
//			_player.GetComponent<Collider2D> ().enabled = false;
//			DataManager._isFailed = true;
		}
	}

	public void CallNextBackGround(float lastX){
		//add to _backGroundOrigin	
		GameObject bg = GameObject.Instantiate(_backGroundPrefab);
		bg.GetComponent<BackGroundMover> ()._mainCamera = gameObject;
		bg.transform.parent = _backGroundOrigin.transform;
		bg.transform.position = new Vector2 (lastX + bg.GetComponent<BackGroundMover> ()._Width / 100.0f,transform.position.y);
		//bg.transform.localScale = new Vector3 (1.0f, Screen.height / 1334.0f, 1.0f);
	}

	public void RebirthPlayer(){
		Vector3 movement = new Vector3 (_player.transform.position.x + offsetX, transform.position.y, transform.position.z);
		transform.position = movement;

		var stairList = _blocks.gameObject.GetComponentsInChildren<StairMovement> ();
		Debug.Log ("stair count: " + stairList.Length);
		for (int i = 0; i < stairList.Length; ++i) {
			stairList [i].resetStairState ();
		}

		_player.GetComponent<PlayerController> ().RebirthPlayer ();
	}


//	void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
//	{
//		if (state == ResponseState.Success)
//		{
//			print ("authorize success !" + "Platform :" + type);
//		}
//		else if (state == ResponseState.Fail)
//		{
//			#if UNITY_ANDROID
//			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
//			#elif UNITY_IPHONE
//			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
//			#endif
//		}
//		else if (state == ResponseState.Cancel) 
//		{
//			print ("cancel !");
//		}
//	}
//
//	void OnGetUserInfoResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
//	{
//		if (state == ResponseState.Success)
//		{
//			print ("get user info result :");
//			print (MiniJSON.jsonEncode(result));
//			print ("Get userInfo success !Platform :" + type );
//		}
//		else if (state == ResponseState.Fail)
//		{
//			#if UNITY_ANDROID
//			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
//			#elif UNITY_IPHONE
//			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
//			#endif
//		}
//		else if (state == ResponseState.Cancel) 
//		{
//			print ("cancel !");
//		}
//	}
//
//	void OnShareResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
//	{
//		if (state == ResponseState.Success)
//		{
//			print ("share successfully - share result :");
//			print (MiniJSON.jsonEncode(result));
//		}
//		else if (state == ResponseState.Fail)
//		{
//			#if UNITY_ANDROID
//			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
//			#elif UNITY_IPHONE
//			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
//			#endif
//		}
//		else if (state == ResponseState.Cancel) 
//		{
//			print ("cancel !");
//		}
//	}
//
//	void OnGetFriendsResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
//	{
//		if (state == ResponseState.Success)
//		{			
//			print ("get friend list result :");
//			print (MiniJSON.jsonEncode(result));
//		}
//		else if (state == ResponseState.Fail)
//		{
//			#if UNITY_ANDROID
//			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
//			#elif UNITY_IPHONE
//			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
//			#endif
//		}
//		else if (state == ResponseState.Cancel) 
//		{
//			print ("cancel !");
//		}
//	}
//
//	void OnFollowFriendResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
//	{
//		if (state == ResponseState.Success)
//		{
//			print ("Follow friend successfully !");
//		}
//		else if (state == ResponseState.Fail)
//		{
//			#if UNITY_ANDROID
//			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
//			#elif UNITY_IPHONE
//			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
//			#endif
//		}
//		else if (state == ResponseState.Cancel) 
//		{
//			print ("cancel !");
//		}
//	}

}
