using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankTitle : MonoBehaviour {

	public RankList _rankList;

	// Use this for initialization
	void Start () {
		//might need init with data
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTitleValueChanged(){
		Debug.Log ("Title changed: " + GetComponent<Dropdown> ().value);
		_rankList.showRankList (GetComponent<Dropdown> ().value + 1);
	}
}
