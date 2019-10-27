using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float rotationSpeed;

    private float maxRotationSpeed = 100f;
    private Vector3 input;
    public Vector3 inputRotation;
    private Rigidbody rb;

    public Vector2 xCon = new Vector2(0f, 130f);
    Vector3 wantaBeRot;

    private Vector3 spawn;

    static PlayerRotation _instance;

    

    public static PlayerRotation GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("PlayerRotation").AddComponent<PlayerRotation>();
        }
        return _instance;
    }

    //Used for testing purposes. GetInstance works properly only if rb is set this way in the Awake method and not in the start Method
    /*void Awake()
    {
        rb = (Rigidbody)GameObject.Find("Player").GetComponent<Rigidbody>();
    }*/
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        //Debug.Log(rb);
        //inputRotation = new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
    }

    SoftJointLimit limit;
    private float minChestRotZ = 0f;
    private float maxChestRotZ = 60f;
    private float chestRotX;
    private float chestRotY;
    private float chestRotZ;

    private void FixedUpdate()
    {

        //rb.transform.rotation = new Quaternion(Mathf.Clamp(wantaBeRot.x, xCon.x, xCon.y), rb.transform.rotation.y, rb.transform.rotation.z, rb.transform.rotation.w);
        //rb.transform.Rotate(Mathf.Clamp(inputRotation.x * Time.deltaTime * rotationSpeed, 0, 130), inputRotation.y * Time.deltaTime * rotationSpeed, inputRotation.z * Time.deltaTime * rotationSpeed);
        if (rb.name == "chest")
        {
            chestRotX = rb.transform.rotation.eulerAngles.x;
            chestRotY = rb.transform.rotation.eulerAngles.y;
            chestRotZ = rb.transform.rotation.eulerAngles.z;
            //Debug.Log(rb.transform.localEulerAngles);

            if ((rb.transform.localEulerAngles.y >= 280f || rb.transform.localEulerAngles.y <= 80f) 
                && (rb.transform.localEulerAngles.x >= 300f || rb.transform.localEulerAngles.x <= 60f) 
                && (rb.transform.localEulerAngles.z >= 300f || rb.transform.localEulerAngles.z <= 60f))
            {
                inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), 0.0f, -Input.GetAxisRaw("Horizontal"));
            }
            else if (rb.transform.localEulerAngles.y < 280f && inputRotation.y < 0)
            {
                rb.transform.localEulerAngles = new Vector3(rb.transform.localEulerAngles.x, rb.transform.localEulerAngles.y+1f, rb.transform.localEulerAngles.z);
                inputRotation = new Vector3(0.0f, 0, 0.0f);
            }
            else if (rb.transform.localEulerAngles.y > 80f && inputRotation.y > 0)
            {
                rb.transform.localEulerAngles = new Vector3(rb.transform.localEulerAngles.x, rb.transform.localEulerAngles.y-1f, rb.transform.localEulerAngles.z);
                inputRotation = new Vector3(0.0f, 0, 0.0f);
            }
            else if (rb.transform.localEulerAngles.x < 300f && inputRotation.x < 0)
            {
                rb.transform.localEulerAngles = new Vector3(rb.transform.localEulerAngles.x+1f, rb.transform.localEulerAngles.y, rb.transform.localEulerAngles.z);
                inputRotation = new Vector3(0.0f, 0, 0.0f);
            }
            else if (rb.transform.localEulerAngles.x > 60f && inputRotation.x > 0)
            {
                rb.transform.localEulerAngles = new Vector3(rb.transform.localEulerAngles.x-1f, rb.transform.localEulerAngles.y, rb.transform.localEulerAngles.z);
                inputRotation = new Vector3(0.0f, 0, 0.0f);
            }
            else if (rb.transform.localEulerAngles.z < 300f && inputRotation.z < 0)
            {
                rb.transform.localEulerAngles = new Vector3(rb.transform.localEulerAngles.x, rb.transform.localEulerAngles.y, rb.transform.localEulerAngles.z+1f);
                inputRotation = new Vector3(0.0f, 0, 0.0f);
            }
            else if (rb.transform.localEulerAngles.z > 60f && inputRotation.z > 0)
            {
                rb.transform.localEulerAngles = new Vector3(rb.transform.localEulerAngles.x, rb.transform.localEulerAngles.y, rb.transform.localEulerAngles.z-1f);
                inputRotation = new Vector3(0.0f, 0, 0.0f);
            }
            else
                inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), 0.0f, -Input.GetAxisRaw("Horizontal"));

        }
        Quaternion rotationY = Quaternion.AngleAxis(inputRotation.y * Time.deltaTime * rotationSpeed, new Vector3(0f, 1f, 0f));
        Quaternion rotationX = Quaternion.AngleAxis(inputRotation.x * Time.deltaTime * rotationSpeed, new Vector3(1f, 0f, 0f));
        Quaternion rotationZ = Quaternion.AngleAxis(inputRotation.z * Time.deltaTime * rotationSpeed, new Vector3(0f, 0f, 1f));
        rb.transform.localRotation = rb.transform.localRotation * rotationX * rotationY * rotationZ;
        //rb.transform.Rotate(inputRotation * Time.deltaTime * rotationSpeed);

        //Debug.Log(rb.position);

    }

    // Update is called once per frame
    void Update()
    {
        if (rb.name == "upper_arm.L")
            inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), 0.0f, -Input.GetAxisRaw("Horizontal"));
            //inputRotation = new Vector3( 0.0f, -Input.GetAxisRaw("Vertical"), -Input.GetAxisRaw("Horizontal"));
        if (rb.name == "upper_arm.R")
            inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), 0.0f, Input.GetAxisRaw("Horizontal"));
            //inputRotation = new Vector3(0.0f, Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        if (rb.name == "forearm.L")
            if(rb.transform.rotation.z > -60f && rb.transform.rotation.z < +60f)
                inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), 0.0f, -Input.GetAxisRaw("Horizontal"));
            else
                inputRotation = new Vector3(0.0f, 0.0f, -Input.GetAxisRaw("Horizontal"));

        //inputRotation = new Vector3( 0.0f, -Input.GetAxisRaw("Vertical"), -Input.GetAxisRaw("Horizontal"));
        if (rb.name == "forearm.R")
            inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), 0.0f, Input.GetAxisRaw("Horizontal"));
            //inputRotation = new Vector3(0.0f, Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        //inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"), 0.0f);

    }

    //Used only for testing purposes, checked if GetInstance is working
    public Quaternion GetPosition()
    {
        return rb.rotation;
    }

    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }

    public void SetRotationSpeed(float newRotationSpeed)
    {
        rotationSpeed = newRotationSpeed;
    }

}
