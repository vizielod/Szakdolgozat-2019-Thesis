using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDefaultValues : MonoBehaviour
{
    private PlayerManager playerManagerComponent;
    private Spawner spawnerComponent;

    private void Awake()
    {
        /*playerManagerComponent = (PlayerManager)GameObject.Find("SceneManager").GetComponent<PlayerManager>();
        spawnerComponent = (Spawner)GameObject.Find("SceneManager").GetComponent<Spawner>();
        playerManagerComponent.SetPlayerLifeAmount(playerManagerComponent.GetDefaultLifeAmount());
        spawnerComponent.SetMoveSpeed(spawnerComponent.GetDefaultMoveSpeed());
        spawnerComponent.SetWallAmount(spawnerComponent.GetDefaultWallAmount());

        Debug.Log(playerManagerComponent.GetPlayerLifeAmount());
        Debug.Log(spawnerComponent.GetMoveSpeed());
        Debug.Log(spawnerComponent.GetWallAmount());*/
    }
    // Start is called before the first frame update
    void Start()
    {
        playerManagerComponent = (PlayerManager)GameObject.Find("SceneManager").GetComponent<PlayerManager>();
        spawnerComponent = (Spawner)GameObject.Find("SceneManager").GetComponent<Spawner>();
        playerManagerComponent.SetPlayerLifeAmount(playerManagerComponent.GetDefaultLifeAmount());
        spawnerComponent.SetMoveSpeed(spawnerComponent.GetDefaultMoveSpeed());
        spawnerComponent.SetWallAmount(spawnerComponent.GetDefaultWallAmount());

        Debug.Log(playerManagerComponent.GetPlayerLifeAmount());
        Debug.Log(spawnerComponent.GetMoveSpeed());
        Debug.Log(spawnerComponent.GetWallAmount());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
