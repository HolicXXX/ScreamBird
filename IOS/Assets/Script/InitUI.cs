using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitUI : MonoBehaviour {

	private int _buttonNum;

	public Image _playerImage;

	// Use this for initialization
	void Start () {
		//PlayerPrefs.SetInt ("PassedStageNum", 0);
		_buttonNum = PlayerPrefs.GetInt("PassedStageNum") + 1;

		Debug.Log ("ButtonNum: " + _buttonNum);

		var buttonList = gameObject.GetComponentsInChildren<Button> ();
		int count = _buttonNum;
		for (int i = 0; i < buttonList.Length; ++i) {
			if(buttonList[i].gameObject.CompareTag("StartButton")){
				buttonList[i].gameObject.SetActive (count > 0);
				--count;
			}
		}
		OnChangeSkinCallBack (PlayerPrefs.GetInt("CurrentSkin"));//
		Messenger<int>.AddListener (GameEvent.CHANGE_SKIN, OnChangeSkinCallBack);
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

	void OnDestroy(){
		Messenger<int>.RemoveListener (GameEvent.CHANGE_SKIN, OnChangeSkinCallBack);
	}
}
