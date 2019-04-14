using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceCam : MonoBehaviour
{
    //public int moveToPos;
    int playerCount = 0;
    public CamManager camManager;
    public Transform pos;
    bool canTrigger = true;
    public bool force;

    // Start is called before the first frame update
    void Update()
    {
        if(force == true)
        {
            if (canTrigger == true)
            {
                playerCount++;
                print(playerCount);
                if (playerCount >= 2)
                {
                    camManager.MoveCam(pos);
                    canTrigger = false;
                }
            }
        }
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IsPlayer>() != null)
        {
            if (canTrigger == true)
            {
                playerCount++;
                print(playerCount);
                if (playerCount >= 2)
                {
                    camManager.MoveCam(pos);
                    canTrigger = false;
                }
            }
        }
    }
}
