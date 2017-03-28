using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class SliderChange : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DataManager._volumNum = PlayerPrefs.GetFloat ("VolumNumber");//LoadJson.LoadSavedUserInfo () ["VolumNumber"].ToString();
		gameObject.GetComponent<Slider> ().value = DataManager._volumNum;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnSliderChanged(){
		DataManager._volumNum = gameObject.GetComponent<Slider> ().value;
	}

	void OnDestroy() {
		DataManager._volumNum = gameObject.GetComponent<Slider> ().value;
//		JsonData jd = LoadJson.LoadSavedUserInfo ();
//		jd ["VolumNumber"] = new JsonData (DataManager._volumNum.ToString());
//		LoadJson.SaveUserInfo (jd);
		PlayerPrefs.SetFloat ("VolumNumber", DataManager._volumNum);
		PlayerPrefs.Save ();
	}
}
