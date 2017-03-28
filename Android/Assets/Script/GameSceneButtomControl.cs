using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_IPHONE
using com.mob;
#endif

public enum GameSceneButtonType{
	B_HOME,
	B_PAUSE,
	B_RESUME,
	B_RESET,
	B_NEXT,
	B_REC
}

public class GameSceneButtomControl : MonoBehaviour {

	public GameSceneButtonType _bType;

	public PauseControl _Player;

	public Image _pauseImage;

	public GameSceneButtomControl _resumeButton;

	public RawImage _mask;

	public Sprite _recOff;
	public Sprite _recOn;

	void Start () {
		if (_bType == GameSceneButtonType.B_REC) {
			#if UNITY_IPHONE
			if (DataManager._isRec) {
				Debug.Log ("Start Record");
				GetComponent<Image> ().sprite = _recOn;
				//
				ShareREC.setSyncAudioComment(true);
				ShareREC.startRecoring();
			} else {
				Debug.Log ("End Record");
				GetComponent<Image> ().sprite = _recOff;
				//
				FinishedRecordEvent evt = new FinishedRecordEvent((Exception ex)=>{});
				ShareREC.stopRecording(evt);
			}
			#endif
		}
	}

	void callPauseMask(bool ispause){
		if (ispause) {
			if (_resumeButton != null)
				_resumeButton.gameObject.SetActive (ispause);
			if (_mask != null)
				_mask.gameObject.SetActive (ispause);
			if (_pauseImage != null)
				_pauseImage.gameObject.SetActive (ispause);
		} else {
			if (_bType == GameSceneButtonType.B_RESUME) {
				gameObject.SetActive (ispause);
				_mask.gameObject.SetActive (ispause);
				_pauseImage.gameObject.SetActive (ispause);
			}
		}

	}

	public void clickCallBack(){
		GetComponent<AudioSource> ().Play ();
		switch (_bType) {
		case GameSceneButtonType.B_HOME:
			{
				if (DataManager._isRec) {
				#if UNITY_IPHONE
				ShareREC.stopRecording (new FinishedRecordEvent ((Exception ex) => {
				}));
				#endif
				DataManager._isRec = false;
				}
				SceneManager.LoadScene ("StartScene");
			}
			break;
		case GameSceneButtonType.B_PAUSE:
			{
				Debug.Log ("Pause");
				Debug.Log ("Call Mask and resume Button");

				DataManager._isPause = true;
				_Player.OnPauseClicked ();
//				for (int i = 0; i < _pauseList.Count; ++i) {
//					_pauseList[i].OnPauseClicked ();
//				}
				callPauseMask(true);
			}
			break;
		case GameSceneButtonType.B_RESUME:
			{
				Debug.Log ("Resume");
				DataManager._isPause = false;
				_Player.OnResumeClicked ();
//				for (int i = 0; i < _pauseList.Count; ++i) {
//					_pauseList[i].OnResumeClicked ();
				//				}
				callPauseMask(false);
			}
			break;
		case GameSceneButtonType.B_RESET:
			{
				if (DataManager._isRec) {
					#if UNITY_IPHONE
					ShareREC.stopRecording (new FinishedRecordEvent ((Exception ex) => {
					}));
					#endif
					DataManager._isRec = false;
				}
				SceneManager.LoadScene ("StartScene");
			}
			break;
		case GameSceneButtonType.B_NEXT:
			{
				Debug.Log ("Call next stage");
				if (DataManager._currentStage >= 3) {
					SceneManager.LoadScene ("StartScene");
				} else {
					DataManager._currentStage += 1;
					SceneManager.LoadScene ("GameScene_1");
				}
			}
			break;
		case GameSceneButtonType.B_REC:
			{
//				DataManager._isRec = !DataManager._isRec;
//				DataManager._didRec = true;
//				if (DataManager._isRec) {
//					Debug.Log ("Start Record");
//					GetComponent<Image> ().sprite = _recOn;
//					//
//					ShareREC.setSyncAudioComment(true);
//					ShareREC.startRecoring();
//				} else {
//					Debug.Log ("End Record");
//					GetComponent<Image> ().sprite = _recOff;
//					//
//					FinishedRecordEvent evt = new FinishedRecordEvent((Exception ex)=>{});
//					ShareREC.stopRecording(evt);
//				}
			}
			break;
		}
	}
}
