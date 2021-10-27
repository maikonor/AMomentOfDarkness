using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnce : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip clip;
    public float volume = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("first time", 1) == 1)
        {
            audioSource.PlayOneShot(clip, volume);
            PlayerPrefs.SetInt("first time", 0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
