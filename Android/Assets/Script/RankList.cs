using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class RankList : MonoBehaviour {

	JsonData _rankData;

	public GameObject _rankItemPrefab;

	public RankItemScript _userInfo;

	public Dropdown _title;

	public PlayerManager _manager;

	void Start () {
		Messenger<JsonData>.AddListener (GameEvent.UPDATE_RANK_LIST, UpdateRankList);
	}

	void OnEnable() {

		if (DataManager._needUpdateRank) {
			//UpdateRankList ();
			_manager.GetRankList();
			DataManager._needUpdateRank = false;
		} else {
			showRankList (1);
		}
		_title.value = 0;
	}

	void OnDestroy(){
		Messenger<JsonData>.RemoveListener (GameEvent.UPDATE_RANK_LIST, UpdateRankList);
	}

	public void UpdateRankList(JsonData jsdArray){
		_rankData = jsdArray;//NetUtil.RequestMsg (NetUtil.MsgGetRankList, new Dictionary<string,string> ());
		if ((int)(_rankData ["error"]) != 0) {
			Debug.Log ("rank request error: " + (int)(_rankData ["error"]));
			//string.Format ("{0}", 1);
		} else {
			Debug.Log ("rank request success");
			showRankList (1);
		}
	}

	public void showRankList(int stage){
		Debug.Log ("show rank list: " + stage);
		//JsonData jd = LoadJson.LoadSavedUserInfo ();
		var contentAdd = GetComponent<ScrollRect> ().content;
		contentAdd.GetComponent<GridLayoutGroup> ().cellSize = new Vector2 (Screen.width / 750.0f * 650.0f, Screen.height / 1334.0f * 100.0f);
		contentAdd.transform.DetachChildren ();
		Debug.Log (stage);
		var list = _rankData ["data"][string.Format("stage_{0}",stage)];
//		Debug.Log (_rankData ["data"] [string.Format ("stage_{0}", stage)].IsArray);
//		return;
		_userInfo._numindex.text = "未上榜";
		_userInfo._name.text = PlayerPrefs.GetString ("PlayerName");//jd ["PlayerName"].ToString ();
		string defaultScore = TimeCount.stringCut ((PlayerPrefs.GetFloat (string.Format ("Stage_{0}", stage)) / 1000.0f).ToString () + "s");//((int)(jd[string.Format ("Stage_{0}", stage)]) / 1000.0f).ToString())
		_userInfo._score.text = defaultScore.Length == 1 ? "0.000s" : defaultScore;
		for (int i = 1; i <= list.Count; ++i) {
			if (list [i - 1] [0].ToString () == _userInfo._name.text) {
				_userInfo._numindex.text = i.ToString ();
				_userInfo._score.text = TimeCount.stringCut(((int)(list [i - 1] [1]) / 1000.0f).ToString()) + "s";
			}
			var singleRankItem = Instantiate (_rankItemPrefab);
			var texts = singleRankItem.GetComponent<RankItemScript> ();
			texts._numindex.text = i.ToString ();
			texts._name.text = list [i - 1] [0].ToString();
			texts._score.text = TimeCount.stringCut(((int)(list [i - 1] [1]) / 1000.0f).ToString()) + "s";
//			texts._name.text = list [i - 1] ["name"].ToString();
//			texts._score.text = list [i - 1] ["score"].ToString();
			//singleRankItem.GetComponent<RectTransform>().sizeDelta = new Vector2 (Screen.width / 750.0f, Screen.height / 1334.0f);
			//Debug.Log (singleRankItem.GetComponent<RectTransform> ().sizeDelta.x + " , " + singleRankItem.GetComponent<RectTransform> ().sizeDelta.y);
			singleRankItem.transform.SetParent (contentAdd.transform);


		}
	}
}
