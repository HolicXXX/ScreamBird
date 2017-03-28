using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using LitJson;

public class PlayerManager : MonoBehaviour, IGameManager {
	public ManagerStatus status {get; private set;}

	public float cloudValue {get; private set;}

	private NetworkService _network;

	public void Startup(NetworkService service) {
//		Debug.Log("Weather manager starting...");
		_network = service;
		status = ManagerStatus.Initializing;
	}

	public void GetUserInfo() {
		StartCoroutine(_network.ReqMsg (NetworkService.MsgGetUserInfo, null, OnGetUserInfoResponse));
	}
	private void OnGetUserInfoResponse(JsonData res) {
		//DataManager.Instance.NickName = (string)res ["nickname"];
		Debug.Log("OnGetUserInfoResponse: " + res);
		//PlayerPrefs.SetString("PlayerName",(string)res ["nickname"]);
		//DataManager.NickName = (string)res ["nickname"];
		Messenger<JsonData>.Broadcast(GameEvent.GET_USER_INFO,res);
		Debug.Log (res.ToJson ());
	}

	public void UpdateScore(string stage,string time) {
		Hashtable args = new Hashtable();
		//		args.Add("deviceId", name);
		//		args.Add("timestamp", DateTime.UtcNow.Ticks);
		args.Add("stage",stage);
		args.Add ("time", time);
		StartCoroutine(_network.ReqMsg (NetworkService.MsgUpdateScore, args, OnUpdateScoreResponse));
	}

	private void OnUpdateScoreResponse(JsonData res) {
		Debug.Log("OnUpdateScoreResponse: " + res);
		Messenger<JsonData>.Broadcast (GameEvent.UPDATA_SCORE, res);
	}

	public void GetRankList() {
		//		Hashtable args = new Hashtable();
		//		args.Add("deviceId", name);
		//		args.Add("timestamp", DateTime.UtcNow.Ticks);
		StartCoroutine(_network.ReqMsg (NetworkService.MsgGetRankList, null, OnGetRankListResponse));
	}
	private void OnGetRankListResponse(JsonData res) {
		Debug.Log("OnGetRankListResponse: " + res);
		Messenger<JsonData>.Broadcast (GameEvent.UPDATE_RANK_LIST, res);
	}

	public void SetNickame(string newname) {
		Hashtable args = new Hashtable();
		//		args.Add("deviceId", name);
		//		args.Add("timestamp", DateTime.UtcNow.Ticks);
		args.Add("nickname",newname);
		StartCoroutine(_network.ReqMsg (NetworkService.MsgSetNickame, args, OnSetNickameResponse));
	}
	private void OnSetNickameResponse(JsonData res) {
		Debug.Log("OnSetNickameResponse: " + res);
		Messenger<JsonData>.Broadcast (GameEvent.UPDATE_NICK_NAME, res);
	}

	public void ConsumeMoney(int money){
		Hashtable args = new Hashtable();
		args.Add ("amount", money.ToString ());
		StartCoroutine(_network.ReqMsg (NetworkService.MsgConsumeMoney, args, OnConsumeMoneyResponse));
	}
	private void OnConsumeMoneyResponse(JsonData res){
		Debug.Log("OnConsumeMoneyResponse: " + res);
		if ((int)(res ["error"]) == 0) {
			PlayerPrefs.SetInt ("Money", int.Parse (res ["data"] ["money"].ToString ()));
		} else {
			PlayerPrefs.SetInt ("Money", 0);
		}
	}

	public void ActivateCode(string code){
		Hashtable args = new Hashtable ();
		args.Add ("code", code.ToString ());
		StartCoroutine (_network.ReqMsg (NetworkService.MsgActivateCode, args, OnActivateCodeResponse));
	}

	private void OnActivateCodeResponse(JsonData res){
		Debug.Log ("OnActivateCodeResponse" + res);
		Messenger<JsonData>.Broadcast (GameEvent.EXCHANGE_CODE, res);
	}

	public void BuySkin(int skinIndex){
		Hashtable args = new Hashtable ();
		args.Add ("sid", skinIndex.ToString ());
		StartCoroutine (_network.ReqMsg (NetworkService.MsgBuySkin, args, OnBuySkinResponse));
	}

	private void OnBuySkinResponse(JsonData res){
		Debug.Log("OnBuySkinResponse: " + res);
		Messenger<JsonData>.Broadcast (GameEvent.BUY_SKIN, res);
	}

	public void ShopList(){
		StartCoroutine (_network.ReqMsg (NetworkService.MsgShopList, null, OnShopListResponse));
	}

	private void OnShopListResponse(JsonData res){
		Debug.Log ("OnShopListResponse: " + res);
		if ((int)(res ["error"]) == 0) {
			Debug.Log (res.ToJson ());
			for (int i = 1; i <= 3; ++i) {
				PlayerPrefs.SetInt (string.Format ("SkinPrice_{0}", i), int.Parse (res ["data"] [i.ToString ()].ToString ()));
			}
		} else {
			for (int i = 1; i <= 3; ++i) {
				PlayerPrefs.SetInt (string.Format ("SkinPrice_{0}", i), 999);
			}
		}
	}
}
