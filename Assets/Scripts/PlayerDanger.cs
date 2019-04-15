﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RootMotion.Dynamics;

public class PlayerDanger : MonoBehaviour
{
    [SerializeField] Image LoadingBar;
    [SerializeField] Transform eyePos;

    [SerializeField] Transform flankCheckPos;   //This should be near the bottom of the player
    [SerializeField] float dangerLevel = 0f;
    float dangerNormSpeed = 0.05f;


    float coverDangerMultiplier = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dangerLevel > 0)
        {
            dangerLevel -= Time.deltaTime * dangerNormSpeed;

            if (dangerLevel < 0)
            {
                dangerLevel = 0;
            }

        }
        LoadingBar.fillAmount = dangerLevel;
    }

    public bool isFlanked(Vector3 gunCheckPos, GameObject attacker)
    {
        RaycastHit hit;
        Debug.DrawRay(flankCheckPos.position, gunCheckPos - flankCheckPos.position, Color.green);

        if (Physics.Raycast(flankCheckPos.position, gunCheckPos - flankCheckPos.position, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject == attacker)
            {
                Debug.Log("flanked.");
                return true;
            }
        }
        return false;
    }

    public void getSuppressed(float amount, bool isFlanked)
    {
        Debug.Log("Getting threated by: " + amount);

        if (isFlanked)
        {
            amount *= coverDangerMultiplier;
        }

        dangerLevel += amount;

        if (dangerLevel > 1 && isFlanked)
        {
            // DEATH!!!!
            Debug.Log("player dead");
            GetComponentInChildren<PuppetMaster>().state = PuppetMaster.State.Dead;
        }
    }

    public Transform getEyePos()
    {
        return eyePos;
    }

}
