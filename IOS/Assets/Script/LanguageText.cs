using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageText : MonoBehaviour {

	public Text[] _texts;

	// Use this for initialization
	void Start () {
		SetLanguageText ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void SetLanguageText(){
		foreach (Text t in _texts) {
			string name = t.name;
			string _t = LoadJson.GetLanguageText (name);
			t.text = _t;
		}
	}
}
