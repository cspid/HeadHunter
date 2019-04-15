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
    public GameObject enableThis;
    public int NumberNeededToAdvance;

    // Start is called before the first frame update
    void Update()
    {
        if(force == true)
        {
            if (canTrigger == true)
            {
                playerCount++;
                print(playerCount);
                if (playerCount >= NumberNeededToAdvance)
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
                if (playerCount >= NumberNeededToAdvance)
                {
                    camManager.MoveCam(pos);
                    canTrigger = false;
                }
                if(enableThis != null)
                {
                    enableThis.SetActive(true);
                }
            }
        }
    }
}
