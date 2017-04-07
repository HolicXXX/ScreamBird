using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using LitJson;

public enum SkinState{
	SS_BUY,
	SS_UNSELECTED,
	SS_SELECTED
}

public class SkinItemState : MonoBehaviour {

	public int _skinNum;

	public SkinState _skinState;

	private Toggle _toggle;
	private Text _text;
	private int _skinMoneyNum;

	public GameObject _tipPrefab;

	public RawImage _buyConfirmMask;

	public Text _buyTipInfo;

	// Use this for initialization
	void Start () {
		int s = PlayerPrefs.GetInt (string.Format ("SkinState_{0}", _skinNum));
		if (s == 1) {
			if (PlayerPrefs.GetInt ("CurrentSkin") == _skinNum) {
				_skinState = SkinState.SS_SELECTED;
			} else {
				_skinState = SkinState.SS_UNSELECTED;
			}
		} else {
			_skinState = SkinState.SS_BUY;
		}
		_skinMoneyNum = PlayerPrefs.GetInt (string.Format ("SkinPrice_{0}", _skinNum));
		_toggle = GetComponentInChildren<Toggle> ();
		_text = GetComponentInChildren<Text> ();
		_text.text = _skinMoneyNum.ToString ();
		SetByState (_skinState);
		Messenger<int>.AddListener (GameEvent.CHANGE_SKIN, OnChangeSkinCallBack);
		Messenger<JsonData>.AddListener (GameEvent.BUY_SKIN, OnBuySkinCallBack);
	}

	void OnDestroy(){
		Messenger<int>.RemoveListener (GameEvent.CHANGE_SKIN, OnChangeSkinCallBack);
		Messenger<JsonData>.RemoveListener (GameEvent.BUY_SKIN, OnBuySkinCallBack);
	}

	public void OnPointerClickCallBack(BaseEventData data){
		switch (_skinState) {
		case SkinState.SS_BUY:
			{
				if (PlayerPrefs.GetInt ("Money") >= int.Parse (_text.text)) {
					Debug.Log ("Buy Skin");

					_buyConfirmMask.gameObject.SetActive (true);
					string tiptext = DataManager._languageIndex == 1 ? string.Format ("确定消耗\n{0}金币\n购买皮肤？", _skinMoneyNum) : string.Format("Cost\n{0}coins\nto buy skin?",_skinMoneyNum);
					_buyTipInfo.text = tiptext;

					var buttons = _buyConfirmMask.GetComponentsInChildren<Button> ();
					buttons [0].onClick.RemoveAllListeners ();
					buttons [0].onClick.AddListener (BuyConfirm);
					buttons [1].onClick.RemoveAllListeners ();
					buttons [1].onClick.AddListener (BuyCancle);

				} else {
					Debug.Log ("Not Enough Money");

					GameObject canvas = GameObject.Find ("Canvas");
					GameObject tip = GameObject.Instantiate (_tipPrefab, canvas.transform);
					var rect = tip.transform as RectTransform;
					rect.anchoredPosition = new Vector2 (0, 0);
					rect.sizeDelta = new Vector2 (Screen.width, Screen.height);
					tip.GetComponentInChildren<Text> ().text = DataManager._languageIndex == 1 ? "金币不足！" : "Not Enough Money";
				}
			}
			break;
		case SkinState.SS_UNSELECTED:
			{
				_toggle.isOn = true;
				_skinState = SkinState.SS_SELECTED;
				PlayerPrefs.SetInt ("CurrentSkin", _skinNum);
				Messenger<int>.Broadcast (GameEvent.CHANGE_SKIN, _skinNum);
			}
			break;
		case SkinState.SS_SELECTED:
			break;
		}
	}

	void OnBuySkinCallBack(JsonData res){
		if (_buySkinNum != _skinNum) {
			return;
		}
		//if response error == 0
		string str = "";
		if ((int)(res ["error"]) == 0) {
			_text.gameObject.SetActive (false);
			_toggle.gameObject.SetActive (true);
			_toggle.isOn = true;
			_skinState = SkinState.SS_SELECTED;

			PlayerPrefs.SetInt ("Money", int.Parse (res ["data"] ["money"].ToString ()));
			Messenger.Broadcast (GameEvent.UPDATA_USER_INFO);

			PlayerPrefs.SetInt ("CurrentSkin", _skinNum);
			PlayerPrefs.SetInt (string.Format ("SkinState_{0}", _skinNum), 1);
			Messenger<int>.Broadcast (GameEvent.CHANGE_SKIN, _skinNum);
			str = "皮肤购买成功！";
		} else {
			Debug.Log ("Buy Skin Error");
			if ((int)(res ["error"]) == 4) {
				str = "金币不足!";
			} else if ((int)(res ["error"]) == 5) {
				str = "皮肤不存在!";
			}
		}
		GameObject canvas = GameObject.Find ("Canvas");
		GameObject tip = GameObject.Instantiate (_tipPrefab, canvas.transform);
		var rect = tip.transform as RectTransform;
		rect.anchoredPosition = new Vector2 (0, 0);
		rect.sizeDelta = new Vector2 (Screen.width, Screen.height);
		tip.GetComponentInChildren<Text> ().text = str;
	}

	void OnChangeSkinCallBack(int num){
		if (num == _skinNum) {
			return;
		}
		if (_toggle.gameObject.activeInHierarchy) {
			_toggle.isOn = false;
			_skinState = SkinState.SS_UNSELECTED;
		}
	}

	void SetByState(SkinState state){
		switch (state) {
		case SkinState.SS_BUY:
			{
				_toggle.gameObject.SetActive (false);
				_text.gameObject.SetActive (true);
				//set _text.text base on _skinNum
			}
			break;
		case SkinState.SS_UNSELECTED:
			{
				_toggle.gameObject.SetActive (true);
				_toggle.isOn = false;
				_text.gameObject.SetActive (false);
			}
			break;
		case SkinState.SS_SELECTED:
			{
				_toggle.gameObject.SetActive (true);
				_toggle.isOn = true;
				_text.gameObject.SetActive (false);
			}
			break;
		}
	}

	static int _buySkinNum = 0;

	public void BuyConfirm(){
		Debug.Log ("Buy" + _skinNum.ToString());
		Camera.main.GetComponent<PlayerManager> ().BuySkin (_skinNum);
		_buySkinNum = _skinNum;
		_buyConfirmMask.gameObject.SetActive (false);
	}

	public void BuyCancle(){
		_buyConfirmMask.gameObject.SetActive (false);
	}
}
