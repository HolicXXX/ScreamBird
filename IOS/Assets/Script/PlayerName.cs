using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class PlayerName : MonoBehaviour {

	public RawImage _changeNameMask;

	// Use this for initialization
	void Start () {
		OnUpdataUserInfoCallBack ();
		Messenger.AddListener (GameEvent.UPDATA_USER_INFO,OnUpdataUserInfoCallBack);
		//Messenger<JsonData>.AddListener (GameEvent.UPDATE_NICK_NAME, OnUpdataNickNameCallBack);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnClickCallBackName(){
		if (_changeNameMask != null) {
			_changeNameMask.gameObject.SetActive (true);
		}
	}

	void OnUpdataUserInfoCallBack(){
		Debug.Log ("updata name callback");
		string name = PlayerPrefs.GetString ("PlayerName");//LoadJson.LoadSavedUserInfo () ["PlayerName"].ToString ()
		gameObject.GetComponent<Text> ().text = name;
	}

	void OnDestroy(){
		Messenger.RemoveListener (GameEvent.UPDATA_USER_INFO, OnUpdataUserInfoCallBack);
		//Messenger<JsonData>.RemoveListener (GameEvent.UPDATE_NICK_NAME, OnUpdataNickNameCallBack);
	}

//	void OnUpdataNickNameCallBack(JsonData jd){
//		Debug.Log ("updata nickname callback");
//		string name = PlayerPrefs.GetString ("PlayerName");//LoadJson.LoadSavedUserInfo () ["PlayerName"].ToString ()
//		gameObject.GetComponent<Text> ().text = name;
//	}
}	
