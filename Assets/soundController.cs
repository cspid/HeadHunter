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
    public AudioClip grunt;
    //public AudioClip crouch;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void crouchSound()
    {
        if (crouch != null)
        {
            audioSource.PlayOneShot(crouch);
        }
        else
        {
            Debug.Log("No crouch sound!");
        }
    }
    void shootSound()
    {
        if (shoot != null)
        {
            audioSource.PlayOneShot(shoot);
        }
        else
        {
            Debug.Log("No shoot sound!");
        }
    }
    void elevatorSound()
    {
        if (elevator != null)
        {
            audioSource.PlayOneShot(elevator);
        }
        else
        {
            Debug.Log("No elevator sound!");
        }
    }
    void footstepSound()
    {
        if (footstep != null)
        {
            audioSource.PlayOneShot(footstep);
        }
        else
        {
            Debug.Log("No footstep sound!");
        }
    }
    void gruntSound()
    {
        if (grunt != null)
        {
            audioSource.PlayOneShot(grunt);
        }
        else
        {
            Debug.Log("No grunt sound!");
        }
    }
}
