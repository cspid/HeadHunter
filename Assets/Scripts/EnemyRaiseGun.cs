using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using RootMotion.FinalIK;


public class EnemyRaiseGun : MonoBehaviour
{
    // Start is called before the first frame update
    float gunRotation = 80.0f;
    float speed;
    Animator anim;
    public bool enemyRaiseGun;
    public float aimSpeed = 1;
    public bool seesPlayer1;
    public bool seesPlayer2;
    public Transform player1;//Spine bone reference of player 1
    public Transform player2;//Spine bone reference of player 2


    LookAtIK lookIK;

    void Start()
    {
        //--------------Get References------------
        anim =GetComponent<Animator>();
        lookIK = GetComponent<LookAtIK>();
    }

    void Update()
    {
        if (enemyRaiseGun == true)
        {
            anim.SetBool("Raise", true);

            //--------------Aiming------------
                //--------------Player 1------------

            //Aim at  player 1 when we see them
            if (seesPlayer1 == true)
            {
                lookIK.solver.target = player1;
                if (lookIK.solver.IKPositionWeight < 1)
                {
                    lookIK.solver.IKPositionWeight += Time.deltaTime * aimSpeed;
                }

            }
            else
            {
                //when the player is out of sight, stop aiming
                if (lookIK.solver.IKPositionWeight > 1)
                {
                    lookIK.solver.IKPositionWeight -= Time.deltaTime * aimSpeed;
                }
                if (lookIK.solver.IKPositionWeight <= 0)
                {
                    lookIK.solver.target = null;
                }

            }
                 //--------------Player 2------------

            if (player2 != null)
            {
                //Aim at  player 2 when we see them
                if (seesPlayer2 == true)
                {
                    lookIK.solver.target = player1;
                    if (lookIK.solver.IKPositionWeight < 1)
                    {
                        lookIK.solver.IKPositionWeight += Time.deltaTime * aimSpeed;
                    }

                }
                else
                {
                    //when the player is out of sight, stop aiming
                    if (lookIK.solver.IKPositionWeight > 1)
                    {
                        lookIK.solver.IKPositionWeight -= Time.deltaTime * aimSpeed;
                    }
                    if (lookIK.solver.IKPositionWeight <= 0)
                    {
                        lookIK.solver.target = null;
                    }

                }
            }
        }
        else
        {
            anim.SetBool("Raise", false);
        }


    }
}
