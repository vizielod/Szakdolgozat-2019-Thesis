using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 * USED THIS SCRIPT FOR TESTING PURPOSES
 * **/

public class PrefabCollisionDetection : MonoBehaviour
{
    private Transform wall;
    public Transform wallSpawnPosition;

    public Transform player;
    public Transform playerSpawnPosition;

    public GameObject crashParticles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "Zombie")
        {
            //print("I hit a player");
            Instantiate(crashParticles, transform.position, Quaternion.identity);
            player.position = playerSpawnPosition.position;
            wall.position = wallSpawnPosition.position;
            //transform.position = wallSpawnPosition.position;
        }
    }
}
