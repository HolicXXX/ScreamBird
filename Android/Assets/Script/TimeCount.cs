using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeCount : MonoBehaviour {

	public float _timeCount;
	// Use this for initialization
	void Start () {
		_timeCount = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (DataManager._isPause || DataManager._isFailed || DataManager._isOver || DataManager._waitForRebirth)
			return;

		if (!DataManager._isOver) {
			_timeCount += Time.deltaTime;
			GetComponent<Text> ().text = stringCut(_timeCount.ToString()) + "s";
		}
	}

	public static string stringCut(string str){
		//Debug.Log (str);
		int pointIndex = str.IndexOf ('.') + 4;
		//Debug.Log (pointIndex);
		if (pointIndex > str.Length) {
			pointIndex = str.Length;
		}
		string ret = new string (str.ToCharArray(),0,pointIndex);
		return ret;
	}

}
