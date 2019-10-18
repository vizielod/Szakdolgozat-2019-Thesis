using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject player;
    public GameObject crashParticles;
    private bool playerRespawned = true;

    public GameObject GetPlayerGameObject()
    {
        return player;
    }

    public void SetPlayerGameObject(GameObject newPlayer)
    {
        player = newPlayer;
    }

    public GameObject GetCrashParticlesGameObject()
    {
        return crashParticles;
    }

    public void SetCrashParticlesGameObject(GameObject newCrashParticles)
    {
        crashParticles = newCrashParticles;
    }

    public void SetPlayerRespawned(bool playerRespawned)
    {
        this.playerRespawned = playerRespawned;
    }

    public bool GetPlayerRespawned()
    {
        return playerRespawned;
    }

}
