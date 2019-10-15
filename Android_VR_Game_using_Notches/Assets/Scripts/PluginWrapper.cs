using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PluginWrapper : MonoBehaviour
{
    private AndroidJavaObject curActivity;
    public string strLog = "No Java Log";
    static PluginWrapper _instance;

    public Text myText;
    public Text chestX;
    public Text chestY;
    public Text chestZ;

    private Rigidbody rb;
    public float rotationSpeed = 5f;
    private float angleX_f;
    private float angleY_f;
    private float angleZ_f;
    private Vector3 inputRotation;


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

        rb = (Rigidbody) GameObject.Find("ZombieRig (2)/rig/hips/spine/chest").GetComponent<Rigidbody>();

        myText = (Text)GameObject.Find("Canvas/Text").GetComponent<Text>();
        chestX = (Text)GameObject.Find("Canvas/ChestX").GetComponent<Text>();
        chestY = (Text)GameObject.Find("Canvas/ChestY").GetComponent<Text>();
        chestZ = (Text)GameObject.Find("Canvas/ChestZ").GetComponent<Text>();
        myText.text = strLog;
        chestX.text = "chestX";
        chestY.text = "chestY";
        chestZ.text = "chestZ";
        //rb = GetComponent<Rigidbody>();

    }
    // Use this for initialization
    void Start()
    {
        //SetText("START");
        //InvokeRepeating("repeatCall", 0.1f, 0.1f);
        PluginWrapper.GetInstance().CallJavaFunc("javaTestFunc", "UnityJavaJarTest");
        PluginWrapper.GetInstance().CallJavaFunc("javaGetCoordFunc", "UnityJavaJarTest");
        Debug.Log("GO");
        Debug.Log(rb);
        //rotationSpeed = 50f; 
        Debug.Log(rotationSpeed);
        //rb = GetComponent<Rigidbody>();
        
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
        PluginWrapper.GetInstance().CallJavaFunc("javaTestFunc", "UnityJavaJarTest");
        PluginWrapper.GetInstance().CallJavaFunc("javaGetCoordFunc", "UnityJavaJarTest");
    }
    // Update is called once per frame
    void Update()
    {
        PluginWrapper.GetInstance().CallJavaFunc("javaTestFunc", "UnityJavaJarTest");
        PluginWrapper.GetInstance().CallJavaFunc("javaGetCoordFunc", "UnityJavaJarTest");
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void FixedUpdate()
    {
        inputRotation = new Vector3(angleX_f, angleY_f, -angleZ_f);
        /*Quaternion deltaRotation = Quaternion.Euler(inputRotation * Time.deltaTime * rotationSpeed);
        rb.MoveRotation(rb.rotation * deltaRotation);*/
        rb.transform.Rotate(inputRotation * Time.deltaTime/* * rotationSpeed*/);
    }

    void DefaultText()
    {
        SetText("DEFAULT");
    }
    public void SetText(string text)
    {
        myText.text = text;
    }
    public void SetAngleX(string x)
    {
        chestX.text = x;
        angleX_f  = float.Parse(x, CultureInfo.InvariantCulture);
    }

    public void SetAngleY(string y)
    {
        chestY.text = y;
        angleY_f = float.Parse(y, CultureInfo.InvariantCulture);
    }

    public void SetAngleZ(string z)
    {
        chestZ.text = z;
        angleZ_f = float.Parse(z, CultureInfo.InvariantCulture);
    }

    public float getAngleX()
    {
        return angleX_f;
    }

    public float getAngleY()
    {
        return angleY_f;
    }

    public float getAngleZ()
    {
        return angleZ_f;
    }


}
