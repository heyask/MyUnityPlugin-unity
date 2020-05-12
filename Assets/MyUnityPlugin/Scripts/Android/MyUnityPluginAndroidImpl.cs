﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUnityPlugin.Plugins.SDK.Impl
{
#if UNITY_Android
    internal class MyUnityPluginAndroidImpl : IMyUnityPluginImpl
    {
		private static string fullClassName = "com.tapas.garlic.plugin.webview.GarlicWebDialogUnityBridge";
		private AndroidJavaClass pluginClass;
		private AndroidJavaObject unityActivity;
		private CallbackBridge callback;

		private class CallbackBridge : AndroidJavaProxy
		{
			private IMyUnityPluginCallback callbackInterface;
			public CallbackBridge(IMyUnityPluginCallback callbackInterface) : base("com.tapas.garlic.plugin.webview.GarlicWebDialogCallback")
			{
				this.callbackInterface = callbackInterface;
			}

			public void SetCallbackInterface(IMyUnityPluginCallback callbackInterface)
			{
				this.callbackInterface = callbackInterface;
			}

			public void OnCallTestFunc1(string _str)
			{
				if (callbackInterface == null) { return; }
				callbackInterface.OnCallTestFunc1(message);
			}
			public void OnCallTestFunc2(string _num)
			{
				if (callbackInterface == null) { return; }
				callbackInterface.OnCallTestFunc2(url);
			}
		}
        
		public MyUnityPluginAndroidImpl()
		{
			callback = new CallbackBridge(null);
			pluginClass = new AndroidJavaClass(fullClassName);
			unityActivity = GetUnityActivity();
		}

    #region implementation

		public void Initialize()
		{
			//Do Nothing
			pluginClass.CallStatic<bool>("Initialize", callback);
		}

		public void TestFunc1(string _str)
		{
			pluginClass.CallStatic("TestFunc1", _str);
		}

		public void TestFunc2(int _num)
		{
			pluginClass.CallStatic("TestFunc2", _num);
		}

		public void SetCallbackInterface(IGarlicWebviewCallback callbackInterface)
		{
			callback.SetCallbackInterface(callbackInterface);
		}

    #endregion
        // private AndroidJavaObject activityContext = null;
        // private AndroidJavaClass javaClass = null;
        // private AndroidJavaObject javaClassInstance = null;

        // void Awake()
        // {

        //     //일단 아까 plugin의 context를 설정해주기 위해
        //     //유니티 자체의 UnityPlayerActivity를 가져옵시다.
        //     using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        //     {
        //         activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

        //     }

        //     //클래스를 불러와줍니다.
        //     //패키지명 + 클래스명입니다.
        //     using (javaClass = new AndroidJavaClass("com.example.MyUnityPlugin.MyUnityPluginClass"))
        //     {
        //         if (javaClass != null)
        //         {
        //             //아까 싱글톤으로 사용하자고 만들었던 static instance를 불러와줍니다.
        //             javaClassInstance = javaClass.CallStatic<AndroidJavaObject>("instance");
        //             //Context를 설정해줍니다.
        //             javaClassInstance.Call("setContext", activityContext);
        //         }
        //     }
        // }

        // public void CallShowToast()
        // {
        //     //Toast는 안드로이드의 UiThread를 사용하기때문에 
        //     //UnityPlayerActivity UiThread를 호출하고, 다시 ShowToast를 호출합니다.

        //     //UiThread에서 호출하지 않으면 Looper.prepare()어쩌고 에러가 뜨는데..
        //     //제대로 이해하지 못했습니다.. 누가 설명좀해줘요.
        //     activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        //     {
        //         javaClassInstance.Call("ShowToast", "Hello world!!");
        //     }));
        // }

        // public void CallAndroidVersionCheck()
        // {

        //     //아까 JavaClass에서 인수로 받아 넣던
        //     //objName과 objMethod의 정체입니다.
        //     //objName은 Scene내의 GameObject의 이름입니다 (ex)gameobject.transform.name)
        //     //objMethod는 GameObject에 SendMessage할 메소드명입니다.
        //     object[] tmpObj = new object[2];
        //     tmpObj[0] = "AndroidManager";
        //     tmpObj[1] = "AndroidLog";

        //     //불러봅시다.
        //     javaClassInstance.Call("AndroidVersionCheck", tmpObj);
        // }


        // // tmpObj[1] = "AndroidLog";
        // // 요기서 설정한 메소드명입니다.
        // public void AndroidLog(string str)
        // {
        //     Debug.Log(str);
        // }
    }
#endif
}