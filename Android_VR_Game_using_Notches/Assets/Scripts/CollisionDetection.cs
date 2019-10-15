using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{

    private Transform wall;
    private Patrol wall_patrolScript;
    public Transform wallSpawnPosition;

    public Transform player;
    public Transform playerSpawnPosition;

    public GameObject crashParticles;
    public Rigidbody[] player_rigidbodies;
    public GameObject player_gameobject;
    // Start is called before the first frame update

    void Awake()
    {
        //zombie = GameObject.Find("ZombieRig (3)");
        player_gameobject = player.gameObject;
        player_rigidbodies = player_gameobject.GetComponentsInChildren<Rigidbody>();
    }

    void Start()
    {
        //rigidbodies = GetComponentsInChildren<Rigidbody>();
        wall = GetComponent<Transform>();
        Debug.Log(wall);
        wall.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        wall.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Player" || other.transform.tag == "Zombie") {        
            //print("I hit a player");
            Instantiate(crashParticles, transform.position, Quaternion.identity);
            
            foreach (Rigidbody rb in player_rigidbodies)
            {
                rb.useGravity = true;
                rb.constraints = RigidbodyConstraints.None;
                
            }
            wall_patrolScript = GetComponent<Patrol>();
            wall_patrolScript.SetMoveSpeed(2f);

            //other.transform.position = playerSpawnPosition.position;
            //wall.position = wallSpawnPosition.position;
            //transform.position = wallSpawnPosition.position;
        }
    }

    /*IEnumerator DelayWallRespawn()
    {
        yield return new WaitForSeconds(0.5f);
        wall.position = wallSpawnPosition.position;
    }*/

}
