using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public static int wallAmount = 5; //it has to be static, so the value stays the same between Scenes.
    public static float moveSpeed = 12;
    public int defaultWallAmount = 5;
    public int defaultMoveSpeed = 12;
    public GameObject[] walls;
    public Transform wallSpawnPosition;
    public Transform wallDestinationPosition;

    private int wall_pool_size;

    private SceneChanger sceneChanger;
    private Text congratulationsText;
    private PlayerManager playerManager;
    public int playerLifeAmount;
    private bool isGameOver = false;

    private GameObject wall;

    // Start is called before the first frame update
    void Start()
    {
        wall_pool_size = walls.Length;
        if(wallAmount > 0 && wall_pool_size!=0)
        {
            wall = Instantiate(walls[Random.Range(0, wall_pool_size)], this.GetComponent<Transform>());
        }
        sceneChanger = (SceneChanger)GameObject.Find("SceneManager").GetComponent<SceneChanger>();
        //playerManager = (PlayerManager)GameObject.Find("ZombiePlayer_Spawn_Postion").GetComponent<PlayerManager>();
        //playerLifeAmount = playerManager.GetPlayerLifeAmount();
    }

    // Update is called once per frame
    void Update()
    {
        if (wall)
        {
            if (wall.transform.position == wallDestinationPosition.position)
            {
                /*if (isGameOver)
                {
                    Destroy(GameObject.Find("BackgroundAudio"));
                    sceneChanger.LoadScene("VRMenuScene");
                    Debug.Log("GAME OVER");
                }
                else */if (wallAmount > 1)
                {
                    wallAmount--;
                    Destroy(wall);
                    wall = Instantiate(walls[Random.Range(0, wall_pool_size)], this.GetComponent<Transform>());
                }
                else
                {
                    /*congratulationsText = (Text)GameObject.Find("ZombiePlayer_Spawn_Postion/1VRView_Zombie_Player(Clone)/rig/hips/HeadCameraHolder/FirstPersonView_MainCamera/Canvas_GameOver/Text").GetComponent<Text>();
                    congratulationsText.text = "CONGRATULATIONS YOU WIN";*/
                    //Destroy(wall);
                    wallAmount--;
                }
            }
        }
        

    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void SetWallAmount(int newAmount)
    {
        wallAmount = newAmount;
    }

    public int GetWallAmount()
    {
        return wallAmount;
    }

    public int GetDefaultWallAmount()
    {
        return defaultWallAmount;
    }

    public int GetDefaultMoveSpeed()
    {
        return defaultMoveSpeed;
    }

    public void SetIsGameOver(bool newIsGameOver)
    {
        isGameOver = newIsGameOver;
    }

    public bool GetIsGameOver()
    {
        return isGameOver;
    }
}
