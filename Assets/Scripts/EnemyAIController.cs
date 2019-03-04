using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    enum aiState {Patrol, Flanked, Combat};
    [SerializeField] aiState myState;
    float supression = 0;

    SeekCoverBehavior myCoverScript;
    [SerializeField] Transform flankCheckPos;

    GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        myCoverScript = GetComponent<SeekCoverBehavior>();
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch (myState)
        {
            case aiState.Patrol:
                break;
            case aiState.Flanked:
                myCoverScript.Detect();
                myCoverScript.CheckIfTargetIsInRange();
                if (myCoverScript.CheckIfDestinationReached())
                    myState = aiState.Combat;
                break;
            case aiState.Combat:
                checkIfFlanked();
                //myCoverScript.Detect();
                //myCoverScript.CheckIfTargetIsInRange();
                myCoverScript.CheckIfDestinationReached();
                break;
            default:
                break;
        }
    }


    void Combat()
    {
        if (supression < 0.5f)
        {

        }
    }

    void findCover()
    {
        myCoverScript.Detect();
        myCoverScript.CheckIfTargetIsInRange();
    }

    bool checkIfFlanked()
    {
        RaycastHit hit;
        foreach (GameObject player in players)
        {
            Debug.DrawRay(flankCheckPos.position, player.transform.position - flankCheckPos.position, Color.green);
            if (Physics.Raycast(flankCheckPos.position, player.transform.position - flankCheckPos.position, out hit, Mathf.Infinity))
            {
                //if (hit.transform.gameObject == player)
                //if (hit.transform.tag == "Player")
                if(hit.transform.gameObject.GetComponentInParent<PierInputManager>())
                {
                    Debug.Log("FLANKED!!!!!!");
                    myState = aiState.Flanked;
                    return true;
                }
                Debug.Log("flank check hit: " + hit.transform.gameObject);
            }
        }



        Debug.Log("not flanked");
        myState = aiState.Combat;
            return false;
    }

}
