using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IPHONE
using com.mob;
#endif

public class WinScript : MonoBehaviour {

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
//			//			var list = gameObject.GetComponentsInChildren<ExtraButtonScript> ();
//			//			for (int i = 0; i < list.Length; ++i) {
//			//				if (list [i].gameObject.GetComponent<ExtraButtonScript> ()._extraButtonType == ExtraButtonType.EBT_LIKE)
//			//				   //list [i].gameObject.GetComponent<ExtraButtonScript> ()._extraButtonType == ExtraButtonType.EBT_SHARE)
//			//					list [i].gameObject.SetActive (false);
//			//			}
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
		StartCoroutine (waitForEndRec());
	} 

	IEnumerator waitForEndRec(){
		yield return new WaitForSeconds (0.5f);
		if (DataManager._isRec) {
			#if UNITY_IPHONE
			ShareREC.stopRecording (new FinishedRecordEvent ((Exception ex) => {
			}));
			#endif
			DataManager._isRec = false;
		}
	}
}
