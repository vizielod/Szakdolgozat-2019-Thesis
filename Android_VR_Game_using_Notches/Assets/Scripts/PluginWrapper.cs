using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PluginWrapper : MonoBehaviour
{
    private AndroidJavaObject curActivity;
    public string strLog = "No Java Log";
    static PluginWrapper _instance;

    public Text myText;

    public static PluginWrapper GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("PluginWrapper").AddComponent<PluginWrapper>();
        }
        return _instance;
    }

    void Awake()
    {
        #if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        curActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        #endif

        myText = (Text)GameObject.Find("Canvas/Text").GetComponent<Text>();
        myText.text = strLog;
    }
    // Use this for initialization
    void Start()
    {
        //SetText("START");
        InvokeRepeating("repeatCall", 0.1f, 0.1f);
        //PluginWrapper.GetInstance().CallJavaFunc("javaTestFunc", "UnityJavaJarTet");
        Debug.Log("GO");
    }

    public void CallJavaFunc(string strFuncName, string strTemp)
    {
        if (curActivity == null)
        {
            strLog = curActivity + " is null";
            return;
        }

        strLog = "Before call" + strFuncName;

        curActivity.Call(strFuncName, strTemp);
        strLog = strFuncName + "is Called with param " + strTemp;
    }

    void SetJavaLog(string strJavaLog)
    {
        strLog = strJavaLog;
        myText.text = strLog;
    }

    void repeatCall()
    {
        PluginWrapper.GetInstance().CallJavaFunc("javaTestFunc", "UnityJavaJarTet");
    }
    // Update is called once per frame
    void Update()
    {
        PluginWrapper.GetInstance().CallJavaFunc("javaTestFunc", "UnityJavaJarTet");
    }
    void DefaultText()
    {
        SetText("DEFAULT");
    }
    public void SetText(string text)
    {
        myText.text = text;
    }
}
