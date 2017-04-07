using UnityEngine;
using System.Collections;
using System;
using LitJson;

public class NetworkService {

	private const string StageInfoUrl = "https://api.shaojishiduo.com/cfg/scream/stage.json";

	private const string MsgUrl = "https://api.shaojishiduo.com/GameScream/";

	public static string MsgGetUserInfo = "getUserInfo";
	public static string MsgUpdateScore = "updateScore";
	public static string MsgGetRankList = "getRankList";
	public static string MsgSetNickame = "setNickame";
	public static string MsgConsumeMoney = "consumeMoney";
	public static string MsgActivateCode = "activateCode";
	public static string MsgBuySkin = "buySkin";
	public static string MsgShopList = "shopList";

	private string deviceID = "";

	private string getDeviceID(){
		if (deviceID == "") {
			deviceID = StringUtil.Base64Encode(DeviceID.Get());
		}
		Debug.Log("deviceId:" + deviceID);
		return deviceID;
	}

	private bool IsResponseValid(WWW www) {
		if (www.error != null) {
			Debug.Log("bad connection");
			return false;
		}
		else if (string.IsNullOrEmpty(www.text)) {
			Debug.Log("bad data");
			return false;
		}
		else {	// all good
			return true;
		}
	}

	public IEnumerator ReqMsg(string msgName, Hashtable args, Action<JsonData> callback) {
		WWWForm form = new WWWForm();
		if (msgName != NetworkService.MsgShopList) {
			form.AddField ("skey", getDeviceID());
			form.AddField ("timestamp", DateTime.UtcNow.Ticks.ToString());
			if (args != null) {
				foreach(DictionaryEntry arg in args) {
					form.AddField(arg.Key.ToString(), arg.Value.ToString());
				}
			}
		}
		WWW www = new WWW(MsgUrl + msgName, form);

		yield return www;
		
		if (!IsResponseValid(www))
			yield break;
		
		Debug.Log ("WWW: " + www.text);
		JsonData jsdArray = JsonMapper.ToObject(www.text);
		callback (jsdArray);
	}

	public IEnumerator ReqStageInfo(Action<JsonData> callback){
		WWW www = new WWW(StageInfoUrl);
		yield return www;

		if (!IsResponseValid(www))
			yield break;

		Debug.Log ("WWW: " + www.text);
		JsonData jsdArray = JsonMapper.ToObject(www.text);
		callback (jsdArray);
	}

}
