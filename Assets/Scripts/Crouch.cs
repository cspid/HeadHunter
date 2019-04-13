using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class Crouch : MonoBehaviour
{
    public PierInputManager manager;
    public PierInputManager.ButtonName crouch;
    //    FullBodyBipedIK IK;
    float crouchWeight;
    public bool isCrouching;
    public bool forceCrouch;
    Animator animator;
    public bool isEnemy;

    public AudioClip crouchSound;
    bool crouchSoundPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    }

    // Update is called once per frame
    void Update()
    {
        if (forceCrouch == false)
        {
            //Only check controller input if this is a player
            if (isEnemy == false)
            {
                if (manager.GetButton(crouch))
                {
                    isCrouching = true;
                }
                else
                {
                    isCrouching = false;
                    crouchSoundPlayed = false;
                }
            }
        }
        else
        {
            isCrouching = true;
        }

        if (isCrouching == true)
        {
            animator.SetLayerWeight(3, 0.7f);
            if (crouchSound != null && crouchSoundPlayed == false)
            {
                AudioSource.PlayClipAtPoint(crouchSound, Camera.main.transform.position);
                crouchSoundPlayed = true;
            }
        }
        else
        {            
            animator.SetLayerWeight(3, 0);            
        }


        //IK.solver.bodyEffector.positionWeight = crouchWeight;
    }

}
