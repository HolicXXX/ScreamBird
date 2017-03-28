using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class ChangeNameScript : MonoBehaviour {

	public Text _namelabel;

	public RawImage _tip;

	public RawImage _mask;

	public PlayerManager _manager;

	// Use this for initialization
	void Start () {
		GetComponent<InputField> ().placeholder.GetComponent<Text> ().text = _namelabel.text;
		Messenger<JsonData>.AddListener (GameEvent.UPDATE_NICK_NAME, OnEndChangeNameInput);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnEnable(){
		GetComponent<InputField> ().placeholder.GetComponent<Text> ().text = _namelabel.text;
	}

	public void OnEndChangeNameInput(JsonData response){
//		input = gameObject.GetComponent<InputField> ().textComponent.text;
//		Dictionary<string,string> dic = new Dictionary<string,string> ();
//		dic.Add ("nickname", input);
//		Debug.Log ("input name: " + input + "input name length: " + input.Length);
//		JsonData response = NetUtil.RequestMsg (NetUtil.MsgSetNickame, dic);
		if ((int)(response ["error"]) != 0) {
			Debug.Log ("change name failed");
			Debug.Log ("error: " + response ["error"].ToString ());
			Debug.Log ("msg: " + response ["msg"].ToString ());
			if (_tip != null) {
				if ((int)(response ["error"]) == 5) {
					_tip.GetComponent<TipCloseScript> ()._tipText.text = "只能修改一次昵称哦~";
				} else {
					_tip.GetComponent<TipCloseScript> ()._tipText.text = "昵称重复，请重新输入！";
				}
				_tip.GetComponent<TipCloseScript> ()._closeMask = false;
				_tip.gameObject.SetActive (true);
			}

		} else {
//			JsonData jd = LoadJson.LoadSavedUserInfo ();
//			_namelabel.text = input;
//			jd ["PlayerName"] = new JsonData(input);
//			LoadJson.SaveUserInfo (jd);
			PlayerPrefs.SetString("PlayerName",gameObject.GetComponent<InputField> ().textComponent.text);
			if (_tip != null) {
				_tip.GetComponent<TipCloseScript> ()._tipText.text = "昵称修改成功！";
				_tip.GetComponent<TipCloseScript> ()._closeMask = true;
				_tip.gameObject.SetActive (true);
			}
			Messenger.Broadcast (GameEvent.UPDATA_USER_INFO);
		}
	}

	public void OnCancelCallBack(){
		if (_mask) {
			GetComponent<AudioSource> ().Play ();
			_mask.gameObject.SetActive (false);
		}
	}

	public void OnConfirmCallBack(){
		GetComponent<AudioSource> ().Play ();
		_manager.SetNickame (gameObject.GetComponent<InputField> ().textComponent.text);
		//OnEndChangeNameInput (gameObject.GetComponent<InputField> ().textComponent.text);

	}

	void OnDestroy(){
		Messenger<JsonData>.RemoveListener (GameEvent.UPDATE_NICK_NAME, OnEndChangeNameInput);
	}
}
