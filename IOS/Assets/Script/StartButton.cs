using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {

	public int ButtonNum;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SetShowText(){
		if (ButtonNum == -1) {
			GetComponentInChildren<Text> ().text = DataManager._languageIndex == 1 ? "敬请期待" : "Coming soon";
			GetComponent<Button> ().interactable = false;
			return;
		}
		GetComponentInChildren<Text> ().text = DataManager._languageIndex == 1 ? string.Format ("第{0}关", ButtonNum) : string.Format ("Stage{0}", ButtonNum);
	}

	public void goToGameScene()
	{
		GetComponent<AudioSource> ().Play ();

		if (DataManager._stageInfo == null && !LoadJson.IsStageInfoFileExists()) {
			Camera.main.gameObject.GetComponent<StartSceneCamere> ()._loadDataMask.gameObject.SetActive (true);
			return;
		}

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
