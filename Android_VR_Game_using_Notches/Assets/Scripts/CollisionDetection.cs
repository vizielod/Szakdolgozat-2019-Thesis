using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{

    private Transform wall;
    private GameObject currWall_GameObject;
    private Transform currWall_Transform;

    private Patrol wall_patrolScript;
    private Wall_Patrol current_WallPatrolScript;
    private Transform wallSpawnPosition;
    private Transform wallDestinationPosition;

    private Transform playerSpawnPosition;

    private GameObject crashParticles;
    private Rigidbody[] player_rigidbodies;
    // Start is called before the first frame update
    private float originalMoveSpeed;

    private GameObject player;
    private bool doCrashParticleEffect;

    void Start()
    {

        playerSpawnPosition = (Transform)GameObject.Find("ZombiePlayer_Spawn_Postion").GetComponent<Transform>();
        player = PrefabUtility.InstantiatePrefab(Resources.Load("Zombie_Player"), playerSpawnPosition) as GameObject;
        wallSpawnPosition = (Transform)GameObject.Find("Wall_Spawn_Position").GetComponent<Transform>();
        wallDestinationPosition = (Transform)GameObject.Find("Wall_Destination_Position").GetComponent<Transform>();
        Debug.Log(player);
        wall = GetComponent<Transform>();
        doCrashParticleEffect = true;
        
        //wall.transform.rotation = new Quaternion(0, 0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {

        current_WallPatrolScript = wall.GetComponent<Wall_Patrol>();
        Debug.Log(player);
        originalMoveSpeed = current_WallPatrolScript.GetMoveSpeed();
        //wall.transform.rotation = new Quaternion(0, 0, 0, 0);
        if(wall.transform.position == wallDestinationPosition.position)
        {
            current_WallPatrolScript.SetMoveSpeed(originalMoveSpeed);
            Destroy(player);      
        }

    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Player" || other.transform.tag == "Zombie") {
            //print("I hit a player");
            if (doCrashParticleEffect)
            {
                crashParticles = PrefabUtility.InstantiatePrefab(Resources.Load("Crash_Particles"), playerSpawnPosition) as GameObject;                
                doCrashParticleEffect = false;
            }
            EnableRagdoll();
            current_WallPatrolScript.SetMoveSpeed(2f);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        StartCoroutine(DelayDestroy());
        Destroy(crashParticles);
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(2f);
    }

    void EnableRagdoll()
    {
        player_rigidbodies = player.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in player_rigidbodies)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            //rb.detectCollisions = true;
        }
    }

    void DisableRagdoll()
    {
        foreach (Rigidbody rb in player_rigidbodies)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
    }

}
