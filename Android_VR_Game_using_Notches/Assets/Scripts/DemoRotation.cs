using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/** 
 * USED THIS SCRIPT FOR TESTING PURPOSES
 * Here I was learning how do the followings work and what are the differences between them:
 * EulerAngles, localEulerAngles
 * Rotation, localRotation
 * Quaternion, AngleAxis.
 * Before I used this knowledge to try it on the Character using the PlayerRotation script,
 * I was testing everything on a single Cube.
 * I was learning all these from a Youtube channel called: Chidre'sTechTutorials,
 * watching his Unity Transform Essentials series: 
 * https://www.youtube.com/watch?v=UST0SwYGwjs&list=PLdE8ESr9Th_s2iBzpSHck5ICInQWAKRyw
 * **/

public class DemoRotation : MonoBehaviour
{
    public float rotationSpeed;

    public Vector3 inputRotation;
    public Vector3 currentRotation;
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

    // Start is called before the first frame update
    void Start()
    {
        /*currentRotation = new Vector3(
            currentRotation.x % 360,
            currentRotation.y % 360,
            currentRotation.z % 360
            );
        Quaternion rotationY = Quaternion.AngleAxis(currentRotation.y * Time.deltaTime, new Vector3(0f, 1f, 0f));
        Quaternion rotationX = Quaternion.AngleAxis(currentRotation.x * Time.deltaTime, new Vector3(1f, 0f, 0f));
        Quaternion rotationZ = Quaternion.AngleAxis(currentRotation.z * Time.deltaTime, new Vector3(0f, 0f, 1f));*/
        //transform.localRotation = rotationX * rotationY * rotationZ;
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), 0.0f, -Input.GetAxisRaw("Horizontal"));
        /*Quaternion rotationY = Quaternion.AngleAxis(inputRotation.y * Time.deltaTime, new Vector3(0f, 1f, 0f));
        Quaternion rotationX = Quaternion.AngleAxis(inputRotation.x * Time.deltaTime, new Vector3(1f, 0f, 0f));
        Quaternion rotationZ = Quaternion.AngleAxis(inputRotation.z * Time.deltaTime, new Vector3(0f, 0f, 1f));

        if (transform.eulerAngles.z < 60)
        {
            transform.localRotation = rotationX * rotationY * rotationZ * transform.localRotation;

            currentRotation = currentRotation + inputRotation * Time.deltaTime;
            currentRotation = new Vector3(currentRotation.x % 360, currentRotation.y % 360, currentRotation.z % 360);
        }*/
        /*else
            transform.localRotation = transform.localRotation;*/
    }
    private void FixedUpdate()
    {
        inputRotation = new Vector3(inputRotation.x % 360, inputRotation.y % 360, inputRotation.z % 360);
        Quaternion rotationY = Quaternion.AngleAxis(inputRotation.y/* * Time.deltaTime*/, new Vector3(0f, 1f, 0f));
        Quaternion rotationX = Quaternion.AngleAxis(inputRotation.x/* * Time.deltaTime*/, new Vector3(1f, 0f, 0f));
        Quaternion rotationZ = Quaternion.AngleAxis(inputRotation.z/* * Time.deltaTime*/, new Vector3(0f, 0f, 1f));
        transform.localRotation = rotationX * rotationY * rotationZ;
        /*if (inputRotation.x > 0)
        {
            if (currentRotation.x <= inputRotation.x)
            {
                transform.localRotation = rotationX;

                currentRotation = currentRotation + inputRotation * Time.deltaTime;
                currentRotation = new Vector3(currentRotation.x % 360, currentRotation.y % 360, currentRotation.z % 360);
            }
        }
        if (inputRotation.x <= 0)
        {
            if (currentRotation.x  > inputRotation.x)
            {
                transform.localRotation = rotationX * rotationY * rotationZ * transform.localRotation;

                currentRotation = currentRotation + inputRotation * Time.deltaTime;
                currentRotation = new Vector3(currentRotation.x % 360, currentRotation.y % 360, currentRotation.z % 360);
            }
        }*/

    }
    /*private void FixedUpdate()
    {

        if (rb.name == "chest")
        {
            Debug.Log(rb.transform.localEulerAngles);

            if ((rb.transform.localEulerAngles.y >= 280f || rb.transform.localEulerAngles.y <= 80f)
                && (rb.transform.localEulerAngles.x >= 300f || rb.transform.localEulerAngles.x <= 60f)
                && (rb.transform.localEulerAngles.z >= 300f || rb.transform.localEulerAngles.z <= 60f))
            {
                inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), 0.0f, -Input.GetAxisRaw("Horizontal"));
            }
            else if (rb.transform.localEulerAngles.y < 280f && inputRotation.y < 0)
            {
                rb.transform.localEulerAngles = new Vector3(rb.transform.localEulerAngles.x, rb.transform.localEulerAngles.y + 1f, rb.transform.localEulerAngles.z);
                inputRotation = new Vector3(0.0f, 0, 0.0f);
            }
            else if (rb.transform.localEulerAngles.y > 80f && inputRotation.y > 0)
            {
                rb.transform.localEulerAngles = new Vector3(rb.transform.localEulerAngles.x, rb.transform.localEulerAngles.y - 1f, rb.transform.localEulerAngles.z);
                inputRotation = new Vector3(0.0f, 0, 0.0f);
            }
            else if (rb.transform.localEulerAngles.x < 300f && inputRotation.x < 0)
            {
                rb.transform.localEulerAngles = new Vector3(rb.transform.localEulerAngles.x + 1f, rb.transform.localEulerAngles.y, rb.transform.localEulerAngles.z);
                inputRotation = new Vector3(0.0f, 0, 0.0f);
            }
            else if (rb.transform.localEulerAngles.x > 60f && inputRotation.x > 0)
            {
                rb.transform.localEulerAngles = new Vector3(rb.transform.localEulerAngles.x - 1f, rb.transform.localEulerAngles.y, rb.transform.localEulerAngles.z);
                inputRotation = new Vector3(0.0f, 0, 0.0f);
            }
            else if (rb.transform.localEulerAngles.z < 300f && inputRotation.z < 0)
            {
                rb.transform.localEulerAngles = new Vector3(rb.transform.localEulerAngles.x, rb.transform.localEulerAngles.y, rb.transform.localEulerAngles.z + 1f);
                inputRotation = new Vector3(0.0f, 0, 0.0f);
            }
            else if (rb.transform.localEulerAngles.z > 60f && inputRotation.z > 0)
            {
                rb.transform.localEulerAngles = new Vector3(rb.transform.localEulerAngles.x, rb.transform.localEulerAngles.y, rb.transform.localEulerAngles.z - 1f);
                inputRotation = new Vector3(0.0f, 0, 0.0f);
            }
            else
                inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), 0.0f, -Input.GetAxisRaw("Horizontal"));
        }
        Quaternion rotationY = Quaternion.AngleAxis(inputRotation.y * Time.deltaTime * rotationSpeed, new Vector3(0f, 1f, 0f));
        Quaternion rotationX = Quaternion.AngleAxis(inputRotation.x * Time.deltaTime * rotationSpeed, new Vector3(1f, 0f, 0f));
        Quaternion rotationZ = Quaternion.AngleAxis(inputRotation.z * Time.deltaTime * rotationSpeed, new Vector3(0f, 0f, 1f));
        rb.transform.localRotation = rb.transform.localRotation * rotationX * rotationY * rotationZ;

    }*/

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
