using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PluginAssist : MonoBehaviour
{

    private AndroidJavaObject curActivity;
    public string strLog = "No Java Log";
    static PluginAssist _instance;

    private PlayerManager playerManagerComponent;

    public Text myText;

    public static PluginAssist GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("PluginAssist").AddComponent<PluginAssist>();
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
        //playerManagerComponent = (PlayerManager)GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        //playerManagerComponent.SetPlayerLifeAmount(playerManagerComponent.GetDefaultLifeAmount());
        //Debug.Log(playerManagerComponent.GetPlayerLifeAmount());
    }
    // Use this for initialization
    void Start()
    {
        //rb = (Rigidbody)GameObject.Find("ZombiePlayer_Spawn_Postion/Zombie_Player(Clone)/rig/hips/spine/chest").GetComponent<Rigidbody>();
        SetText("START");
        //InvokeRepeating("repeatCall", 0.1f, 0.1f);
        //myText = (Text)GameObject.Find("Canvas/Text").GetComponent<Text>();
        PluginAssist.GetInstance().CallJavaFunc("javaTestFunc", "UnityJavaJarTest");
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
        PluginAssist.GetInstance().CallJavaFunc("javaTestFunc", "UnityJavaJarTest");
    }
    // Update is called once per frame
    void Update()
    {
        PluginAssist.GetInstance().CallJavaFunc("javaTestFunc", "UnityJavaJarTest");
        #if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //#if UNITY_ANDROID
            Application.Quit();
            //#endif
        }
        #endif        
    }

    public void SetText(string text)
    {
        myText.text = text;
    }

}
