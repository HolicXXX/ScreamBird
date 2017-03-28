using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

public enum SettingToggleType{
	STT_MUSIC,
	STT_SOUND
}

public class SettingScript : MonoBehaviour {

	public SettingToggleType _type;

	// Use this for initialization
	void Start () {
		switch (_type) {
		case SettingToggleType.STT_MUSIC:
			{
				GetComponent<Toggle> ().isOn = PlayerPrefs.GetInt("Music") == 1;
			}
			break;
		case SettingToggleType.STT_SOUND:
			{
				GetComponent<Toggle> ().isOn = PlayerPrefs.GetInt("Sound") == 1;
			}
			break;
		}
	}

	void OnEnable(){
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnDestroy(){
		Debug.Log ("setting button on destroy");
	}
}
