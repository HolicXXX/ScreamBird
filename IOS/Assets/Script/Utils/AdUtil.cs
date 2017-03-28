using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdUtil : MonoBehaviour
{
	public static string VIDEO = "video"; //非静音，5秒可跳过
	public static string REWARDED_VIDEO = "rewardedVideo"; //非静音，不可跳过
	public static string SKIP_VIDEO = "skipVideo"; //静音，5秒可跳过

	public static AdUtil _Instance;

	void Awake(){
//		if (Advertisement.isSupported) {
//			#if UNITY_ANDROID
//			Advertisement.Initialize ("1337220", false);
//			#elif UNITY_IPHONE  
//			Advertisement.Initialize ("1337221", false);
//			#endif
//		} else {  
//			Debug.LogWarning ("Unity Ads is not supported on the current runtime platform.");  
//		}
	}
//	public void ShowRewardedAd()
//	{
//		if (Advertisement.IsReady("rewardedVideo"))
//		{
//			var options = new ShowOptions { resultCallback = HandleShowResult };
//			Advertisement.Show("rewardedVideo", options);
//		}
//	}
//
//	private void HandleShowResult(ShowResult result)
//	{
//		switch (result)
//		{
//		case ShowResult.Finished:
//			Debug.Log("The ad was successfully shown.");
//			//
//			// YOUR CODE TO REWARD THE GAMER
//			// Give coins etc.
//			break;
//		case ShowResult.Skipped:
//			Debug.Log("The ad was skipped before reaching the end.");
//			break;
//		case ShowResult.Failed:
//			Debug.LogError("The ad failed to be shown.");
//			break;
//		}
//	}
}