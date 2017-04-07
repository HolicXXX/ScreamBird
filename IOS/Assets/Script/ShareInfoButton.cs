using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cn.sharesdk.unity3d;

public enum ShareInfoButtonType{
	SIB_COPYLINK,
	SIB_SHARELINK
}

public class ShareInfoButton : MonoBehaviour {

	public ShareInfoButtonType _type;

	public ShareSDK ssdk;

	public GameObject _copyTipPrefab;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnClickCallBack(){
		string shareUrl = "https://api.shaojishiduo.com/GameScream/share?s=" + PlayerPrefs.GetString("ShareID");
		switch (_type) {
		case ShareInfoButtonType.SIB_COPYLINK:
			{
				GameObject canvas = GameObject.Find ("Canvas");
				GameObject tip = GameObject.Instantiate (_copyTipPrefab, new Vector3 (Screen.width / 2, Screen.height / 2, 0.0f), Quaternion.identity);
				tip.transform.SetParent (canvas.transform);
				tip.GetComponentInChildren<Text> ().text = LoadJson.GetLanguageText ("CopyTips");
				//tip.transform.position = new Vector3 (0, 0, 0);
				ClipboardManager.CopyToClipboard (LoadJson.GetLanguageText ("CopyLinkText") + shareUrl);
			}
			break;
		case ShareInfoButtonType.SIB_SHARELINK:
			{
				string shareTitle = "这游戏有毒，我已中毒至深，尖叫根本停不下来！";
				string shareDesc = "与全世界一起尖叫！谁才是声控之王？是你吗？";

				string shareTitleEn = "This game is poisonous and i have been poisoned deeply! I can't stop playing and screaming!";
				string shareDescEn = "Scream with people around the world! Who is the king of voice control? Is that you?";

				string shareSite = "尖叫鸟";
				//string shareUrl = "https://api.shaojishiduo.com/GameScream/share?s=" + PlayerPrefs.GetString("ShareID");
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
				sFacebook.SetText(shareTitleEn + " Download：" + shareUrl);
				sFacebook.SetTitle(shareDescEn);
				sFacebook.SetImageUrl(shareImg);
				sFacebook.SetShareType(ContentType.Webpage);
				content.SetShareContentCustomize(PlatformType.Facebook, sFacebook);

				//Twitter
				ShareContent sTwitter = new ShareContent();
				sTwitter.SetText(shareDescEn + " Download：" + shareUrl);
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
		}
	}

}
