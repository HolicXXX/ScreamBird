using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using cn.sharesdk.unity3d;
using com.mob;

public enum ExtraButtonType{
	EBT_RANK,
	EBT_MUSIC,
	EBT_LIKE,
	EBT_SHARE,
	EBT_HELP,
	EBT_WX,
	EBT_GAMESHARE,
	EBT_SETTING,
	EBT_REBIRTH,
	EBT_APPSCORE,
	EBT_MONEYINFO,
	EBT_SKIN
}

public class ExtraButtonScript : MonoBehaviour {

	public ExtraButtonType _extraButtonType;

	public RawImage _mask;

	public Toggle _music;
	public Toggle _sound;
	public ShareSDK ssdk;

	//public Sprite _musicOn;
	//public Sprite _musicOff;

	// Use this for initialization
	void Start () {
//		if (_extraButtonType == ExtraButtonType.EBT_MUSIC) {
//			int music = (int)(LoadJson.LoadSavedUserInfo () ["Music"]);//PlayerPrefs.GetInt ("Music",0);
//			if (music == 1) {
//				GetComponent<Image> ().sprite = _musicOn;
//			} else {
//				GetComponent<Image> ().sprite = _musicOff;
//			}
//		}
		if(_extraButtonType == ExtraButtonType.EBT_WX){
			if (DataManager._languageIndex == 2) {
				gameObject.SetActive (false);
			}
		}

		if(_extraButtonType == ExtraButtonType.EBT_REBIRTH){
			Messenger<JsonData>.AddListener (GameEvent.COIN_UPDATED, OnConsumeMoneyCallBack);
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnDestroy(){
		if(_extraButtonType == ExtraButtonType.EBT_REBIRTH){
			Messenger<JsonData>.RemoveListener (GameEvent.COIN_UPDATED, OnConsumeMoneyCallBack);
		}
	}

	void OnConsumeMoneyCallBack(JsonData jd){
		if ((int)(jd ["error"]) == 0) {
			DataManager._waitForRebirth = false;
			PlayerPrefs.SetInt ("Money", int.Parse (jd ["data"] ["money"].ToString ()));
			//reset player
		} else {
			Debug.Log ("Server not enough money");
			PlayerPrefs.SetInt ("Money", int.Parse (jd ["data"] ["money"].ToString ()));
		}
	}

	public void OnClickCallBack(){
		GetComponent<AudioSource> ().Play ();
		switch (_extraButtonType) {
		case ExtraButtonType.EBT_HELP:
			{
				Debug.Log ("call help layer");
				_mask.gameObject.SetActive (true);
			}
			break;
		case ExtraButtonType.EBT_LIKE:
			{
				Debug.Log ("call like api");
				Hashtable userData = new Hashtable();
				//ShareREC.editLastingRecording ("我天籁般的的叫声，是时候让你们听到了！", userData, null);
				string str = DataManager._languageIndex == 1?"我天籁般的的叫声，是时候让你们听到了！":"My scream is like the sounds of nature. It's time to show you.";
				ShareREC.openSocial (str, userData, SocialPageType.Share, null);
			}
			break;
		case ExtraButtonType.EBT_MUSIC:
			{
				Debug.Log ("call music on/off");
//				JsonData jd = LoadJson.LoadSavedUserInfo ();
//				int music = (int)(jd["Music"]);
//				if (music == 1) {
//					//PlayerPrefs.SetInt ("Music", 0);
//					jd["Music"] = new JsonData(0);
//					GetComponent<Image> ().sprite = _musicOff;
//				} else {
//					//PlayerPrefs.SetInt ("Music", 1);
//					jd["Music"] = new JsonData(1);
//					GetComponent<Image> ().sprite = _musicOn;
//				}
//				//PlayerPrefs.Save ();
//				LoadJson.SaveUserInfo(jd);
			}
			break;
		case ExtraButtonType.EBT_RANK:
			{
				Debug.Log ("call rank layer");
				_mask.gameObject.SetActive (true);
			}
			break;
		case ExtraButtonType.EBT_SHARE:
			{
				Debug.Log ("call share api");
				Hashtable userData = new Hashtable();
				string str = DataManager._languageIndex == 1?"我天籁般的的叫声，是时候让你们听到了！":"My scream is like the sounds of nature. It's time to show you.";
				ShareREC.openSocial (str, userData, SocialPageType.Share, null);
			}
			break;
		case ExtraButtonType.EBT_WX:
			{
				Debug.Log ("call weixin api");
				_mask.gameObject.SetActive (true);

			}
			break;
		case ExtraButtonType.EBT_GAMESHARE:
			{
				Debug.Log ("call url share api");
				string shareTitle = "这游戏有毒，我已中毒至深，尖叫根本停不下来！";
				string shareDesc = "与全世界一起尖叫！谁才是声控之王？是你吗？";

				string shareTitleEn = "This game is poisonous and i have been poisoned deeply! I can't stop playing and screaming!";
				string shareDescEn = "Scream with people around the world! Who is the king of voice control? Is that you?";

				string shareSite = "尖叫鸟";
				string shareUrl = "https://api.shaojishiduo.com/GameScream/share?s=" + PlayerPrefs.GetString("ShareID");
				string shareImg = "http://liteapp-1252384896.costj.myqcloud.com/scream/images/bird_icon_256.png";

				ShareContent content = new ShareContent();
				content.SetText(shareDesc);
				content.SetImageUrl(shareImg);
				content.SetTitle(shareTitle);
				content.SetTitleUrl(shareUrl);
				content.SetSite(shareSite);
				content.SetSiteUrl(shareUrl);
				content.SetUrl(shareUrl);
				content.SetComment(shareDesc);
				content.SetShareType(ContentType.Webpage);

				//不同平台分享不同内容
				//				ShareContent customizeShareParams = new ShareContent();
				//				customizeShareParams.SetText(shareDesc);
				//				customizeShareParams.SetImageUrl(shareImg);
				//				customizeShareParams.SetShareType(ContentType.Webpage);
				//				customizeShareParams.SetObjectID("SinaID");
				//				content.SetShareContentCustomize(PlatformType.SinaWeibo, customizeShareParams);

				//Instagram
				//				ShareContent sInstagram = new ShareContent();
				//				sInstagram.SetText(shareDesc);
				//				sInstagram.SetImageUrl(shareImg);
				//				sInstagram.SetShareType(ContentType.Image);
				//				content.SetShareContentCustomize(PlatformType.Instagram, sInstagram);

				//fbfb
				ShareContent sFacebook = new ShareContent();
				sFacebook.SetText(shareTitleEn + " Download: " + shareUrl);
				sFacebook.SetTitle(shareDescEn);
				sFacebook.SetImageUrl(shareImg);
				sFacebook.SetShareType(ContentType.Webpage);
				content.SetShareContentCustomize(PlatformType.Facebook, sFacebook);

				//Twitter
				ShareContent sTwitter = new ShareContent();
				sTwitter.SetText(shareDescEn + " Download: " + shareUrl);
				sTwitter.SetImageUrl(shareImg);
				sTwitter.SetUrl(shareUrl);
				sTwitter.SetShareType(ContentType.Auto);
				content.SetShareContentCustomize(PlatformType.Twitter, sTwitter);

				PlatformType[] pfs = {PlatformType.WeChat, PlatformType.WeChatMoments, 
					PlatformType.Facebook, 
					PlatformType.QQ, PlatformType.QZone, PlatformType.Twitter
				};

				//通过分享菜单分享
				ssdk.ShowPlatformList (pfs, content, 100, 100);
			}
			break;
		case ExtraButtonType.EBT_MONEYINFO:
			{
				Debug.Log ("Money Info Mask");
				_mask.gameObject.SetActive (true);
			}
			break;
		case ExtraButtonType.EBT_SETTING:
			{
				Debug.Log ("Setting Mask");
				_mask.gameObject.SetActive (true);
			}
			break;
		case ExtraButtonType.EBT_REBIRTH:
			{
				Debug.Log ("Rebirth");
				if (PlayerPrefs.GetInt ("Money") > 0) {
//					Dictionary<string,string> dic = new Dictionary<string,string> ();
//					dic.Add ("amount", "1");
//					JsonData jd = NetUtil.RequestMsg (NetUtil.MsgConsumeMoney, dic);
					ssdk.gameObject.GetComponent<PlayerManager>().ConsumeMoney(1);
					ssdk.gameObject.GetComponent<CameraMove> ().RebirthPlayer ();
					DataManager._waitForRebirth = false;
					DataManager._isUsedRebirth = true;
					_mask.gameObject.SetActive (false);
				} else {
					Debug.Log ("playerprefs not enough money");
					Debug.Log ("Show Get money mask");
//					DataManager._waitOperate = true;
//					transform.parent.gameObject.GetComponent<RebirthMask> ()._lessMoneyTip.gameObject.SetActive (true);
					gameObject.GetComponentInParent<RebirthMask>().setLessMoneyTipMask(true);
				}
			}
			break;
		case ExtraButtonType.EBT_APPSCORE:
			{
				Debug.Log ("Link to AppStore");
				#if UNITY_IPHONE || UNITY_EDITOR
				//Debug.Log ("aaaaaaaaaaa");
				const string APP_ID = "1211127073";//1211127073
				var url = string.Format(
					"https://itunes.apple.com/app/viewContentsUserReviews/id{0}",
					APP_ID);//itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=
				Application.OpenURL(url);
				#endif
			}
			break;
		case ExtraButtonType.EBT_SKIN:
			{
				_mask.gameObject.SetActive (true);
			}
			break;
		}
	}

	public void OnMusicValueChanged(bool value){
//		JsonData jd = LoadJson.LoadSavedUserInfo ();
		Debug.Log (value);
		PlayerPrefs.SetInt ("Music", (_music.isOn ? 1 : 0));
//		jd["Music"] = new JsonData(_music.isOn?1:0);
//		LoadJson.SaveUserInfo(jd);
	}

	public void OnSoundValueChanged(bool value){
//		JsonData jd = LoadJson.LoadSavedUserInfo ();
		Debug.Log (value);
		PlayerPrefs.SetInt ("Sound", (_sound.isOn ? 1 : 0));
//		jd["Sound"] = new JsonData(_music.isOn?1:0);
//		LoadJson.SaveUserInfo(jd);
	}

}
