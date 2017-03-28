using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {

	public int ButtonNum;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void goToGameScene()
	{
		GetComponent<AudioSource> ().Play ();
		DataManager._isOver = false;
		DataManager._timeCount = 0.0f;
		DataManager._currentStage = ButtonNum;
		DataManager._volumNum = PlayerPrefs.GetFloat ("VolumNumber");//LoadJson.LoadSavedUserInfo () ["VolumNumber"].ToString();
		DataManager._didRec = true;
		DataManager._isRec = true;
		DataManager._waitForRebirth = false;
		DataManager._isUsedRebirth = false;
		SceneManager.LoadScene ("GameScene_1");

	}
}
