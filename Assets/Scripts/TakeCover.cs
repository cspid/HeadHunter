using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeCover : MonoBehaviour
{
    public int nearbyCover = 0;
    public Crouch crouch;

    void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.GetComponent<CoverScript>() != null)
        {
            nearbyCover += 1;
        } 
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CoverScript>() != null)
        {
            nearbyCover -= 1;
        }
    }

    void Update()
    {
        if (nearbyCover > 0) {
            crouch.forceCrouch = true;
        }
        else
        {
            crouch.forceCrouch = false;
        }
    }
}
