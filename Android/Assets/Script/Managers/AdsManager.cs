using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : Singleton<AdsManager> {

	// Use this for initialization
	void Start () {
		if (Advertisement.isSupported) {
			#if UNITY_ANDROID
			Advertisement.Initialize ("1337220", false);
			#elif UNITY_IPHONE  
			Advertisement.Initialize ("1337221", false);
			#endif
			Debug.Log ("Is Ads ready:  " + Advertisement.IsReady (AdUtil.SKIP_VIDEO));
		} else {  
			Debug.LogWarning ("Unity Ads is not supported on the current runtime platform.");  
		}

	}

	// Update is called once per frame
	void Update () {
		
	}
}
