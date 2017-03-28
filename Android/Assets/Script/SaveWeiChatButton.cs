using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveWeiChatButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickCallBack(){

//		Texture2D text = Resources.Load("weixin-1@2x") as Texture2D;
//		byte[] data = text.EncodeToPNG ();


		string filename = "weixinerweima.png";
		string Path_save = "";
		if (Application.platform == RuntimePlatform.Android) {
			Path_save = "/mnt/sdcard/DCIM/Camera";
			if (!Directory.Exists (Path_save)) {
				Directory.CreateDirectory (Path_save);
			}
			string destination = Path_save + "/" + filename;
			Path_save = destination;
		}
		Application.CaptureScreenshot (Path_save);


//		File.WriteAllBytes (Path_save, data);

//		string imagePath = "";
//		if(Application.platform==RuntimePlatform.Android || Application.platform==RuntimePlatform.IPhonePlayer)  
//
//			imagePath=Application.persistentDataPath;  
//
//		else if(Application.platform==RuntimePlatform.WindowsPlayer)  
//
//			imagePath=Application.dataPath;  
//
//		else if(Application.platform==RuntimePlatform.WindowsEditor)
//		{  
//
//			imagePath=Application.dataPath;  
//
//			imagePath= imagePath.Replace("/Assets",null);  
//		}
//		imagePath = imagePath + "/screencapture.png";
//		Application.CaptureScreenshot (imagePath);
	}
}
