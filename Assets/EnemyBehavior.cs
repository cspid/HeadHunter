using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    enum aiType { Guard, Flanker}
    [SerializeField] aiType myAiType;
    Enemy enemyScript;
    Crouch crouchScript;
    bool isFiring;

    float maxSuppressionToManeuver = 0.3f;
    float maxSuppressionToFire = 0.9f;
    float timeBetweenManeuvers = 3.5f;

    float maneuverTimeCounter = 0;

    PlayerDanger targetPlayerScript;
    PlayerDanger[] players;

    [SerializeField] SeekCoverBehavior myCoverScript;


    [SerializeField] Transform startPos;
    [SerializeField] Transform[] advancePos;
    int posCounter = 0;
    //Transform lastPos;
    //Transform targetCover;
    bool isManeuvering = false;

    bool temp = true;

    int MAX_AMMO = 20;
    int ammo;

    float fireRate = 0.2f;
    float shootCounter = 0;

    bool isReloading = false;

    float reloadTime = 4f;
    float reloadCounter = 0;

    [SerializeField] ParticleSystem muzzleFlash;

    [SerializeField] Transform gunCheckPos;

    NavMeshAgent myNavAgent;

    float suppressionValue = 0.05f;

    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<PlayerDanger>();
        enemyScript = GetComponentInParent<Enemy>();
        crouchScript = GetComponent<Crouch>();
        myNavAgent = GetComponent<NavMeshAgent>();

        ammo = MAX_AMMO;

        //myCoverScript.Detect();
        //GetComponent<NavMeshAgent>().SetDestination(targetCover.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        switch (myAiType)
        {
            case aiType.Guard:
                if (enemyScript.getSuppressionVar() < maxSuppressionToFire)
                {
                    fight();
                }
                else
                {
                    hunkerDown();
                }
                break;
            case aiType.Flanker:
                if (isManeuvering)  // Already having a maneuver
                {
                    CheckIfDestinationReached();
                }
                else // Not maneuvering
                {
                    if (maneuverTimeCounter < timeBetweenManeuvers)    // Too soon for another manuever
                    {
                        maneuverTimeCounter += Time.deltaTime;
                    }

                    if (enemyScript.getSuppressionVar() > maxSuppressionToFire)    // Too risky to even fire
                    {
                        hunkerDown();
                    }
                    else if (enemyScript.getSuppressionVar() > maxSuppressionToManeuver)    // Too risky to maneuver
                    {
                        fight();
                    }
                    else if (advancePos[posCounter] && maneuverTimeCounter >= timeBetweenManeuvers)
                    {
                        maneuver();
                    }
                }


                break;
            default:
                break;
        }



       

        //if (targetCover != null && temp)
        //{
        //    myCoverScript.GoToCover(targetCover.transform.position);
        //    temp = false;
        //}

    }

    bool CheckIfDestinationReached()
    {
        if (myNavAgent.remainingDistance <= myNavAgent.stoppingDistance && !myNavAgent.pathPending)
        {
            hunkerDown();
            isManeuvering = false;
            posCounter++;
            Debug.Log("Destination reached");
            return true;
        }
        return false;
    }

    void maneuver()
    {
        isManeuvering = true;
        myNavAgent.SetDestination(advancePos[posCounter].position);
        maneuverTimeCounter = 0;
        standUp();
    }

    void fight()
    {
        // GUARD - Shooter behaviour
        if (!isReloading)
        {
            standUp();  // FOR NOW ALWAYS STAND UP IN COMBAT, UNLESS RELOADING. CHANGE THIS LATER
            shootCounter += Time.deltaTime;

            if (shootCounter >= fireRate)
            {
                if(isFiring == false)
                {
                    audioSource.Play();
                    print("enemy firing");
                    isFiring = true;
                }
                    shoot();

                ammo--;
                shootCounter = 0;

                if (ammo <= 0)
                {
                    isReloading = true;
                    hunkerDown();
                }
            }
            else
            {

            }
        }
        else  //isReloading
        {

            isFiring = false;
            audioSource.Stop();
            print("enemy stop firing");

            reloadCounter += Time.deltaTime;

            if (reloadCounter >= reloadTime)
            {
                isReloading = false;
                ammo = MAX_AMMO;
                reloadCounter = 0;
                
            }
        }
    }

    void hunkerDown() {
        audioSource.Stop();
        crouchScript.setCrouch(true);
    }


    void standUp()
    {
        crouchScript.setCrouch(false);
    }

    void shoot()
    {
        PlayerDanger targetPlayer = findTarget();
        Vector3 temp = targetPlayer.getEyePos().position;
        temp.y = this.transform.position.y;
        this.transform.LookAt(temp);

        //Debug.Log("take that you evil player character!");

        RaycastHit hit;
        Physics.Raycast(GetComponentInParent<Enemy>().getEyePos().position, 
            targetPlayer.getEyePos().position - GetComponentInParent<Enemy>().getEyePos().position, out hit, Mathf.Infinity);

        if (hit.transform.IsChildOf(targetPlayer.transform) || hit.transform.gameObject == targetPlayer.transform || hit.transform == targetPlayer.transform.parent)
        {
            targetPlayer.getSuppressed(suppressionValue, targetPlayer.isFlanked(gunCheckPos.position, this.transform.parent.gameObject));
        }
            
        muzzleFlash.Play();

    }

    PlayerDanger findTarget()
    {
        
        foreach (PlayerDanger player in players)
        {
            if (player.isFlanked(gunCheckPos.position, this.transform.parent.gameObject))
            {
                return player;
            }
        }
        foreach (PlayerDanger player in players)
        {
            RaycastHit hit;
            Physics.Raycast(GetComponentInParent<Enemy>().getEyePos().position,
            player.getEyePos().position - GetComponentInParent<Enemy>().getEyePos().position, out hit, Mathf.Infinity);
            if (hit.transform.IsChildOf(player.transform) || hit.transform.gameObject == player.transform || hit.transform == player.transform.parent)
            {
                return player;
            }

        }
        return players[Random.Range(0, players.Length)];
    }


}
