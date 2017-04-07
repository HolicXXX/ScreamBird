using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitUI : MonoBehaviour {

	private int _buttonNum;

	public Image _playerImage;

	public Text[] _texts;

	public GameObject _startButtonPrefab;

	public GameObject _startButtonOrigin;

	public GameObject _scrollUp;

	public GameObject _scollDown;

	public List<StartButton> _startButtons = new List<StartButton>();

	public Dropdown _rankList;

	void Awake(){
	}
	// Use this for initialization
	void Start () {
		initButtons ();
		SetLanguageText ();
		OnChangeSkinCallBack (PlayerPrefs.GetInt("CurrentSkin"));//
		Messenger<int>.AddListener (GameEvent.CHANGE_SKIN, OnChangeSkinCallBack);
		Messenger.AddListener (GameEvent.STAGE_INFO, initButtons);
	}

	void initButtons(){
		Debug.Log ("initUI");
		_buttonNum = PlayerPrefs.GetInt("PassedStageNum") + 1;

		if (_buttonNum > DataManager._stageMaxNum) {
			_buttonNum = DataManager._stageMaxNum;
		}

		Debug.Log ("ButtonNum: " + _buttonNum);

		for (int i = 0; i < _buttonNum; ++i) {
			if (_startButtons.Count > i) {
				var b = _startButtons [i];
				b.ButtonNum = i + 1;
				b.SetShowText ();
			} else {
				var b = GameObject.Instantiate (_startButtonPrefab);
				b.GetComponent<RectTransform> ().SetParent (_startButtonOrigin.GetComponent<RectTransform> (), false);
				StartButton sb = b.GetComponent<StartButton> ();
				sb.ButtonNum = i + 1;
				sb.SetShowText ();
				_startButtons.Add (sb);
			}
		}
		Debug.Log ("Buttons: " + _startButtons.Count);
		Debug.Log ("MaxMNum: " + DataManager._stageMaxNum);
		//TODO
		if (_startButtons.Count == DataManager._stageMaxNum) {
			Debug.Log ("add Coming soon button");
			var b = GameObject.Instantiate (_startButtonPrefab);
			b.GetComponent<RectTransform> ().SetParent (_startButtonOrigin.GetComponent<RectTransform> (), false);
			StartButton sb = b.GetComponent<StartButton> ();
			sb.ButtonNum = -1;
			sb.SetShowText ();
			_startButtons.Add (sb);
		}
		Debug.Log ("Buttons: " + _startButtons.Count);

		StartCoroutine (SetScrollAfterLayout ());
	}

	IEnumerator SetScrollAfterLayout(){
//		yield return new WaitForEndOfFrame ();
		yield return null;
		SetScrollEnable ();
	}

	void SetLanguageText(){
		foreach (Text t in _texts) {
			string name = t.name;
			string _t = LoadJson.GetLanguageText (name);
			t.text = _t;
		}
	}

	void OnChangeSkinCallBack(int num){
		//PlayerPrefs.GetInt("CurrentSkin")
		var sprites = Resources.LoadAll<Sprite> (string.Format ("Skin_{0}/", num));
		string currentName = _playerImage.sprite.name;
		var newSprite = Array.Find (sprites, item => item.name == currentName);
		if (newSprite) {
			_playerImage.sprite = newSprite;
		}
	}

	void SetScrollEnable(){
		if (_startButtons.Count == 0) {
			return;
		}
		if (_startButtons [0].transform.localPosition.y + _startButtonOrigin.transform.localPosition.y > 200) {
			_scrollUp.SetActive (true);
		} else {
			_scrollUp.SetActive (false);
		}
//		Debug.Log (_startButtons.Count);
//		Debug.Log (_startButtons [_startButtons.Count - 1].transform.localPosition.y + _startButtonOrigin.transform.localPosition.y);
		if (_startButtons[_startButtons.Count - 1].transform.localPosition.y + _startButtonOrigin.transform.localPosition.y > -200) {
			_scollDown.SetActive (false);
		} else {
			_scollDown.SetActive (true);
		}
	}

	public void OnScrollUpClicked(){
		_startButtonOrigin.transform.position -= new Vector3 (0, 139,0);
		//SetScrollEnable ();
	}

	public void OnScrollDownClicked(){
		_startButtonOrigin.transform.position += new Vector3 (0, 139,0);
		//SetScrollEnable ();
	}

	public void OnScrollValueChanged(Vector2 vec){
		//Debug.Log (vec);
		var trans = _startButtonOrigin.GetComponent<RectTransform> ();
		if (vec.y > 1.0f) {
			//_startButtonOrigin.transform.parent.GetComponent<ScrollRect>()
			trans.localPosition = new Vector3(0,trans.parent.GetComponent<RectTransform> ().sizeDelta.y / 2,0);
		}
		if (vec.y < 0f) {
			trans.localPosition = new Vector3 (0, trans.sizeDelta.y - trans.parent.GetComponent<RectTransform> ().sizeDelta.y / 2, 0);
		}
		//Debug.Log (trans.localPosition);
		SetScrollEnable ();
	}

	void OnDestroy(){
		Messenger<int>.RemoveListener (GameEvent.CHANGE_SKIN, OnChangeSkinCallBack);
		Messenger.RemoveListener (GameEvent.STAGE_INFO, initButtons);
	}
}
