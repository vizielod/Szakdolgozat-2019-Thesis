﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform wallSpawnPosition;
    public Transform wallDestinationPosition;
    public float moveSpeed;

    public GameObject crashParticles;

    public GameObject wall;

    //private int currentPoint;

    // Start is called before the first frame update
    void Start()
    {
        /*transform.position = patrolPoints[0].position;
        currentPoint = 0;*/
        transform.position = wallSpawnPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        //If the wall doesn't hit the player will reach the destination position, in which case it will respawn at the spawning position.
        if (transform.position == wallDestinationPosition.position)
        {
            //transform.position = wallSpawnPosition.position;
            Destroy(this.gameObject);

        }
        transform.position = Vector3.MoveTowards(transform.position, wallDestinationPosition.position, moveSpeed * Time.deltaTime);
        /*if(transform.position == patrolPoints[currentPoint].position)
        {
            currentPoint++;
        }

        if(currentPoint >=
            patrolPoints.Length)
        {
            currentPoint = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPoint].position, moveSpeed * Time.deltaTime);*/
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public void SetMoveSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }

    /*void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            print("I hit a player");
        }
    }*/
}
