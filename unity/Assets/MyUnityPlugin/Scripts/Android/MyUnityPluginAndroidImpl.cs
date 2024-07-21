using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MyUnityPlugin.Plugins.SDK.Impl
{
#if UNITY_ANDROID
    internal class MyUnityPluginAndroidImpl : IMyUnityPluginImpl
    {
        private AndroidJavaObject activityContext = null;
        private AndroidJavaClass pluginClass = null;
        private AndroidJavaObject pluginClassInstance = null;
        private CallbackBridge callback;


        private class CallbackBridge : AndroidJavaProxy
        {
            private IMyUnityPluginCallback callbackInterface;
            public CallbackBridge(IMyUnityPluginCallback callbackInterface) : base("com.example.myunityplugin.MyUnityPluginCallback")
            {
                this.callbackInterface = callbackInterface;
            }

            public void SetCallbackInterface(IMyUnityPluginCallback callbackInterface)
            {
                this.callbackInterface = callbackInterface;
            }

            public void OnLoad()
            {
                Debug.Log("MyUnityPluginAndroidImpl: OnLoad");
                if (callbackInterface == null) { return; }
                callbackInterface.OnLoad();
            }

            public void OnCallTestFunc1(string str)
            {
                Debug.Log("MyUnityPluginAndroidImpl: OnCallTestFunc1");
                if (callbackInterface == null) { return; }
                callbackInterface.OnCallTestFunc1(str);
            }

            public void OnCallTestFunc2(int num)
            {
                Debug.Log("MyUnityPluginAndroidImpl: OnCallTestFunc2");
                if (callbackInterface == null) { return; }
                callbackInterface.OnCallTestFunc2(num);
            }
        }

        public bool Initialize()
        {
            callback = new CallbackBridge(null);

            //일단 아까 plugin의 context를 설정해주기 위해
            //유니티 자체의 UnityPlayerActivity를 가져옵시다.
            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

            }

            //클래스를 불러와줍니다.
            //패키지명 + 클래스명입니다.
            using (pluginClass = new AndroidJavaClass("com.example.myunityplugin.MyUnityPluginBridge"))
            {
                if (pluginClass != null)
                {
                    //아까 싱글톤으로 사용하자고 만들었던 static instance를 불러와줍니다.
                    pluginClassInstance = pluginClass.CallStatic<AndroidJavaObject>("instance");
                    //Context를 설정해줍니다.
                    pluginClassInstance.Call("setContext", activityContext);
                    //Initialize
                    pluginClassInstance.Call<bool>("Initialize", callback);
                }
            }

            if (activityContext != null && pluginClass != null && pluginClassInstance != null)
                return true;
            else
                return false;
        }

        public string TestFunc1(string str)
        {
            //Toast는 안드로이드의 UiThread를 사용하기때문에 
            //UnityPlayerActivity UiThread를 호출하고, 다시 ShowToast를 호출합니다.

            //UiThread에서 호출하지 않으면 Looper.prepare()어쩌고 에러가 뜨는데..
            //제대로 이해하지 못했습니다.. 누가 설명좀해줘요.
            activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                pluginClassInstance.Call("ShowToast", "Hello world!!");
            }));

            return "a";
        }

        public int TestFunc2(int num)
        {

            //아까 JavaClass에서 인수로 받아 넣던
            //objName과 objMethod의 정체입니다.
            //objName은 Scene내의 GameObject의 이름입니다 (ex)gameobject.transform.name)
            //objMethod는 GameObject에 SendMessage할 메소드명입니다.
            object[] tmpObj = new object[2];
            tmpObj[0] = "MyUnityPlugin";
            tmpObj[1] = "DebugLog";

            //불러봅시다.
            pluginClassInstance.Call("AndroidVersionCheck", tmpObj);

            return 1;
        }

        public void SetCallbackInterface(IMyUnityPluginCallback callbackInterface)
        {
            callback.SetCallbackInterface(callbackInterface);
        }


        // private static string fullClassName = "com.example.myunityplugin.MyUnityPluginBridge";
        // private AndroidJavaClass pluginClass;
        // private AndroidJavaObject pluginClassInstance;
        // private AndroidJavaObject unityActivity;
        // private CallbackBridge callback;

        // private class CallbackBridge : AndroidJavaProxy
        // {
        //     private IMyUnityPluginCallback callbackInterface;
        //     public CallbackBridge(IMyUnityPluginCallback callbackInterface) : base("com.example.myunityplugin.MyUnityPluginCallback")
        //     {
        //         this.callbackInterface = callbackInterface;
        //     }

        //     public void SetCallbackInterface(IMyUnityPluginCallback callbackInterface)
        //     {
        //         this.callbackInterface = callbackInterface;
        //     }

        //     public void OnCallTestFunc1(string str)
        //     {
        //         if (callbackInterface == null) { return; }
        //         callbackInterface.OnCallTestFunc1(str);
        //     }

        //     public void OnCallTestFunc2(string num)
        //     {
        //         if (callbackInterface == null) { return; }
        //         callbackInterface.OnCallTestFunc2(num);
        //     }
        // }

        // public MyUnityPluginAndroidImpl()
        // {
        //     AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //     unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        //     // callback = new CallbackBridge(null);
        //     // pluginClass = new AndroidJavaClass(fullClassName);
        //     // pluginClassInstance = pluginClass.CallStatic<AndroidJavaObject>("instance");


        //     // using (pluginClass = new AndroidJavaClass(fullClassName))
        //     // {
        //     //     if (pluginClass != null)
        //     //     {
        //     //         //아까 싱글톤으로 사용하자고 만들었던 static instance를 불러와줍니다.
        //     //         pluginClassInstance = pluginClass.CallStatic<AndroidJavaObject>("instance");
        //     //         //Context를 설정해줍니다.
        //     //         // pluginClassInstance.Call("setContext", unityActivity);
        //     //     }
        //     // }
        // }

        // #region implementation

        // public bool Initialize()
        // {
        //     //Do Nothing
        //     // return pluginClassInstance.Call<bool>("Initialize", callback);
        //     return true;
        // }

        // public string TestFunc1(string str)
        // {
        //     Debug.Log("a");
        //     return "a";
        //     // return pluginClass.CallStatic("TestFunc1", str);
        // }

        // public int TestFunc2(int num)
        // {
        //     Debug.Log("1");
        //     return 1;
        //     // return pluginClass.CallStatic("TestFunc2", num);
        // }

        // public void SetCallbackInterface(IMyUnityPluginCallback callbackInterface)
        // {
        //     // callback.SetCallbackInterface(callbackInterface);
        // }

        // #endregion
    }
#endif
}