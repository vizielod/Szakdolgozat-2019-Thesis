using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public Transform wall;
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

    void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Player") {        
            //print("I hit a player");
            Instantiate(crashParticles, transform.position, Quaternion.identity);
            player.position = playerSpawnPosition.position;
            wall.position = wallSpawnPosition.position;
            //transform.position = wallSpawnPosition.position;
        }
    }

    /*IEnumerator DelayWallRespawn()
    {
        yield return new WaitForSeconds(0.5f);
        wall.position = wallSpawnPosition.position;
    }*/

}
