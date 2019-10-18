using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int wallCount;
    public GameObject[] walls;
    public Transform wallSpawnPosition;
    public Transform wallDestinationPosition;

    private int wall_pool_size;

    public float moveSpeed;
    private GameObject wall;

    // Start is called before the first frame update
    void Start()
    {
        wall_pool_size = walls.Length;
        wall = Instantiate(walls[Random.Range(0, wall_pool_size)], this.GetComponent<Transform>());
    }

    // Update is called once per frame
    void Update()
    {
        if (wall)
        {
            if (wall.transform.position == wallDestinationPosition.position)
            {
                
                if (wallCount > 1)
                {
                    wallCount--;
                    Destroy(wall);
                    wall = Instantiate(walls[Random.Range(0, wall_pool_size)], this.GetComponent<Transform>());
                }
                else
                {
                    Destroy(wall);
                    //TODO: Ide jöhet egy játék vége felírat, vagy next level vagy valami hasonló
                }
            }
        }
        if (!wall)
        {
            //TODO: Ide jöhet egy játék vége felírat, vagy next level vagy valami hasonló
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_ANDROID
            Application.Quit();
            #endif
            
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
}
