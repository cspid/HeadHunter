using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundController : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip crouch;
    public AudioClip shoot;
    public AudioClip elevator;
    public AudioClip footstep;
    public AudioClip death;
    public AudioClip[] grunts;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void playSound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("No" + clip + "sound!");
        }
    }

    /*INSTRUCTION
     soundController AudioController;
     AudioController = GameObject.Find("SoundsController").GetComponent<soundController>();
     AudioController.playSound(AudioController.CLIP);*/
}
