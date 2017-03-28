using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.mob;
using UnityEngine.Advertisements;

public class FailedScript : MonoBehaviour {

	public GameObject _recButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable() {
		if(PlayerPrefs.GetInt("Sound") == 1)
			GetComponent<AudioSource> ().Play ();
//		if (!DataManager._didRec) {
//			_recButton.gameObject.SetActive (false);
////			var list = gameObject.GetComponentsInChildren<ExtraButtonScript> ();
////			for (int i = 0; i < list.Length; ++i) {
////				if (list [i].gameObject.GetComponent<ExtraButtonScript> ()._extraButtonType == ExtraButtonType.EBT_LIKE)
////				   //list [i].gameObject.GetComponent<ExtraButtonScript> ()._extraButtonType == ExtraButtonType.EBT_SHARE)
////					list [i].gameObject.SetActive (false);
////			}
//			Vector2[] buttonPos = new Vector2[3]{new Vector2(-150,-300),new Vector2(0,-300),new Vector2(150,-300)};
//			var list = gameObject.GetComponentsInChildren<ExtraButtonScript> ();
//			for (int i = 0; i < list.Length; ++i) {
//				switch (list [i].gameObject.GetComponent<ExtraButtonScript> ()._extraButtonType) {
//				case ExtraButtonType.EBT_RANK:
//					{
//						list [i].gameObject.GetComponent<RectTransform> ().localPosition = buttonPos [0];
//					}
//					break;
//				case ExtraButtonType.EBT_APPSCORE:
//					{
//						list [i].gameObject.GetComponent<RectTransform> ().localPosition = buttonPos [1];
//					}
//					break;
//				case ExtraButtonType.EBT_GAMESHARE:
//					{
//						list [i].gameObject.GetComponent<RectTransform> ().localPosition = buttonPos [2];
//					}
//					break;
//				}
//			}
//		}else{
//			_recButton.gameObject.SetActive (true);
//			Vector2[] buttonPos = new Vector2[4]{new Vector2(-180,-300),new Vector2(-60,-300),new Vector2(60,-300),new Vector2(180,-300)};
//			var list = gameObject.GetComponentsInChildren<ExtraButtonScript> ();
//			for (int i = 0; i < list.Length; ++i) {
//				switch (list [i].gameObject.GetComponent<ExtraButtonScript> ()._extraButtonType) {
//				case ExtraButtonType.EBT_RANK:
//					{
//						list [i].gameObject.GetComponent<RectTransform> ().localPosition = buttonPos [0];
//					}
//					break;
//				case ExtraButtonType.EBT_APPSCORE:
//					{
//						list [i].gameObject.GetComponent<RectTransform> ().localPosition = buttonPos [1];
//					}
//					break;
//				case ExtraButtonType.EBT_GAMESHARE:
//					{
//						list [i].gameObject.GetComponent<RectTransform> ().localPosition = buttonPos [2];
//					}
//					break;
//				case ExtraButtonType.EBT_LIKE:
//					{
//						list [i].gameObject.GetComponent<RectTransform> ().localPosition = buttonPos [3];
//					}
//					break;
//				}
//			}
//		}
		++DataManager._deathCount;
		StartCoroutine (waitForEndRec());
		//StartCoroutine (ShowAdWhenReady());
		ShowRewardedAd();
	} 

	IEnumerator waitForEndRec(){
		yield return new WaitForSeconds (0.5f);
		if (DataManager._isRec) {
			ShareREC.stopRecording (new FinishedRecordEvent ((Exception ex) => {
			}));
			DataManager._isRec = false;
		};
	}


	//Ads
	IEnumerator ShowAdWhenReady()
	{
		Debug.Log ("show ad when ready");
		while (!Advertisement.IsReady(AdUtil.SKIP_VIDEO))
			yield return null;
		ShowRewardedAd ();
	}

	public void ShowRewardedAd()
	{
		Debug.Log ("Is Ads ready:  " + Advertisement.IsReady (AdUtil.SKIP_VIDEO));
		Debug.Log ("DeathCount: " + DataManager._deathCount);
		if (DataManager._deathCount < PlayerPrefs.GetInt ("AdsCount")) {
			return;
		}
		if (Advertisement.IsReady (AdUtil.SKIP_VIDEO)) {
			DataManager._deathCount = 0;
			Debug.Log ("show ad");
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show(AdUtil.SKIP_VIDEO, options);
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");
			//
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}

}
