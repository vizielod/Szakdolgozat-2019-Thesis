using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CollisionDetection : MonoBehaviour
{
    public bool isRagdoll;
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

    private PlayerManager playerManager;
    public int playerLifeAmount;
    private bool decreaseLifeAmount;
    private GameObject player;
    private bool doCrashParticleEffect;
    //private bool playerRespawned = true;

    private Text gameEndedText;
    public bool isGameOver = false;

    private SceneChanger sceneChanger;
    private Spawner spawnerComponent;

    void Start()
    {
        
        //Instantiating the Player Prefab. PrefabUtility works fine but only in Unity Editor.
        //DisableRagdoll is needed to set bodyparts to Kinematic, some adjustments will be a must
        //in the future, when i want to set the elbow notches, since right now the arms are fixed into this position.
        //Controlling the character with the sensors, I want the lower body and the head on the neck to stay Kinematic.
        playerSpawnPosition = (Transform)GameObject.Find("ZombiePlayer_Spawn_Postion").GetComponent<Transform>();
        playerManager = (PlayerManager)GameObject.Find("ZombiePlayer_Spawn_Postion").GetComponent<PlayerManager>();
        sceneChanger = (SceneChanger)GameObject.Find("SceneManager").GetComponent<SceneChanger>();
        spawnerComponent = (Spawner)GameObject.Find("Wall_spawner").GetComponent<Spawner>();

        //Debug.Log(playerManager.GetPlayerLifeAmount());
        //player = PrefabUtility.InstantiatePrefab(Resources.Load("Zombie_Player"), playerSpawnPosition) as GameObject;
        player = Instantiate(playerManager.GetPlayerGameObject(), playerSpawnPosition);
        playerManager.SetPlayerRespawned(true);
        playerLifeAmount = playerManager.GetPlayerLifeAmount();
        isGameOver = spawnerComponent.GetIsGameOver();
        //Debug.Log(playerManager.GetPlayerRespawned());
        player_rigidbodies = player.GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
        player.GetComponent<Transform>().position = playerSpawnPosition.position;

        wallSpawnPosition = (Transform)GameObject.Find("Wall_Spawn_Position").GetComponent<Transform>();
        wallDestinationPosition = (Transform)GameObject.Find("Wall_Destination_Position").GetComponent<Transform>();
        wall = GetComponent<Transform>();
        doCrashParticleEffect = true;
        decreaseLifeAmount = true;
        //wall.transform.rotation = new Quaternion(0, 0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver && playerLifeAmount < 1)
        {
            spawnerComponent.SetIsGameOver(true);
            Debug.Log("isgameover");
        }
        current_WallPatrolScript = wall.GetComponent<Wall_Patrol>();
        originalMoveSpeed = current_WallPatrolScript.GetMoveSpeed();
        //wall.transform.rotation = new Quaternion(0, 0, 0, 0);
        if(wall.transform.position == wallDestinationPosition.position && spawnerComponent.GetWallAmount() > 1)
        {          
            current_WallPatrolScript.SetMoveSpeed(originalMoveSpeed);
            Destroy(player);
        }
        if (playerLifeAmount < 1 && player)
        {
            GameObject.Find("ZombiePlayer_Spawn_Postion/1VRView_Zombie_Player(Clone)/rig/hips/HeadCameraHolder/FirstPersonView_MainCamera/Canvas_GameOver").SetActive(true);
            gameEndedText = (Text)GameObject.Find("ZombiePlayer_Spawn_Postion/1VRView_Zombie_Player(Clone)/rig/hips/HeadCameraHolder/FirstPersonView_MainCamera/Canvas_GameOver/Text").GetComponent<Text>();
            gameEndedText.text = "GAME OVER";
            StartCoroutine(DelayAndExitToMenu(4f));
        }
        else if (playerLifeAmount > 0 && spawnerComponent.GetWallAmount() < 1 && player)
        {
            GameObject.Find("ZombiePlayer_Spawn_Postion/1VRView_Zombie_Player(Clone)/rig/hips/HeadCameraHolder/FirstPersonView_MainCamera/Canvas_GameOver").SetActive(true);
            gameEndedText = (Text)GameObject.Find("ZombiePlayer_Spawn_Postion/1VRView_Zombie_Player(Clone)/rig/hips/HeadCameraHolder/FirstPersonView_MainCamera/Canvas_GameOver/Text").GetComponent<Text>();
            gameEndedText.text = "YOU WIN";
            StartCoroutine(DelayAndExitToMenu(5f));
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Player" || other.transform.tag == "Zombie") {
            //print("I hit a player");
            if (doCrashParticleEffect)
            {
                //crashParticles = PrefabUtility.InstantiatePrefab(Resources.Load("Crash_Particles"), playerSpawnPosition) as GameObject;
                crashParticles = Instantiate(playerManager.GetCrashParticlesGameObject(), playerSpawnPosition);
                doCrashParticleEffect = false;
            }
            if (decreaseLifeAmount)
            {
                playerLifeAmount--;
                playerManager.SetPlayerLifeAmount(playerLifeAmount);
                decreaseLifeAmount = false;
            }
            EnableRagdoll();    
            current_WallPatrolScript.SetMoveSpeed(5f);
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

    IEnumerator DelayAndExitToMenu(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(GameObject.Find("BackgroundAudio"));
        sceneChanger.LoadScene("VRMenuScene");
    }

    void EnableRagdoll()
    {
        //player_rigidbodies = player.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in player_rigidbodies)
        {
            isRagdoll = true;
            rb.useGravity = true;
            rb.isKinematic = false;
            //rb.detectCollisions = true;
        }
    }

    void DisableRagdoll()
    {
        foreach (Rigidbody rb in player_rigidbodies)
        {
            isRagdoll = false;
            rb.useGravity = false;
            rb.isKinematic = true;
            //rb.detectCollisions = false;
        }
    }

    public bool IsRagdoll()
    {
        return isRagdoll;
    }

}
