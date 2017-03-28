using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UnityEngine;
using LitJson;
using UnityEngine.iOS;
using System.Runtime.InteropServices;

public class NetUtil : MonoBehaviour {

	public const string msgUrl = "https://api.shaojishiduo.com/GameScream/";

	public static string MsgGetUserInfo = "getUserInfo";
	public static string MsgUpdateScore = "updateScore";
	public static string MsgGetRankList = "getRankList";
	public static string MsgSetNickame = "setNickame";
	public static string MsgConsumeMoney = "consumeMoney";
	//public static string MsgShare = "share";

	public static string deviceId = ""; //78:31:c1:c2:d3:a0

	private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";  
	private static bool CheckValidationResult(object sender, 
		X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)  
	{  
		return true; //总是接受     
	}

	public static HttpWebResponse CreatePostHttpResponse(string url, 
		IDictionary<string, string> parameters, Encoding charset)  
	{  
		HttpWebRequest request = null;  
		//HTTPSQ请求  
		ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);  
		request = WebRequest.Create(url) as HttpWebRequest;  
		request.ProtocolVersion = HttpVersion.Version10;  
		request.Method = "POST";  
		request.ContentType = "application/x-www-form-urlencoded";  
		request.UserAgent = DefaultUserAgent;  
		//request.BeginGetRequestStream(
		//如果需要POST数据     
		if (!(parameters == null || parameters.Count == 0))  
		{  
			StringBuilder buffer = new StringBuilder();  
			int i = 0;  
			foreach (string key in parameters.Keys)  
			{  
				if (i > 0)  
				{  
					buffer.AppendFormat("&{0}={1}", key, parameters[key]);  
				}  
				else  
				{  
					buffer.AppendFormat("{0}={1}", key, parameters[key]);  
				}  
				i++;  
			}  
			byte[] data = charset.GetBytes(buffer.ToString());  
			using (Stream stream = request.GetRequestStream())  
			{  
				stream.Write(data, 0, data.Length);  
			}  
		}  
		return request.GetResponse() as HttpWebResponse;  
	}

	public static JsonData RequestMsg(string msg, IDictionary<string, string> parameters){
		Encoding encoding = Encoding.GetEncoding("utf-8");  
//		IDictionary<string, string> parameters = new Dictionary<string, string>();  
		parameters.Add("skey", NetUtil.getMacAdd());
		HttpWebResponse response = NetUtil.CreatePostHttpResponse(msgUrl + msg, parameters, encoding);  
		//打印返回值  
		Stream stream = response.GetResponseStream();   //获取响应的字符串流  
		StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
		string res = sr.ReadToEnd();   //从头读到尾，放到字符串html
		Debug.Log("RequestMsg res: " + res);
		//res = "{\"error\":0,\"data\":{\"stage_1\":[{\"name\":\"guest_3b2p05\",\"score\":\"12345\"}],\"stage_2\":[],\"stage_3\":[]}}";
		JsonData jsdArray = JsonMapper.ToObject(res);//转换成json格式;需要引入LitJson
		return jsdArray;
	}

	public static string Base64Encode(string message) {  
		byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(message);  
		return Convert.ToBase64String(bytes);  
	}  

	public static string Base64Decode(string message) {  
		byte[] bytes = Convert.FromBase64String(message);  
		return Encoding.GetEncoding("utf-8").GetString(bytes);  
	}

//	[DllImport("__Internal")]
//	private static extern string GetIphoneADID();

	public static string getMacAdd(){
//		#if UNITY_ANDROID
//			deviceId = SystemInfo.deviceUniqueIdentifier;
//		#elif UNITY_IPHONE  
//			deviceId = "IOS-" + GetIphoneADID();
//		#endif

//		if (Application.platform == RuntimePlatform.IPhonePlayer) {
//			//deviceId = Device.advertisingIdentifier;
//			if (deviceId == "") {
//				NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces(); 
//				string mac = "";
//				foreach (NetworkInterface ni in nis) {
//					mac = ni.GetPhysicalAddress ().ToString ();
//					if (mac != "") {
//						deviceId = NetUtil.Base64Encode(mac);
//						break;
//					}
//					//			mac = mac + "-" + ni.GetPhysicalAddress ().ToString ();
//					//			Debug.Log ("Name = " + ni.Name);  
//					//			Debug.Log ("Des = " + ni.Description);  
//					//			Debug.Log ("Type = " + ni.NetworkInterfaceType.ToString() );  
//					//			Debug.Log ("Mac地址 = " + ni.GetPhysicalAddress().ToString() );  
//					//			Debug.Log ("------------------------------------------------");  
//				}  
//			}
//		}
//		else {
//			deviceId = SystemInfo.deviceUniqueIdentifier;
//		}

		deviceId = DeviceID.Get ();
		Debug.Log("deviceId:" + deviceId);
		//return NetUtil.Base64Encode(deviceId);
		return StringUtil.Base64Encode(deviceId);
	}

	public static IEnumerator NetRequest(string msgName, WWWForm form)
	{
		if (deviceId == ""){
			Debug.Log ("deviceId 1:" + deviceId);
//			setMacAdd();
			Debug.Log ("deviceId 2:" + deviceId);
		}

		form.AddField("skey", deviceId);
		WWW ret = new WWW(msgUrl + msgName, form);
		yield return ret;
		if (ret.error != null)
		{
			Debug.LogError("error:" + ret.error);
			yield break;
		}
		if (string.IsNullOrEmpty(ret.text))
		{
			yield break;
		}
		Debug.Log("NetResponse:" + ret.text);
		JsonData jsdArray = JsonMapper.ToObject(ret.text);//转换成json格式;需要引入LitJson
		if ((string)jsdArray["error"] == "0") {
			yield return jsdArray;
		}
	}

}
