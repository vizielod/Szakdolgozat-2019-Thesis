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
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawn = transform.position;
        //inputRotation = new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
    }

    // Update is called once per frame
    void Update()
    {
        //inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), 0.0f, 0.0f);
        //inputRotation = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, 0.0f);
        //inputRotation = new Vector3(0.0f, Input.GetAxisRaw("Horizontal"), 0.0f);
        
        /*inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), 0.0f, -Input.GetAxisRaw("Horizontal"));
        Quaternion deltaRotation = Quaternion.Euler(inputRotation * Time.deltaTime * rotationSpeed);
        rb.MoveRotation(rb.rotation * deltaRotation);*/
    }

    private void FixedUpdate()
    {
        inputRotation = new Vector3(Input.GetAxisRaw("Vertical"), 0.0f, -Input.GetAxisRaw("Horizontal"));
        Quaternion deltaRotation = Quaternion.Euler(inputRotation * Time.deltaTime * rotationSpeed);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
