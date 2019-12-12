using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject player;
    public GameObject crashParticles;
    public static int playerLifeAmount = 3;
    public int defaultPlayerLifeAmount = 3;
    private bool playerRespawned = true;

    public GameObject GetPlayerGameObject()
    {
        return player;
    }

    public int GetDefaultLifeAmount()
    {
        return defaultPlayerLifeAmount;
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

    public int GetPlayerLifeAmount()
    {
        return playerLifeAmount;
    }

    public void SetPlayerLifeAmount(int newPlayerLifeAmount)
    {
        playerLifeAmount = newPlayerLifeAmount;
    }
}
