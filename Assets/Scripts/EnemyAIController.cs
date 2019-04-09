using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    RaiseGun raiseGunScript;
    enum aiState {Patrol, Flanked, Combat, Guard, Maneuver};
    [SerializeField] aiState myState;
    float supression = 0;

    SeekCoverBehavior myCoverScript;
    [SerializeField] Transform flankCheckPos;

    GameObject[] players;

    [SerializeField] const int MAX_AMMO = 20;
    int ammo;

    [SerializeField] const float RELOAD_TIME = 3.5f;
    float reloadCounter = 0;

    bool isReloading = false;

    bool isShooting = false;
    bool isAiming = false;

    float shootCounter = 0;
    float SHOOT_COOLDOWN = 4.5f;

    Animator myAnimator;

    Transform target;
    ErdemGun myGunScript;

    GameObject retreatPoint;
    GameObject defaultPoint;
    GameObject advancePoint;

    // Start is called before the first frame update
    void Start()
    {
        ammo = MAX_AMMO;

        //Change later, shortcut for testing
        target = GameObject.FindGameObjectWithTag("Player").transform;

        myGunScript = GetComponentInChildren<ErdemGun>();

        myAnimator = GetComponent<Animator>();
        raiseGunScript = GetComponentInChildren<RaiseGun>();
        myCoverScript = GetComponent<SeekCoverBehavior>();
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
        {
            reloadCounter += Time.deltaTime;
            if (reloadCounter >= RELOAD_TIME)
            {
                ammo = MAX_AMMO;
                reloadCounter = 0;
                isReloading = false;
            }
        }


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
            case aiState.Guard:
                myCoverScript.CheckIfDestinationReached();
                Combat();
                break;
            default:
                break;
        }


        if (isAiming && !isShooting && !isReloading)
        {
            shootCounter += Time.deltaTime;
            Vector3 dir = target.position - this.transform.position;
            dir.Normalize();
            float turnAmount = Mathf.Atan2(dir.x, dir.y);
            myAnimator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);

            if (shootCounter >= SHOOT_COOLDOWN)
            {
                isShooting = true;
                shootCounter -= SHOOT_COOLDOWN;
                //Shoot();
                StartCoroutine(ShootOverTime());
            }
        }

        if (!isReloading && ammo <= 0)
        {
            isReloading = true;
        }
    }

    IEnumerator ShootOverTime()
    {
        bool tempBool = myAnimator.GetBool("Crouch");
        myAnimator.SetBool("Crouch", false);

        yield return new WaitForSeconds(1f);
        Shoot();
        yield return new WaitForSeconds(.5f);
        myAnimator.SetBool("Crouch", tempBool);
        isShooting = false;
    }

    void Shoot()
    {
        Debug.Log("pew pew~~~~~~");
        myGunScript.shoot();
        ammo--;
    }

    void Combat()
    {
        if (myState != aiState.Combat)
            return;

        if (supression < 0.5f)
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
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
        //raiseGunScript.enemyAim = true; //gone, old system
            return false;
    }

    void retreat()
    {
        myState = aiState.Maneuver;
        myCoverScript.GoToCover(retreatPoint.transform.position);
    }

    void advance()
    {
        myState = aiState.Maneuver;
        myCoverScript.GoToCover(advancePoint.transform.position);
    }

    void failedManeuver()
    {
        myCoverScript.GoToCover(defaultPoint.transform.position);
    }
}
