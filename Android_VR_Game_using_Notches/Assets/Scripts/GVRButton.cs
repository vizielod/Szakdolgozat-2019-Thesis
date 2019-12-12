using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GVRButton : MonoBehaviour
{

    public Image imgCircle;
    public UnityEvent GVRClick;
    public float totalTime = 1.5f;
    bool gvrStatus;
    public float gvrTimer;
    public AudioSource backgroundAudioSource;
    private bool changeMusicButtonText = true; //used for the TurnMusic ON/OFF button. So when the Click function is fired it is not calling the ChangeTurnMusicText function canstantly. We ahve to look away and look back to the button to Invoke the function again.
    private GameObject go;

    private void Start()
    {
        backgroundAudioSource = (AudioSource)GameObject.Find("BackgroundAudio").GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (this.name == "Turn_Music_ON_OFF")
        {
            if (gvrStatus && changeMusicButtonText)
            {
                imgCircle.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.3f, 0.3f);
                gvrTimer += Time.deltaTime;
                imgCircle.fillAmount = gvrTimer / totalTime;
            }

            if (gvrTimer > totalTime && changeMusicButtonText)
            {

                GVRClick.Invoke();
                imgCircle.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
                //changeMusicButtonText = true;
                //resetTimer();
            }
        }
        else if(this.name == "Configure_Level")
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
                //imgCircle.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
                //resetTimer();
                GvrOff(); //azért kell, hogy a Status és a Timer is ressetelodjon
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
                //resetTimer();
            }
        }
    }

    private void resetTimer()
    {
        gvrTimer = 0;
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(10f);
    }

    public void GvrOn()
    {
        gvrStatus = true;
        changeMusicButtonText = true;
    }

    public void GvrOff()
    {
        gvrStatus = false;
        gvrTimer = 0;
        imgCircle.fillAmount = 1;
        imgCircle.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    public void ChangeTurnMusicText()
    {
        if (!backgroundAudioSource.isPlaying && changeMusicButtonText)
        {
            Text Mytext = (Text)GameObject.Find("Turn_Music_ON_OFF/Text").GetComponent<Text>();
            Mytext.text = "Turn Music ON";
            changeMusicButtonText = false;
        }
        if (backgroundAudioSource.isPlaying && changeMusicButtonText)
        {
            Text Mytext = (Text)GameObject.Find("Turn_Music_ON_OFF/Text").GetComponent<Text>();
            Mytext.text = "Turn Music OFF";
            changeMusicButtonText = false;
        }
    }
}
