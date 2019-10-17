using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] walls;
    public Transform wallSpawnPosition;
    public Transform wallDestinationPosition;

    public int wall_pool_size = 3;

    public float moveSpeed;
    private GameObject wall;

    // Start is called before the first frame update
    void Start()
    {
        wall = Instantiate(walls[Random.Range(0, 2)], this.GetComponent<Transform>());
    }

    // Update is called once per frame
    void Update()
    {
        
        if (wall.transform.position == wallDestinationPosition.position && wall_pool_size != 0)
        {
            wall_pool_size--;
            Destroy(wall);
            wall = Instantiate(walls[Random.Range(0, 2)], this.GetComponent<Transform>());
        }
        if (wall.transform.position == wallDestinationPosition.position && wall_pool_size == 0)
        {
            Destroy(wall);
            //TODO: Ide jöhet egy játék vége felírat, vagy next level vagy valami hasonló
        }
        /*if (wall.GetComponent<Rigidbody>().velocity.Equals(Vector3.zero))
        {
            //Destroy(wall);
        }*/

    }
}
