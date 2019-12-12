using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GVRConfigureButton : MonoBehaviour
{
    public Image imgCircle;
    public UnityEvent GVRClick;
    public float totalTime = 1f;
    bool gvrStatus;
    public float gvrTimer;
    private Spawner spawnerComponent;
    private PlayerManager playerManagerComponent;

    private float moveSpeed;
    private float minMoveSpeed = 1;
    private float maxMoveSpeed = 50;

    private int wallAmount;
    private int minWallAmount = 1;
    private int maxWallAmount = 50;

    private int playerLifeAmount;
    private int minPlayerLifeAmount = 1;
    private int maxPlayerLifeAmount = 10;

    private Text speedText;
    private Text amountText;
    private Text lifeText;

    private void Start()
    {        
        spawnerComponent = (Spawner)GameObject.Find("SceneManager").GetComponent<Spawner>();
        playerManagerComponent = (PlayerManager)GameObject.Find("SceneManager").GetComponent<PlayerManager>();
        moveSpeed = spawnerComponent.GetMoveSpeed();
        wallAmount = spawnerComponent.GetWallAmount();
        playerLifeAmount = playerManagerComponent.GetPlayerLifeAmount();
        speedText = (Text)GameObject.Find("Wall_Speed/Current_Wall_Speed/Text").GetComponent<Text>();
        speedText.text = moveSpeed.ToString();
        amountText = (Text)GameObject.Find("Wall_Amount/Current_Wall_Amount/Text").GetComponent<Text>();
        amountText.text = wallAmount.ToString();
        lifeText = (Text)GameObject.Find("Player_Life/Current_Player_Life/Text").GetComponent<Text>();
        lifeText.text = playerLifeAmount.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if(this.name == "Back")
        {
            if (gvrStatus)
            {
                imgCircle.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.3f, 0.3f);
                gvrTimer += Time.deltaTime;
                imgCircle.fillAmount = gvrTimer / totalTime;
            }

            if (gvrTimer > totalTime)
            {

                GVRClick.Invoke();
                imgCircle.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
                //resetTimer();
                GvrOff();
            }
        }
        else
        {
            if (gvrStatus)
            {
                imgCircle.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.3f, 0.3f);
                gvrTimer += Time.deltaTime;
                imgCircle.fillAmount = gvrTimer / totalTime;
            }

            if (gvrTimer > totalTime)
            {

                GVRClick.Invoke();
                imgCircle.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
                resetTimer();
            }
        }
    }

    private void resetTimer()
    {
        gvrTimer = 0;
    }


    public void GvrOn()
    {
        gvrStatus = true;
    }

    public void GvrOff()
    {
        gvrStatus = false;
        gvrTimer = 0;
        imgCircle.fillAmount = 1;
        imgCircle.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    public void IncreaseWallMoveSpeed()
    {
        if(moveSpeed < maxMoveSpeed)
        {
            moveSpeed++;
            speedText.text = moveSpeed.ToString();
            spawnerComponent.SetMoveSpeed(moveSpeed);
        }
    }

    public void DecreaseWallMoveSpeed()
    {
        if(moveSpeed > minMoveSpeed)
        {
            moveSpeed--;
            speedText.text = moveSpeed.ToString();
            spawnerComponent.SetMoveSpeed(moveSpeed);
        }
    }

    public void IncreaseWallAmount()
    {
        if(wallAmount < maxWallAmount)
        {
            wallAmount++;
            amountText.text = wallAmount.ToString();
            spawnerComponent.SetWallAmount(wallAmount);
        }
    }

    public void DecreaseWallAmount()
    {
        if (wallAmount > minWallAmount)
        {
            wallAmount--;
            amountText.text = wallAmount.ToString();
            spawnerComponent.SetWallAmount(wallAmount);
        }
    }

    public void IncreasePlayerLifeAmount()
    {
        if(playerLifeAmount < maxPlayerLifeAmount)
        {
            playerLifeAmount++;
            lifeText.text = playerLifeAmount.ToString();
            playerManagerComponent.SetPlayerLifeAmount(playerLifeAmount);
        }
    }

    public void DecreasePlayerLifeAmount()
    {
        if (playerLifeAmount > minPlayerLifeAmount)
        {
            playerLifeAmount--;
            lifeText.text = playerLifeAmount.ToString();
            playerManagerComponent.SetPlayerLifeAmount(playerLifeAmount);
        }
    }
}
