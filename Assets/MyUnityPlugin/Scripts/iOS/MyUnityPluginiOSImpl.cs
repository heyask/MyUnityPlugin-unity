using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MyUnityPlugin.Plugins.SDK.Impl
{
#if UNITY_IOS
    internal class MyUnityPluginiOSImpl : IMyUnityPluginImpl
    {
        IMyUnityPluginCallback callbackInterface;

        public void Initialize()
        {
            MyUnityPlugin.__IOS_Initialize();
        }

        /*
        Native Functions
        */
        public string TestFunc1(string _str)
        {
            return MyUnityPlugin.__IOS_TestFunc1(_str);
        }

        public int TestFunc2(int _num)
        {
            return MyUnityPlugin.__IOS_TestFunc2(_num);
        }

        public void SetCallbackInterface(IMyUnityPluginCallback callbackInterface)
        {
            this.callbackInterface = callbackInterface;
        }

        /*
        Callback Functions
        */
        public void __fromnative_OnLoad()
        {
            Debug.Log("MyUnityPluginiOSImpl: __fromnative_OnLoad");
            if (callbackInterface == null) { return; }
            callbackInterface.OnLoad();
        }
        public void __fromnative_OnCallTestFunc1(string _str)
        {
            Debug.Log("MyUnityPluginiOSImpl: __fromnative_OnCallTestFunc1");
            if (callbackInterface == null) { return; }
            callbackInterface.OnCallTestFunc1(_str);
        }
        public void __fromnative_OnCallTestFunc2(string _num)
        {
            Debug.Log("MyUnityPluginiOSImpl: __fromnative_OnCallTestFunc2");
            if (callbackInterface == null) { return; }
            callbackInterface.OnCallTestFunc2(_num);
        }
    }
#endif
}