using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Old_CollisionDetection : MonoBehaviour
{
    private Transform wall;
    private GameObject wallGO;
    private GameObject currWall_GameObject;
    private Transform currWall_Transform;

    private Patrol wall_patrolScript;
    private Wall_Patrol current_WallPatrolScript;
    public Transform wallSpawnPosition;
    public Transform wallDestinationPosition;

    public Transform playerSpawnPosition;

    public GameObject crashParticles;
    public Rigidbody[] player_rigidbodies;
    // Start is called before the first frame update
    private float originalMoveSpeed;

    private GameObject player;
    private PlayerManager playerManager;

    void Awake()
    {
        /*wall_patrolScript = GetComponent<Patrol>();
        originalMoveSpeed = wall_patrolScript.GetMoveSpeed();*/
        /*current_WallPatrolScript = GetComponentInChildren<Wall_Patrol>();
        Debug.Log(current_WallPatrolScript);
        originalMoveSpeed = current_WallPatrolScript.GetMoveSpeed();*/
    }

    void Start()
    {
        playerSpawnPosition = (Transform)GameObject.Find("ZombiePlayer_Spawn_Postion").GetComponent<Transform>();
        playerManager = (PlayerManager)GameObject.Find("ZombiePlayer_Spawn_Postion").GetComponent<PlayerManager>();
        //player = PrefabUtility.InstantiatePrefab(Resources.Load("Zombie_Player"), playerSpawnPosition) as GameObject;
        //rb = (Rigidbody)GameObject.Find("ZombieRig (2)/rig/hips/spine/chest").GetComponent<Rigidbody>();
        player = Instantiate(playerManager.GetPlayerGameObject(), playerSpawnPosition);

        //player = GameObject.Find("ZombieRig (3)");
        Debug.Log(player);
        wall = GetComponent<Transform>();
        wallGO = wall.gameObject;
        Debug.Log(wall);
        Debug.Log(wallGO);
        wall.transform.rotation = new Quaternion(0, 0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(wall.GetChildCount());

        if (wall.GetChildCount() > 0)
        {
            currWall_Transform = wall.GetChild(0);
            Debug.Log(currWall_Transform);
        }
        current_WallPatrolScript = currWall_Transform.GetComponent<Wall_Patrol>();
        Debug.Log(current_WallPatrolScript.GetMoveSpeed());
        Debug.Log(player);
        originalMoveSpeed = current_WallPatrolScript.GetMoveSpeed();
        //wall.transform.rotation = new Quaternion(0, 0, 0, 0);
        /*if(wall.transform.position == wallDestinationPosition.position)
        {
            //wall_patrolScript.SetMoveSpeed(originalMoveSpeed);
            current_WallPatrolScript.SetMoveSpeed(originalMoveSpeed);
            Destroy(player);
            player = PrefabUtility.InstantiatePrefab(Resources.Load("Zombie_Player"), playerSpawnPosition) as GameObject;          
        }*/
        if (currWall_Transform.position == wallDestinationPosition.position)
        {
            //wall_patrolScript.SetMoveSpeed(originalMoveSpeed);
            current_WallPatrolScript.SetMoveSpeed(originalMoveSpeed);
            Destroy(player);
            //player = PrefabUtility.InstantiatePrefab(Resources.Load("Zombie_Player"), playerSpawnPosition) as GameObject;
            player = Instantiate(playerManager.GetPlayerGameObject(), playerSpawnPosition);
        }

    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "Zombie")
        {
            //print("I hit a player");
            Debug.Log("OnCollisionEnter");
            Debug.Log(player);
            Instantiate(crashParticles, transform.position, Quaternion.identity);

            EnableRagdoll();
            //current_WallPatrolScript.SetMoveSpeed(2f);

        }
    }

    /*IEnumerator DelayWallRespawn()
    {
        yield return new WaitForSeconds(0.5f);
        wall.position = wallSpawnPosition.position;
    }*/

    void EnableRagdoll()
    {
        //Transform player_transform = player.GetComponent<Transform>();
        player_rigidbodies = player.GetComponentsInChildren<Rigidbody>();

        Debug.Log("EnableRagdoll" + player);
        foreach (Rigidbody rb in player_rigidbodies)
        {
            Debug.Log(rb);
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
