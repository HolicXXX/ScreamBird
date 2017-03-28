using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using cn.sharesdk.unity3d.sdkporter;
using System.IO;


public static class ShareSDKPostProcessBuild 
{
	//[PostProcessBuild]
	[PostProcessBuildAttribute(88)]
	public static void onPostProcessBuild(BuildTarget target,string targetPath)
	{
		string unityEditorAssetPath = Application.dataPath;

		if (target != BuildTarget.iOS) 
		{
			Debug.LogWarning ("Target is not iPhone. XCodePostProcess will not run");
			return;
		}

		XCProject project = new XCProject (targetPath);
		//var files = System.IO.Directory.GetFiles( unityEditorAssetPath, "*.projmods", System.IO.SearchOption.AllDirectories );
		var files = System.IO.Directory.GetFiles( unityEditorAssetPath + "/ShareSDKiOSAutoPackage/Editor/SDKPorter", "*.projmods", System.IO.SearchOption.AllDirectories);
		foreach( var file in files ) 
		{
			project.ApplyMod( file );
		}

		//如需要预配置Xocode中的URLScheme 和 白名单,请打开下两行代码,并自行配置相关键值
		string projPath = Path.GetFullPath (targetPath);
		EditInfoPlist (projPath);

		//编辑代码文件
        EditorCode(projPath);

		//Finally save the xcode project
		project.Save();
	}

	private static void EditInfoPlist(string projPath)
	{

		XCPlist plist = new XCPlist (projPath);

		//URL Scheme 添加
		string PlistAdd = @"  
            <key>CFBundleURLTypes</key>
			<array>
				<dict>
					<key>CFBundleURLName</key>
					<string>meipai</string>
					<key>CFBundleURLSchemes</key>
					<array>
						<string>mp1089867596</string>
					</array>
				</dict>
				<dict>
					<key>CFBundleURLSchemes</key>
					<array>
						<string>dingoanxyrpiscaovl4qlw</string>
					</array>
					<key>CFBundleURLName</key>
					<string>dingtalk</string>
				</dict>
				<dict>
					<key>CFBundleURLSchemes</key>
					<array>
						<string>ap2015072400185895</string>
					</array>
					<key>CFBundleURLName</key>
					<string>alipayShare</string>
				</dict>
				<dict>
					<key>CFBundleURLSchemes</key>
					<array>
					<string>vk5312801</string>
					<string>yx0d9a9f9088ea44d78680f3274da1765f</string>
					<string>pin4797078908495202393</string>
					<string>kakao48d3f524e4a636b08d81b3ceb50f1003</string>
					<string>pdk4797078908495202393</string>
					<string>tb2QUXqO9fcgGdtGG1FcvML6ZunIQzAEL8xY6hIaxdJnDti2DYwM</string>
					<string>com.mob.demoShareSDK</string>
					<string>rm226427com.mob.demoShareSDK</string>
					<string>pocketapp1234</string>
					<string>QQ41EC8DB4</string>
					<string>wxe959e86bfda2583c</string>
					<string>tencent1106021812</string>
					<string>fb732683093575228</string>
					<string>wb3456727435</string>
					</array>
				</dict>
			</array>";

		//白名单添加
		string LSAdd = @"
		<key>LSApplicationQueriesSchemes</key>
			<array>
			<string>dingtalk-open</string>
			<string>dingtalk</string>
			<string>mqqopensdkapiV4</string>
			<string>weibosdk</string>
			<string>sinaweibohd</string>
			<string>sinaweibo</string>
			<string>vkauthorize</string>
			<string>fb-messenger</string>
			<string>yixinfav</string>
			<string>yixinoauth</string>
			<string>yixinopenapi</string>
			<string>yixin</string>
			<string>pinit</string>
			<string>kakaolink</string>
			<string>kakao48d3f524e4a636b08d81b3ceb50f1003</string>
			<string>alipay</string>
			<string>storykompassauth</string>
			<string>pinterestsdk.v1</string>
			<string>kakaokompassauth</string>
			<string>alipayshare</string>
			<string>pinit</string>
			<string>line</string>
			<string>whatsapp</string>
			<string>mqqwpa</string>
			<string>instagram</string>
			<string>fbauth2</string>
			<string>renren</string>
			<string>renrenios</string>
			<string>renrenapi</string>
			<string>rm226427com.mob.demoShareSDK</string>
			<string>mqq</string>
			<string>mqqopensdkapiV2</string>
			<string>mqqopensdkapiV3</string>
			<string>wtloginmqq2</string>
			<string>mqqapi</string>
			<string>mqqOpensdkSSoLogin</string>
			<string>sinaweibohdsso</string>
			<string>sinaweibosso</string>
			<string>wechat</string>
			<string>weixin</string>
		</array>";

		//权限
		string NSAdd = @"
			<key>UIRequiresPersistentWiFi</key>
			<true/>
			<key>NSCameraUsageDescription</key>
			<string>请求访问摄像机</string>
			<key>NSMicrophoneUsageDescription</key>
			<string>请求访问麦克风</string>
            <key>NSPhotoLibraryUsageDescription</key>
            <string>请求访问相册</string>";

		//在plist里面增加一行
		plist.AddKey(PlistAdd);
		plist.AddKey (LSAdd);
		plist.AddKey (NSAdd);
		plist.Save();
	}

