using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankTitle : MonoBehaviour {

	public RankList _rankList;

	// Use this for initialization
	void Start () {
		addStageOptions ();
		Messenger.AddListener (GameEvent.STAGE_INFO, addStageOptions);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void addStageOptions(){
		Dropdown dp = GetComponent<Dropdown> ();
		List<Dropdown.OptionData> oplist = new List<Dropdown.OptionData> ();
		for (int i = dp.options.Count;i < DataManager._stageMaxNum; ++i) {
			Dropdown.OptionData data = new Dropdown.OptionData ();
			string itemtext = DataManager._languageIndex == 1 ? string.Format ("第{0}关", i + 1) : string.Format ("Stage{0}", i + 1);
			data.text = itemtext;
			oplist.Add (data);
		}
		dp.AddOptions (oplist);
	}

	void OnDestroy(){
		Messenger.RemoveListener (GameEvent.STAGE_INFO, addStageOptions);
	}

	public void OnTitleValueChanged(){
		Debug.Log ("Title changed: " + GetComponent<Dropdown> ().value);
		_rankList.showRankList (GetComponent<Dropdown> ().value + 1);
	}
}
