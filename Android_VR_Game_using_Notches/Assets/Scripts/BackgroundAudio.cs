using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{

    private AudioSource audiosource;

    private void Awake()
    {
        audiosource = this.GetComponent<AudioSource>();
        DontDestroyOnLoad(transform.gameObject);
        /*if (GameObject.Find("DontDestroyOnLoad/BackgroundAudio"))
        {
            Destroy(GameObject.Find("DontDestroyOnLoad/BackgroundAudio"));
        }*/
        //DontDestroyOnLoad(transform.gameObject);
        
        //audiosource.playOnAwake = false;
    }


    public void TurnAudioOnAndOff()
    {
        if (audiosource.isPlaying)
        {
            audiosource.Pause();

        }
        else if (!audiosource.isPlaying)
        {
            audiosource.UnPause();
        }
    }


}
