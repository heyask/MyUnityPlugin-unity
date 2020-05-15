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

        public bool Initialize()
        {
            MyUnityPlugin.__IOS_Initialize();
            // TODO
            return true;
        }

        /*
        Native Functions
        */
        public string TestFunc1(string str)
        {
            return MyUnityPlugin.__IOS_TestFunc1(str);
        }

        public int TestFunc2(int num)
        {
            return MyUnityPlugin.__IOS_TestFunc2(num);
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
        public void __fromnative_OnCallTestFunc1(string str)
        {
            Debug.Log("MyUnityPluginiOSImpl: __fromnative_OnCallTestFunc1");
            if (callbackInterface == null) { return; }
            callbackInterface.OnCallTestFunc1(str);
        }
        public void __fromnative_OnCallTestFunc2(string num)
        {
            Debug.Log("MyUnityPluginiOSImpl: __fromnative_OnCallTestFunc2");
            if (callbackInterface == null) { return; }
            try {
                int parsedNum = int.Parse(num);
                callbackInterface.OnCallTestFunc2(parsedNum);
            } catch(FormatException e) {
                throw new System.FormatException(e, "original");
            } catch(OverflowException e) {
                throw new System.OverflowException(e, "original");
            }
        }
    }
#endif
}