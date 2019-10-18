using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float rotationSpeed;

    private float maxRotationSpeed = 100f;
    private Vector3 input;
    private Vector3 inputRotation;
    private Rigidbody rb;

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

    private void FixedUpdate()
    {
        //inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), 0.0f, -Input.GetAxisRaw("Horizontal"));
        //Quaternion deltaRotation = Quaternion.Euler(inputRotation * Time.deltaTime * rotationSpeed);
        //rb.MoveRotation(rb.rotation * deltaRotation);
        rb.transform.Rotate(inputRotation * Time.deltaTime * rotationSpeed);
        //Debug.Log(rb.position);

    }

    // Update is called once per frame
    void Update()
    {
        inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), 0.0f, -Input.GetAxisRaw("Horizontal"));
        //inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"), 0.0f);
        //inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), 0.0f, 0.0f);
        //inputRotation = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, 0.0f);
        //inputRotation = new Vector3(0.0f, Input.GetAxisRaw("Horizontal"), 0.0f);

        /*inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), 0.0f, -Input.GetAxisRaw("Horizontal"));
        Quaternion deltaRotation = Quaternion.Euler(inputRotation * Time.deltaTime * rotationSpeed);
        rb.MoveRotation(rb.rotation * deltaRotation);*/
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
