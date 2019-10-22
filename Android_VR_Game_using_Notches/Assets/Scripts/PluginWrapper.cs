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
    private Rigidbody rb_leftUpperArm;
    private Rigidbody rb_leftForeArm;
    private Rigidbody rb_rightUpperArm;
    private Rigidbody rb_rightForeArm;

    public float rotationSpeed = 5f;
    private float angleX_f;
    private float angleY_f;
    private float angleZ_f;

    private float leftUpperArmAngleX_f;
    private float leftUpperArmAngleY_f;
    private float leftUpperArmAngleZ_f;

    private float leftForeArmAngleX_f;
    private float leftForeArmAngleY_f;
    private float leftForeArmAngleZ_f;

    private float rightUpperArmAngleX_f;
    private float rightUpperArmAngleY_f;
    private float rightUpperArmAngleZ_f;

    private float rightForeArmAngleX_f;
    private float rightForeArmAngleY_f;
    private float rightForeArmAngleZ_f;

    private Vector3 inputRotation;
    private Vector3 leftUpperArm_inputRotation;
    private Vector3 leftForeArm_inputRotation;
    private Vector3 rightUpperArm_inputRotation;
    private Vector3 rightForeArm_inputRotation;

    private PlayerManager playerManager;
    private GameObject player;

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
        playerManager = (PlayerManager)GameObject.Find("ZombiePlayer_Spawn_Postion").GetComponent<PlayerManager>();
        Debug.Log(playerManager.GetPlayerRespawned());
        /*player = playerManager.GetPlayerGameObject();
        Transform trans = player.GetComponent<Transform>();
        rb = (Rigidbody)trans.Find("chest").GetComponent<Rigidbody>();
        Debug.Log(rb);*/
        //Rigidbody rigidbody = go.Find("chest").GetComponent<Rigidbody>();
        //rb = (Rigidbody) GameObject.Find("ZombieRig (2)/rig/hips/spine/chest").GetComponent<Rigidbody>();
        //rb = (Rigidbody)GameObject.Find("ZombiePlayer_Spawn_Postion/Zombie_Player(Clone)/rig/hips/spine/chest").GetComponent<Rigidbody>();

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
        //rb = (Rigidbody)GameObject.Find("ZombiePlayer_Spawn_Postion/Zombie_Player(Clone)/rig/hips/spine/chest").GetComponent<Rigidbody>();
        //SetText("START");
        //InvokeRepeating("repeatCall", 0.1f, 0.1f);
        PluginWrapper.GetInstance().CallJavaFunc("javaTestFunc", "UnityJavaJarTest");
        PluginWrapper.GetInstance().CallJavaFunc("javaGetCoordFunc", "UnityJavaJarTest");
        Debug.Log("GO");
        Debug.Log(rb);
        //rotationSpeed = 50f; 
        Debug.Log(rotationSpeed);
        //rb = GetComponent<Rigidbody>();
        /*if (GameObject.Find("ZombiePlayer_Spawn_Postion/Zombie_Player(Clone)"))
            rb = (Rigidbody)GameObject.Find("ZombiePlayer_Spawn_Postion/Zombie_Player(Clone)/rig/hips/spine/chest").GetComponent<Rigidbody>();
        Debug.Log(rb);*/
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
        if (GameObject.Find("ZombiePlayer_Spawn_Postion/Zombie_Player(Clone)") && playerManager.GetPlayerRespawned())
        {
            Debug.Log("PluginWrapper Update rb meghivva");
            rb = (Rigidbody)GameObject.Find("ZombiePlayer_Spawn_Postion/Zombie_Player(Clone)/rig/hips/spine/chest").GetComponent<Rigidbody>();
            rb_leftUpperArm = (Rigidbody)GameObject.Find("ZombiePlayer_Spawn_Postion/Zombie_Player(Clone)/rig/hips/spine/chest/shoulder.L/upper_arm.L").GetComponent<Rigidbody>();
            rb_leftForeArm = (Rigidbody)GameObject.Find("ZombiePlayer_Spawn_Postion/Zombie_Player(Clone)/rig/hips/spine/chest/shoulder.L/forearm.L").GetComponent<Rigidbody>();
            rb_rightUpperArm = (Rigidbody)GameObject.Find("ZombiePlayer_Spawn_Postion/Zombie_Player(Clone)/rig/hips/spine/chest/shoulder.R/upper_arm.R").GetComponent<Rigidbody>();
            rb_rightForeArm = (Rigidbody)GameObject.Find("ZombiePlayer_Spawn_Postion/Zombie_Player(Clone)/rig/hips/spine/chest/shoulder.R/forearm.R").GetComponent<Rigidbody>();
            playerManager.SetPlayerRespawned(false);
        }
    }

    private void FixedUpdate()
    {
        inputRotation = new Vector3(angleX_f, angleY_f, -angleZ_f);
        leftUpperArm_inputRotation = new Vector3(leftUpperArmAngleX_f, leftUpperArmAngleY_f, -leftUpperArmAngleZ_f);
        leftForeArm_inputRotation = new Vector3(leftForeArmAngleX_f, leftForeArmAngleY_f, -leftForeArmAngleZ_f);
        rightUpperArm_inputRotation = new Vector3(rightUpperArmAngleX_f, rightUpperArmAngleY_f, -rightUpperArmAngleZ_f);
        rightForeArm_inputRotation = new Vector3(rightForeArmAngleX_f, rightForeArmAngleY_f, -rightForeArmAngleZ_f);
        /*Quaternion deltaRotation = Quaternion.Euler(inputRotation * Time.deltaTime * rotationSpeed);
        rb.MoveRotation(rb.rotation * deltaRotation);*/
        if (rb)
            rb.transform.Rotate(inputRotation * Time.deltaTime/* * rotationSpeed*/);
        if (rb_leftUpperArm)
            rb_leftUpperArm.transform.Rotate(leftUpperArm_inputRotation * Time.deltaTime/* * rotationSpeed*/);
        if (rb_leftForeArm)
            rb_leftForeArm.transform.Rotate(leftForeArm_inputRotation * Time.deltaTime/* * rotationSpeed*/);
        if (rb_rightUpperArm)
            rb_rightUpperArm.transform.Rotate(rightUpperArm_inputRotation * Time.deltaTime/* * rotationSpeed*/);
        if (rb_rightForeArm)
            rb_rightForeArm.transform.Rotate(rightForeArm_inputRotation * Time.deltaTime/* * rotationSpeed*/);
        //Debug.Log(rb);
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
        angleX_f = float.Parse(x, CultureInfo.InvariantCulture);
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
    /** LEFT UPPER ARM **/
    public void SetLeftUpperArmAngleX(string x)
    {
        leftUpperArmAngleX_f = float.Parse(x, CultureInfo.InvariantCulture);
    }

    public void SetLeftUpperArmAngleY(string y)
    {
        leftUpperArmAngleY_f = float.Parse(y, CultureInfo.InvariantCulture);
    }

    public void SetLeftUpperArmAngleZ(string z)
    {
        leftUpperArmAngleZ_f = float.Parse(z, CultureInfo.InvariantCulture);
    }

    public float getLeftUpperArmAngleX()
    {
        return leftUpperArmAngleX_f;
    }

    public float getLeftUpperArmAngleY()
    {
        return leftUpperArmAngleY_f;
    }

    public float getLeftUpperArmAngleZ()
    {
        return leftUpperArmAngleZ_f;
    }
    /** LEFT FOREARM **/
    public void SetLeftForeArmAngleX(string x)
    {
        leftForeArmAngleX_f = float.Parse(x, CultureInfo.InvariantCulture);
    }

    public void SetLeftForeArmAngleY(string y)
    {
        leftForeArmAngleY_f = float.Parse(y, CultureInfo.InvariantCulture);
    }

    public void SetForeForeArmAngleZ(string z)
    {
        leftForeArmAngleZ_f = float.Parse(z, CultureInfo.InvariantCulture);
    }

    public float getLeftForeArmAngleX()
    {
        return leftForeArmAngleX_f;
    }

    public float getLeftForeArmAngleY()
    {
        return leftForeArmAngleY_f;
    }

    public float getLeftForeArmAngleZ()
    {
        return leftForeArmAngleZ_f;
    }

    /** RIGHT UPPER ARM **/
    public void SetRightUpperArmAngleX(string x)
    {
        rightUpperArmAngleX_f = float.Parse(x, CultureInfo.InvariantCulture);
    }

    public void SetRightUpperArmAngleY(string y)
    {
        rightUpperArmAngleY_f = float.Parse(y, CultureInfo.InvariantCulture);
    }

    public void SetRightUpperArmAngleZ(string z)
    {
        rightUpperArmAngleZ_f = float.Parse(z, CultureInfo.InvariantCulture);
    }

    public float getRightUpperArmAngleX()
    {
        return rightUpperArmAngleX_f;
    }

    public float getRightUpperArmAngleY()
    {
        return rightUpperArmAngleY_f;
    }

    public float getRightUpperArmAngleZ()
    {
        return rightUpperArmAngleZ_f;
    }
    /** RIGHT FOREARM **/
    public void SetRightForeArmAngleX(string x)
    {
        rightForeArmAngleX_f = float.Parse(x, CultureInfo.InvariantCulture);
    }

    public void SetRightForeArmAngleY(string y)
    {
        rightForeArmAngleY_f = float.Parse(y, CultureInfo.InvariantCulture);
    }

    public void SetRightForeArmAngleZ(string z)
    {
        rightForeArmAngleZ_f = float.Parse(z, CultureInfo.InvariantCulture);
    }

    public float getRightForeArmAngleX()
    {
        return rightForeArmAngleX_f;
    }

    public float getRightForeArmAngleY()
    {
        return rightForeArmAngleY_f;
    }

    public float getRightForeArmAngleZ()
    {
        return rightForeArmAngleZ_f;
    }

}
