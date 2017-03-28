using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class ClipboardManager{
	#if UNITY_IPHONE
	/* Interface to native implementation */
	[DllImport ("__Internal")]
	private static extern void _copyTextToClipboard(string text);
	#endif

	public static void CopyToClipboard( string input)
	{
		#if UNITY_ANDROID
		// 对Android的调用
		AndroidJavaObject androidObject = new AndroidJavaObject("com.androidclip.clip.ClipBoardTools");     
		AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		if (activity == null)
		return ;

		// 复制到剪贴板
		androidObject.Call("copyTextToClipboard", activity, input);

		// 从剪贴板中获取文本
		//String text =androidObject.Call<String>("getTextFromClipboard");
		#elif UNITY_IPHONE
		_copyTextToClipboard(input);
		#elif UNITY_EDITOR  
		TextEditor t = new TextEditor();
		t.content = new GUIContent(input);
		t.OnFocus();
		t.Copy();
		#endif
	}

}