	private static void EditorCode(string filePath)
    {
		//读取UnityAppController.mm文件
        XClass UnityAppController = new XClass(filePath + "/Classes/UnityAppController.mm");

		string shareRecAddImport = @"
			#import <ShareSDK/ShareSDK.h>
			#import <ShareSDKConnector/ShareSDKConnector.h>

			//腾讯开放平台（对应QQ和QQ空间）SDK头文件
			#import <TencentOpenAPI/TencentOAuth.h>
			#import <TencentOpenAPI/QQApiInterface.h>

			//微信SDK头文件
			#import ""WXApi.h""
		";
 
        //在指定代码后面增加一行代码
		UnityAppController.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"", shareRecAddImport);
//        UnityAppController.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"","#import <ShareSDK/ShareSDK.h>\r#import <ShareSDKConnector/ShareSDKConnector.h>\r#import <TencentOpenAPI/TencentOAuth.h>\r#import <TencentOpenAPI/QQApiInterface.h>\r#import \"WXApi.h\"");
 
        //在指定代码中替换一行
        // UnityAppController.Replace("return YES;","return [ShareSDK handleOpenURL:url sourceApplication:sourceApplication annotation:annotation wxDelegate:nil];");
 
		string shareRecAddCode = @"
[ShareSDK registerApp:@""1bc53f0b85612""
     
          activePlatforms:@[
                            //@(SSDKPlatformTypeSinaWeibo),
                            //@(SSDKPlatformTypeMail),
                            //@(SSDKPlatformTypeSMS),
                            //@(SSDKPlatformTypeCopy),
                            @(SSDKPlatformTypeFacebook),
                            @(SSDKPlatformTypeTwitter),
                            @(SSDKPlatformTypeWechat),
                            @(SSDKPlatformTypeQQ),
                            //@(SSDKPlatformTypeRenren),
                            //@(SSDKPlatformTypeGooglePlus)
                            ]
                 onImport:^(SSDKPlatformType platformType)
     {
         switch (platformType)
         {
             case SSDKPlatformTypeWechat:
                 [ShareSDKConnector connectWeChat:[WXApi class]];
                 break;
             case SSDKPlatformTypeQQ:
                 [ShareSDKConnector connectQQ:[QQApiInterface class] tencentOAuthClass:[TencentOAuth class]];
                 break;
//             case SSDKPlatformTypeSinaWeibo:
//                 [ShareSDKConnector connectWeibo:[WeiboSDK class]];
//                 break;
//             case SSDKPlatformTypeRenren:
//                 [ShareSDKConnector connectRenren:[RennClient class]];
//                 break;
             default:
                 break;
         }
     }
          onConfiguration:^(SSDKPlatformType platformType, NSMutableDictionary *appInfo)
     {
         
         switch (platformType)
         {
//             case SSDKPlatformTypeSinaWeibo:
//                 //设置新浪微博应用信息,其中authType设置为使用SSO＋Web形式授权
//                 [appInfo SSDKSetupSinaWeiboByAppKey:@""3456727435""
//                                           appSecret:@""d114468818a80eef31892a0760124be1""
//                                         redirectUri:@""http://www.sharesdk.cn""
//                                            authType:SSDKAuthTypeBoth];
//                 break;
             case SSDKPlatformTypeWechat:
                 [appInfo SSDKSetupWeChatByAppId:@""wxe959e86bfda2583c""
                                       appSecret:@""62093411771f00c82e2a0b6aaf970ca6""];
                 break;
             case SSDKPlatformTypeQQ:
                 [appInfo SSDKSetupQQByAppId:@""1106021812""
                                      appKey:@""ZxWcDTBb90cfTyDr""
                                    authType:SSDKAuthTypeBoth];
                 break;
             case SSDKPlatformTypeFacebook:
                 [appInfo SSDKSetupFacebookByApiKey:@""732683093575228""
                                          appSecret:@""ea0325352875f3ea1995ce2eb5817749""
                                           authType:SSDKAuthTypeBoth];
                 break;
             case SSDKPlatformTypeTwitter:
                 [appInfo SSDKSetupTwitterByConsumerKey:@""XId3NFijHo40WQnwis8UDdFVd""
                    consumerSecret:@""WHB6IbVt7yTIZAte2LuzJeedTPJbEU6RHe1CQcotstJ3Zzu1oC""
                    redirectUri:@""https://api.shaojishiduo.com/ThirdPlugin/twitter_notify""];
                 break;
                 //             case SSDKPlatformTypeRenren:
                 //                 [appInfo        SSDKSetupRenRenByAppId:@""226427""
                 //                                                 appKey:@""fc5b8aed373c4c27a05b712acba0f8c3""
                 //                                              secretKey:@""f29df781abdd4f49beca5a2194676ca4""
                 //                                               authType:SSDKAuthTypeBoth];
                 //                 break;
                 //             case SSDKPlatformTypeGooglePlus:
                 //                 [appInfo SSDKSetupGooglePlusByClientID:@""232554794995.apps.googleusercontent.com""
                 //                                           clientSecret:@""PEdFgtrMw97aCvf0joQj7EMk""
                 //                                            redirectUri:@""http://localhost""];
                 //                 break;
             default:
                 break;
         }
     }];
		";
        //在指定代码后面增加一行
		UnityAppController.WriteBelow("- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions\n{", shareRecAddCode);
        // UnityAppController.WriteBelow("UnityCleanup();\n}","- (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url\r{\r    return [ShareSDK handleOpenURL:url wxDelegate:nil];\r}");
 
    }


}
