using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RebirthMask : MonoBehaviour {

	public Text _countDown;

	public float _countTime;

	public Text _moneyNum;

	public RawImage _lessMoneyTip;

	// Use this for initialization
	void Start () {
		_moneyNum.text = DataManager._languageIndex == 1 ? string.Format ("（拥有金币：{0}）", PlayerPrefs.GetInt ("Money")) : string.Format (" You have: {0} ", PlayerPrefs.GetInt ("Money"));

	}
	
	// Update is called once per frame
	void Update () {
		if (DataManager._waitOperate)
			return;
		_countTime -= Time.deltaTime;
		int num = Mathf.CeilToInt (_countTime);
		if (num < 0) {
			Debug.Log ("Don't Rebirth");
			DataManager._waitForRebirth = false;
			DataManager._isFailed = true;
			gameObject.SetActive (false);
			return;
		}
		_countDown.text = num.ToString();
	}

	void OnEnable() {
		_countDown.text = Mathf.CeilToInt (_countTime).ToString();
	}

	public void setLessMoneyTipMask(bool isShow){
		if (isShow) {
			DataManager._waitOperate = true;
			_lessMoneyTip.gameObject.SetActive (true);
		} else {
			DataManager._waitOperate = false;
			_lessMoneyTip.gameObject.SetActive (false);
		}
	}

}
