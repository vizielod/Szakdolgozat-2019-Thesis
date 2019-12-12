using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PluginWrapper : MonoBehaviour
{
    private static float LEFTUPPERARM_STARTPOS_DEVIATION_X = 0;
    private static float LEFTUPPERARM_STARTPOS_DEVIATION_Y = +280f;
    private static float LEFTUPPERARM_STARTPOS_DEVIATION_Z = +85f;

    private static float RIGHTUPPERARM_STARTPOS_DEVIATION_X = -10f;
    private static float RIGHTUPPERARM_STARTPOS_DEVIATION_Y = +80f;
    private static float RIGHTUPPERARM_STARTPOS_DEVIATION_Z = -80f;


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
    private float chestAngleX_f;
    private float chestAngleY_f;
    private float chestAngleZ_f;

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

    public Vector3 currentRotation;
    public Vector3 inputChestRotation;
    public Vector3 inputLeftUpperArmRotation;
    private Vector3 leftUpperArm_inputRotation;
    private Vector3 leftForeArm_inputRotation;
    private Vector3 rightUpperArm_inputRotation;
    private Vector3 rightForeArm_inputRotation;

    private PlayerManager playerManager;
    public int playerLifeAmount;
    private CollisionDetection collisionDetection;
    private GameObject player;

    private SceneChanger sceneChanger;

    public void ZombiePlayerDefaultSetup()
    {
        chestAngleX_f = -16.791f;
        chestAngleY_f = 0;
        chestAngleZ_f = 0;

        leftUpperArmAngleX_f = 30.102f;
        leftUpperArmAngleY_f = -89.846f;
        leftUpperArmAngleZ_f = 35.237f;

        leftForeArmAngleX_f = 94.272f;
        leftForeArmAngleY_f = -2.334f;
        leftForeArmAngleZ_f = -2.256f;

        rightUpperArmAngleX_f = 30.102f;
        rightUpperArmAngleY_f = 89.846f;
        rightUpperArmAngleZ_f = -35.237f;

        rightForeArmAngleX_f = 24.272f;
        rightForeArmAngleY_f = 2.334f;
        rightForeArmAngleZ_f = 2.256f;

}
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
        playerLifeAmount = playerManager.GetPlayerLifeAmount();

        sceneChanger = (SceneChanger)GameObject.Find("SceneManager").GetComponent<SceneChanger>();
        myText = (Text)GameObject.Find("Canvas/Text").GetComponent<Text>();
        //myText.text = strLog;
        /*chestX = (Text)GameObject.Find("Canvas/ChestX").GetComponent<Text>();
        chestY = (Text)GameObject.Find("Canvas/ChestY").GetComponent<Text>();
        chestZ = (Text)GameObject.Find("Canvas/ChestZ").GetComponent<Text>();
        chestX.text = "chestX";
        chestY.text = "chestY";
        chestZ.text = "chestZ";*/
        //rb = GetComponent<Rigidbody>();
    }
    // Use this for initialization
    void Start()
    {
        //rb = (Rigidbody)GameObject.Find("ZombiePlayer_Spawn_Postion/Zombie_Player(Clone)/rig/hips/spine/chest").GetComponent<Rigidbody>();
        SetText("START");
        //InvokeRepeating("repeatCall", 0.1f, 0.1f);
        PluginWrapper.GetInstance().CallJavaFunc("javaTestFunc", "UnityJavaJarTest");
        PluginWrapper.GetInstance().CallJavaFunc("javaGetCoordFunc", "UnityJavaJarTest");
        if (GameObject.Find("Wall_spawner").GetComponentInChildren<CollisionDetection>())
        {
            collisionDetection = (CollisionDetection)GameObject.Find("Wall_spawner").GetComponentInChildren<CollisionDetection>();
        }

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
        playerLifeAmount = playerManager.GetPlayerLifeAmount();
        Debug.Log(playerLifeAmount);
        PluginWrapper.GetInstance().CallJavaFunc("javaTestFunc", "UnityJavaJarTest");
        PluginWrapper.GetInstance().CallJavaFunc("javaGetCoordFunc", "UnityJavaJarTest");
        #if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //#if UNITY_ANDROID
            Application.Quit();
            //#endif
        }
        #endif
        if (GameObject.Find("ZombiePlayer_Spawn_Postion/1VRView_Zombie_Player(Clone)") && playerManager.GetPlayerRespawned() && playerLifeAmount > 0)
        {
            
            if (GameObject.Find("Wall_spawner").GetComponentInChildren<CollisionDetection>())
            {
                collisionDetection = (CollisionDetection)GameObject.Find("Wall_spawner").GetComponentInChildren<CollisionDetection>();
            }
            Debug.Log("PluginWrapper Update rb meghivva");
            rb = (Rigidbody)GameObject.Find("ZombiePlayer_Spawn_Postion/1VRView_Zombie_Player(Clone)/rig/hips/spine/chest").GetComponent<Rigidbody>();
            Debug.Log(rb.transform.localEulerAngles);
            rb_leftUpperArm = (Rigidbody)GameObject.Find("ZombiePlayer_Spawn_Postion/1VRView_Zombie_Player(Clone)/rig/hips/spine/chest/shoulder.L/upper_arm.L").GetComponent<Rigidbody>();
            //Debug.Log(rb_leftUpperArm.transform.localEulerAngles);
            //inputLeftUpperArmRotation = rb_leftUpperArm.transform.localEulerAngles;
            //Debug.Log(inputLeftUpperArmRotation);
            rb_leftForeArm = (Rigidbody)GameObject.Find("ZombiePlayer_Spawn_Postion/1VRView_Zombie_Player(Clone)/rig/hips/spine/chest/shoulder.L/upper_arm.L/forearm.L").GetComponent<Rigidbody>();

            rb_rightUpperArm = (Rigidbody)GameObject.Find("ZombiePlayer_Spawn_Postion/1VRView_Zombie_Player(Clone)/rig/hips/spine/chest/shoulder.R/upper_arm.R").GetComponent<Rigidbody>();
            
            rb_rightForeArm = (Rigidbody)GameObject.Find("ZombiePlayer_Spawn_Postion/1VRView_Zombie_Player(Clone)/rig/hips/spine/chest/shoulder.R/upper_arm.R/forearm.R").GetComponent<Rigidbody>();

            playerManager.SetPlayerRespawned(false);
            //ZombiePlayerDefaultSetup();
        }
    }
    private void FixedUpdate()
    {

        /*inputRotation = new Vector3(angleX_f, angleY_f, -angleZ_f);
        leftUpperArm_inputRotation = new Vector3(leftUpperArmAngleX_f, -leftUpperArmAngleY_f, -leftUpperArmAngleZ_f);
        leftForeArm_inputRotation = new Vector3(leftForeArmAngleX_f, -leftForeArmAngleY_f, -leftForeArmAngleZ_f);
        rightUpperArm_inputRotation = new Vector3(rightUpperArmAngleX_f, rightUpperArmAngleY_f, rightUpperArmAngleZ_f);
        rightForeArm_inputRotation = new Vector3(rightForeArmAngleX_f, rightForeArmAngleY_f, rightForeArmAngleZ_f);*/
        if (!collisionDetection.IsRagdoll())//ide kéne még egy feltétel, ha nincs is collisionDetection akkor ne lépjen bele ebbe vagy valami ilytesmi
        {
            /** CHEST **/
            if (rb)
            {
                inputChestRotation = new Vector3(chestAngleX_f % 360, -chestAngleY_f % 360, -chestAngleZ_f % 360);
                //inputChestRotation = new Vector3(inputChestRotation.x % 360, inputChestRotation.y % 360, inputChestRotation.z % 360);
                Quaternion rotationY = Quaternion.AngleAxis(inputChestRotation.y, new Vector3(0f, 1f, 0f));
                Quaternion rotationX = Quaternion.AngleAxis(inputChestRotation.x, new Vector3(1f, 0f, 0f));
                Quaternion rotationZ = Quaternion.AngleAxis(inputChestRotation.z, new Vector3(0f, 0f, 1f));
                rb.transform.localRotation = rotationX * rotationY * rotationZ;
            }

            /** LEFT UPPER ARM **/
            if (rb_leftUpperArm)
            {
                leftUpperArm_inputRotation = new Vector3(
                    leftUpperArmAngleX_f + LEFTUPPERARM_STARTPOS_DEVIATION_X % 360, 
                    -leftUpperArmAngleY_f + LEFTUPPERARM_STARTPOS_DEVIATION_Y % 360, 
                    -leftUpperArmAngleZ_f + LEFTUPPERARM_STARTPOS_DEVIATION_Z % 360
                    );
                /*leftUpperArm_inputRotation = new Vector3(
                -leftUpperArmAngleX_f  % 360,
                -leftUpperArmAngleY_f  % 360,
                -leftUpperArmAngleZ_f  % 360
                );*/
                //leftUpperArm_inputRotation = new Vector3(inputLeftUpperArmRotation.x % 360, inputLeftUpperArmRotation.y % 360, -inputLeftUpperArmRotation.z % 360);
                //leftUpperArm_inputRotation = new Vector3(leftUpperArmAngleX_f % 360, -leftUpperArmAngleY_f % 360, -leftUpperArmAngleZ_f % 360);
                Quaternion rotationY = Quaternion.AngleAxis(leftUpperArm_inputRotation.y, new Vector3(0f, 1f, 0f));
                Quaternion rotationX = Quaternion.AngleAxis(leftUpperArm_inputRotation.x, new Vector3(1f, 0f, 0f));
                Quaternion rotationZ = Quaternion.AngleAxis(leftUpperArm_inputRotation.z, new Vector3(0f, 0f, 1f));
                rb_leftUpperArm.transform.localRotation = rotationX * rotationY * rotationZ;
            }

            /** LEFT FOREARM **/
            if (rb_leftForeArm)
            {
                leftForeArm_inputRotation = new Vector3(-leftForeArmAngleX_f % 360, -leftForeArmAngleY_f % 360, -leftForeArmAngleZ_f % 360);
                Quaternion rotationY = Quaternion.AngleAxis(leftForeArm_inputRotation.y, new Vector3(0f, 1f, 0f));
                Quaternion rotationX = Quaternion.AngleAxis(leftForeArm_inputRotation.x, new Vector3(1f, 0f, 0f));
                Quaternion rotationZ = Quaternion.AngleAxis(leftForeArm_inputRotation.z, new Vector3(0f, 0f, 1f));
                rb_leftForeArm.transform.localRotation = rotationX * rotationY * rotationZ;
            }

            /** RIGHT UPPER ARM **/
            if (rb_rightUpperArm)
            {
                rightUpperArm_inputRotation = new Vector3(
                    rightUpperArmAngleX_f + RIGHTUPPERARM_STARTPOS_DEVIATION_X % 360,
                    rightUpperArmAngleY_f + RIGHTUPPERARM_STARTPOS_DEVIATION_Y % 360,
                    rightUpperArmAngleZ_f + RIGHTUPPERARM_STARTPOS_DEVIATION_Z % 360
                    );
                /*rightUpperArm_inputRotation = new Vector3(
                    rightUpperArmAngleX_f  % 360,
                    rightUpperArmAngleY_f  % 360,
                    rightUpperArmAngleZ_f  % 360
                    );*/
                //rightForeArm_inputRotation = new Vector3(rightUpperArmAngleX_f % 360, rightUpperArmAngleY_f % 360, rightUpperArmAngleZ_f % 360);
                Quaternion rotationY = Quaternion.AngleAxis(rightUpperArm_inputRotation.y, new Vector3(0f, 1f, 0f));
                Quaternion rotationX = Quaternion.AngleAxis(rightUpperArm_inputRotation.x, new Vector3(1f, 0f, 0f));
                Quaternion rotationZ = Quaternion.AngleAxis(rightUpperArm_inputRotation.z, new Vector3(0f, 0f, 1f));
                rb_rightUpperArm.transform.localRotation = rotationX * rotationY * rotationZ;
            }

            /** RIGHT FOREARM **/
            if (rb_rightForeArm)
            {
                rightForeArm_inputRotation = new Vector3(rightForeArmAngleX_f % 360, rightForeArmAngleY_f % 360, rightForeArmAngleZ_f % 360);
                Quaternion rotationY = Quaternion.AngleAxis(rightForeArm_inputRotation.y, new Vector3(0f, 1f, 0f));
                Quaternion rotationX = Quaternion.AngleAxis(rightForeArm_inputRotation.x, new Vector3(1f, 0f, 0f));
                Quaternion rotationZ = Quaternion.AngleAxis(rightForeArm_inputRotation.z, new Vector3(0f, 0f, 1f));
                rb_rightForeArm.transform.localRotation = rotationX * rotationY * rotationZ;
            }
        }
        
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
        chestAngleX_f = float.Parse(x, CultureInfo.InvariantCulture);
    }

    public void SetAngleY(string y)
    {
        chestY.text = y;
        chestAngleY_f = float.Parse(y, CultureInfo.InvariantCulture);
    }

    public void SetAngleZ(string z)
    {
        chestZ.text = z;
        chestAngleZ_f = float.Parse(z, CultureInfo.InvariantCulture);
    }

    public float getAngleX()
    {
        return chestAngleX_f;
    }

    public float getAngleY()
    {
        return chestAngleY_f;
    }

    public float getAngleZ()
    {
        return chestAngleZ_f;
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
