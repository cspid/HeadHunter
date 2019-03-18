using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class RaiseGun : MonoBehaviour
{
    // Start is called before the first frame update
    float gunRotation = 80.0f;
    float speed;
    Animator anim;
    bool raised;
    public bool isEnemy;
    public bool enemyAim;

    void Start()
    {
        anim =GetComponent<Animator>();
    }
        void Update()
    {
        if (isEnemy == false)
        {
            if (CrossPlatformInputManager.GetButton("RaiseGun"))
            {
                anim.SetBool("Raise", true);
            }
            else
            {
                anim.SetBool("Raise", false);
            }
        }
        else
        {
            if (enemyAim == true)
            {
                anim.SetBool("Raise", true);
            }
            else
            {
                anim.SetBool("Raise", false);
            }
        }
        //if (CrossPlatformInputManager.GetButton("RaiseGun"))
        //{
        //    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - speed, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        //    if (transform.localEulerAngles.x > 0)
        //    {
        //        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - speed, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        //        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x - speed, transform.localEulerAngles.y, transform.localEulerAngles.z);
        //    }
        //}
        //else
        //{
        //    if (transform.localEulerAngles.x < 80)
        //    {
        //        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + speed, transform.localEulerAngles.y, transform.localEulerAngles.z);

        //    }
        //}
    }
}
