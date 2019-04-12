using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class Crouch : MonoBehaviour
{
    FullBodyBipedIK IK;
    float crouchWeight;
    public bool isCrouching;
    public bool forceCrouch;
    public AudioClip clip;

    private bool crouchSoundPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        IK = GetComponent<FullBodyBipedIK>();
    }

    // Update is called once per frame
    void Update()
    {
        if (forceCrouch == false)
        {
            if (Input.GetButton("Crouch"))
            {
                isCrouching = true;
            }
            else
            {
                isCrouching = false;
                crouchSoundPlayed = false;
            }
        }
        else
        {
            isCrouching = true;
        }

        if (isCrouching == true)
        {
            if (clip != null && crouchSoundPlayed == false)
            {
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
                crouchSoundPlayed = true;
            }
            print("Crouching");

            if (crouchWeight < 0.144)
            {
                crouchWeight += Time.deltaTime;
            }
        }
        else
        {
            if (crouchWeight > 0)
            {
                crouchWeight -= Time.deltaTime;
            }
        }


        IK.solver.bodyEffector.positionWeight = crouchWeight;
    }

}
