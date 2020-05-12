using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using MyUnityPlugin.Plugins.SDK.Impl;


namespace MyUnityPlugin.Plugins.SDK
{
    public interface IMyUnityPluginCallback
    {
        void OnLoad();
        void OnCallTestFunc1(string _str);
        void OnCallTestFunc2(string _num);
    }

    internal interface IMyUnityPluginImpl
    {
        void Initialize();
        string TestFunc1(string _str);
        int TestFunc2(int _num);
        void SetCallbackInterface(IMyUnityPluginCallback callback);
    }

    public class MyUnityPlugin : MonoBehaviour
    {
        private static MyUnityPlugin mInstance;
        private IMyUnityPluginImpl mImpl;

        public static MyUnityPlugin Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = FindObjectOfType(typeof(MyUnityPlugin)) as MyUnityPlugin;
                    if (mInstance == null)
                    {
                        var instanceObj = new GameObject("MyUnityPlugin");
                        mInstance = instanceObj.AddComponent<MyUnityPlugin>();
                    }
                }

                return mInstance;
            }
        }

        void Awake()
        {
            mInstance = this;
        }


        private MyUnityPlugin()
        {
#if UNITY_ANDROID
		    mImpl = new GarlicWebviewAndroidImpl();
#elif UNITY_IOS
            mImpl = new MyUnityPluginiOSImpl();
#endif
            mImpl.Initialize();
        }

        /*
        Native Functions
        */
        public string TestFunc1(string _str)
        {
            if (mImpl != null)
            {
                return mImpl.TestFunc1(_str);
            }

            return null;
        }

        public int TestFunc2(int _num)
        {
            if (mImpl != null)
            {
                return mImpl.TestFunc2(_num);
            }

            return -1;
        }

        public void SetCallbackInterface(IMyUnityPluginCallback callbackInterface)
        {
            mImpl.SetCallbackInterface(callbackInterface);
        }

#if UNITY_IOS
        [DllImport("__Internal")]
        internal static extern void __IOS_Initialize();
        [DllImport("__Internal")]
        internal static extern string __IOS_TestFunc1(string str);
        [DllImport("__Internal")]
        internal static extern int __IOS_TestFunc2(int num);


        #region Callback from native

        void __fromnative_OnLoad()
        {
            (mImpl as MyUnityPluginiOSImpl).__fromnative_OnLoad();
        }
        void __fromnative_OnCallTestFunc1(string _str)
        {
            (mImpl as MyUnityPluginiOSImpl).__fromnative_OnCallTestFunc1(_str);
        }
        void __fromnative_OnCallTestFunc2(string _num)
        {
            (mImpl as MyUnityPluginiOSImpl).__fromnative_OnCallTestFunc2(_num);
        }

        #endregion

#endif
    }
}
