using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    private float maxSpeed = 5f;
    private Vector3 input;
    private Rigidbody rb;

    private Vector3 spawn;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawn = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
        //rb.AddForce(input * moveSpeed, ForceMode.Force);
        rb.velocity = input * moveSpeed;
        /*if(Rigidbody.velocity.magnitude < maxSpeed)
        {
            Rigidbody.AddForce(input * moveSpeed);
        }*/
    }
}
