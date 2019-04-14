using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceCam : MonoBehaviour
{
    public int moveToPos;
    int playerCount = 0;
    public CamManager camManager;
    public Transform pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IsPlayer>() != null)
        {
            playerCount++;
            print(playerCount);
            if(playerCount >= 2)
            {
                camManager.MoveCam(pos);
            }
        }
    }
}
