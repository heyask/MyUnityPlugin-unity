using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyUnityPlugin.Plugins;


/*
iOS 플러그인 사용 샘플
*/
namespace MyUnityPlugin.Plugins.SDK
{
    public class MyUnityPluginSample : MonoBehaviour
    {
        void Start()
        {
            MyUnityPlugin.Instance.SetCallbackInterface(new MyUnityPluginCallbackReceiver());
        }

        public void OnClickTestFunc1()
        {
            MyUnityPlugin.Instance.TestFunc1("string test!");
        }

        public void OnClickTestFunc2()
        {
            MyUnityPlugin.Instance.TestFunc2(999);
        }

        private class MyUnityPluginCallbackReceiver : IMyUnityPluginCallback
        {
            public void OnLoad()
            {
                Debug.Log("MyUnityPluginSample: MyUnityPlugin Callback Test: OnLoad");
            }

            public void OnCallTestFunc1(string _str)
            {
                Debug.Log("MyUnityPluginSample: MyUnityPlugin Callback Test: OnCallTestFunc1");
            }

            public void OnCallTestFunc2(string _num)
            {
                Debug.Log("MyUnityPluginSample: MyUnityPlugin Callback Test: OnCallTestFunc2");
            }
        }
    }
}