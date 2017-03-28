using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class ExchangeCodeForMoney : MonoBehaviour {

	public Text _code;

	public GameObject _tips;

	// Use this for initialization
	void Start () {
		Messenger<JsonData>.AddListener (GameEvent.EXCHANGE_CODE, GetExchangeResponse);
	}

	void OnDestroy(){
		Messenger<JsonData>.RemoveListener (GameEvent.EXCHANGE_CODE, GetExchangeResponse);
	}

	public void OnClickCallBack(){
		if(_code.text.Length != 0)
			Camera.main.GetComponent<PlayerManager> ().ActivateCode (_code.text);
	}

	private static Dictionary<int,string> tipString = new Dictionary<int, string>(){
		{0,"激活成功，获得20金币"},
		{4,"激活码已使用"},
		{5,"此用户已使用"},
		{7,"激活码不存在"}
	};

	public void GetExchangeResponse(JsonData res){
		int error = (int)(res ["error"]);
		if (error == 0) {
			Debug.Log ("Exchange Successed");
			PlayerPrefs.SetInt ("Money", PlayerPrefs.GetInt ("Money") + int.Parse (res ["data"] ["money"].ToString ()));
			Messenger.Broadcast (GameEvent.UPDATA_USER_INFO);
		}
		GameObject canvas = GameObject.Find ("Canvas");
		GameObject tip = GameObject.Instantiate (_tips, canvas.transform);
		var rect = tip.transform as RectTransform;
		rect.anchoredPosition = new Vector2 (0, 0);
		rect.sizeDelta = new Vector2 (Screen.width, Screen.height);
		tip.GetComponentInChildren<Text> ().text = tipString [error].ToString ();
	}
}
