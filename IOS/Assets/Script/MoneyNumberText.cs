using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyNumberText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		OnUpdataUserInfoCallBack ();
		Messenger.AddListener (GameEvent.UPDATA_USER_INFO, OnUpdataUserInfoCallBack);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnUpdataUserInfoCallBack(){
		Debug.Log ("updata money callback");
		int num = PlayerPrefs.GetInt ("Money");
		gameObject.GetComponent<Text> ().text = num.ToString ();
	}

	void OnDestroy(){
		Messenger.RemoveListener (GameEvent.UPDATA_USER_INFO, OnUpdataUserInfoCallBack);
	}
}
