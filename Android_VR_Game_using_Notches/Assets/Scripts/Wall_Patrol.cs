using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Patrol : MonoBehaviour
{
    private float moveSpeed = 5f;
    private Transform destination;
    //private Vector3 wallDestinationPosition = new Vector3(0, 0.5f, -12.16f); //It might be saved as resource and called it like that
    private Transform wallDestinationPosition;
    // Start is called before the first frame update
    void Start()
    {
        wallDestinationPosition = (Transform)GameObject.Find("Wall_Destination_Position").GetComponent<Transform>();
        //moveSpeed = Spawner.GetMoveSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += Time.deltaTime * transform.forward * 5;
        transform.position = Vector3.MoveTowards(transform.position, wallDestinationPosition.position, moveSpeed * Time.deltaTime);
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}
