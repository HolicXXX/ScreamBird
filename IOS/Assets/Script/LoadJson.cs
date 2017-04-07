using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public enum BlockType{
	BT_GOAL,
	BT_DOWNBLOCK,
	BT_UPBLOCK,
	BT_DOWNSPIKE,
	BT_UPSPIKE,
	BT_STAIR,
	BT_LRSTAIR,
	BT_UDSTAIR,
	BT_FALLSTAIR,
	BT_ENEMY
}

[SerializeField]
public class BlockBase{
	public int _id{ get; set; }
	public BlockType _type{ get; set; }
	public float _startX{ get; set; }
	public float _endX{ get; set; }
	public float _Y{ get; set; }
	public float _stairXStart{ get; set; }
	public float _stairXEnd{ get; set; }
	public float _stairYStart{ get; set; }
	public float _stairYEnd{ get; set; }
	public float _stairMoveTime{ get; set;}
	public float _stairDelay{ get; set; }
}

public class LoadJson : MonoBehaviour {

	public static string currentJsonFileName;
	public static string currentJsonString;

	public static JsonData LoadDefaultUserInfo(){//firsttime load
		TextAsset resJson = Resources.Load ("UserInfo") as TextAsset;
		string _data = resJson.text;
		Debug.Log (_data);
		JsonData ret = JsonMapper.ToObject (_data);
		return ret;
	}

	public static JsonData LoadSavedUserInfo(){
		string oringPath = "";
		#if UNITY_ANDROID
		oringPath = Application.persistentDataPath;
		#else
		oringPath = Application.persistentDataPath;
		#endif
		string filePath = oringPath + "/UserInfo.json";
		JsonData ret;
		if (!File.Exists (filePath)) {
			ret = LoadDefaultUserInfo ();
			SaveUserInfo (ret);
		} else {
			FileStream fs = new FileStream (filePath, FileMode.Open);
			StreamReader sr = new StreamReader (fs);
			string data = sr.ReadToEnd ();
			Debug.Log (data);
//			char[] temp = new char[data.Length - 2];
//			data.CopyTo (1, temp, 0, data.Length - 2);
//			string strTemp = new string (temp);
//			Debug.Log (strTemp);
//			Debug.Log (strTemp.StartsWith("{"));
//			Debug.Log (strTemp.EndsWith ("}"));
//			Debug.Log (strTemp.Replace("\\",""));
//			strTemp = strTemp.Replace ("\\", "");
//			string strTemp = data.Replace ("\\", "");
//			Debug.Log (strTemp);
			ret = JsonMapper.ToObject (data);
			fs.Close ();
		}
		return ret;
	}

	public static void SaveUserInfo(JsonData data){
		string oringPath = "";
		#if UNITY_ANDROID
		oringPath = Application.persistentDataPath;
		#else
		oringPath = Application.persistentDataPath;
		#endif
		string filePath = oringPath + "/UserInfo.json";
		if (File.Exists (filePath)) {
			File.Delete(filePath);
		}
		Debug.Log (JsonMapper.ToJson (data));
		FileStream fs = new FileStream (filePath, FileMode.Create);
		byte[] bts = System.Text.Encoding.UTF8.GetBytes (JsonMapper.ToJson (data));
		fs.Write (bts, 0, bts.Length);
		fs.Close ();
	}

	public static bool IsStageInfoFileExists(){
		return File.Exists (Application.persistentDataPath + "/StageInfo.json");
	}

	public static List<BlockBase> LoadJsonByName(JsonData jdata){
		if (jdata == null) {
			string originPath = Application.persistentDataPath + "/StageInfo.json";
			FileStream fs = new FileStream (originPath, FileMode.Open);
			StreamReader sr = new StreamReader (fs);
			string data = sr.ReadToEnd ();
			jdata = JsonMapper.ToObject (data);
			fs.Close ();
		}
//		currentJsonFileName = fileName;
//		TextAsset jsonfile = Resources.Load ("stage") as TextAsset;
//		string _data = jsonfile.text;
//		currentJsonString = _data;
//		JsonData jdata = JsonMapper.ToObject (_data);
		if (jdata.IsArray) {
			List<BlockBase> ret = new List<BlockBase>();
			var list = jdata;
			for (int i = 0; i < list.Count; ++i) {
				//Debug.Log (DataManager._currentStage.ToString () + " " + list [i] ["stage"]);
				if (DataManager._currentStage == toInt(list [i] ["stage"].ToString())) {
					//Debug.Log ("add");
					BlockBase bb = new BlockBase ();
					bb._id = toInt(list [i] ["id"].ToString());
					bb._type = (BlockType)toInt(list [i] ["style"].ToString());
					bb._startX = toFloat(list [i] ["xHead"].ToString());
					bb._endX = toFloat(list [i] ["xEnd"].ToString());
					bb._Y = toFloat(list [i] ["yHead"].ToString());
					if (bb._type == BlockType.BT_LRSTAIR) {
						bb._stairXStart = toFloat(list [i] ["xMin"].ToString());
						bb._stairXEnd = toFloat(list [i] ["xMax"].ToString());
						bb._stairMoveTime = toFloat(list [i] ["time"].ToString());
					}
					if (bb._type == BlockType.BT_UDSTAIR) {
						bb._stairYStart = toFloat(list [i] ["yMin"].ToString());
						bb._stairYEnd = toFloat(list [i] ["yMax"].ToString());
						bb._stairMoveTime = toFloat(list [i] ["time"].ToString());
					}
					if (bb._type == BlockType.BT_FALLSTAIR) {
						bb._stairDelay = toFloat(list [i] ["fallTime"].ToString());
					}
					ret.Add (bb);
				}
			}
			return ret;
		}
		return null;
	}

	private static JsonData _languageJson = null;

	public static string GetLanguageText(string keyName){
		if (LoadJson._languageJson == null) {
			LoadJson.LoadLanguageJson ();
		}
		string result = "";
		int index = -1;
		DataManager._nameIndex.TryGetValue (keyName, out index);
		if (index == -1) {
			Debug.Log ("No value at key: " + keyName);
			return "";
		}
		result = _languageJson [index] [DataManager._languageIndex].ToString ();
		return result;
	}

	private static void LoadLanguageJson(){
		TextAsset jsonfile = Resources.Load ("LanguageText") as TextAsset;
		string _data = jsonfile.text;
		_languageJson = JsonMapper.ToObject (_data);
	}

	//public static 
	static int toInt(string str){
		return Int32.Parse (str);
	}
	static float toFloat(string str){
		return float.Parse (str);
	}
}
