using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShareMoneyCount : MonoBehaviour {

	public PlayerManager _manager;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Text> ().text = PlayerPrefs.GetInt ("TodayGainMoney").ToString();
		Messenger.AddListener (GameEvent.UPDATA_USER_INFO, OnUpdataUserInfoCallBack);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnEnable(){
		if(SceneManager.GetActiveScene().name == "StartScene")
			_manager.GetUserInfo ();
	}

	void OnDestroy(){
		Messenger.RemoveListener (GameEvent.UPDATA_USER_INFO, OnUpdataUserInfoCallBack);
	}

	void OnUpdataUserInfoCallBack(){
		Debug.Log ("TodayGainMoney : " + PlayerPrefs.GetInt ("TodayGainMoney").ToString ());
		gameObject.GetComponent<Text> ().text = PlayerPrefs.GetInt ("TodayGainMoney").ToString();
	}

}
