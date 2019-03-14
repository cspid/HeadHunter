using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    EnemyRaiseGun raiseGunScript;
    enum aiState {Patrol, Flanked, Combat};
    [SerializeField] aiState myState;
    float supression = 0;

    SeekCoverBehavior myCoverScript;
    [SerializeField] Transform flankCheckPos;

    GameObject[] players;
    bool isShooting = false;
    float shootCounter = 0;
    float SHOOT_COOLDOWN = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        raiseGunScript = GetComponentInChildren<EnemyRaiseGun>();
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
                Combat();
                break;
            default:
                break;
        }


        if (isShooting)
        {
            shootCounter += Time.deltaTime;

            if (shootCounter >= SHOOT_COOLDOWN)
            {
                shootCounter -= SHOOT_COOLDOWN;
                Shoot();
            }
        }
    }

    void Shoot()
    {

    }

    void Combat()
    {
        if (myState != aiState.Combat)
            return;

        if (supression < 0.5f)
        {
            isShooting = true;
        }
        else
        {
            isShooting = false;
            shootCounter = 0;
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
        raiseGunScript.setGunRaise(true);
            return false;
    }

}
