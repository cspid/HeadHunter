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

    float maxSuppressionToManeuver = 0.3f;
    float timeBetweenManeuvers = 3.5f;

    PlayerDanger targetPlayerScript;
    PlayerDanger[] players;

    [SerializeField] SeekCoverBehavior myCoverScript;


    [SerializeField] Transform startPos;
    [SerializeField] Transform[] advancePos;
    Transform lastPos;
    Transform targetCover;

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

    float suppressionValue = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<PlayerDanger>();

        ammo = MAX_AMMO;

        //myCoverScript.Detect();
        //GetComponent<NavMeshAgent>().SetDestination(targetCover.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        // GUARD - Shooter behaviour
        if (!isReloading)
        {
            shootCounter += Time.deltaTime;

            if (shootCounter >= fireRate)
            {
                shoot();
                ammo--;
                shootCounter = 0;

                if (ammo <= 0)
                {
                    isReloading = true;
                }
            }
        }
        else  //isReloading
        {
            reloadCounter += Time.deltaTime;

            if (reloadCounter >= reloadTime)
            {
                isReloading = false;
                ammo = MAX_AMMO;
                reloadCounter = 0;
            }
        }

        //if (targetCover != null && temp)
        //{
        //    myCoverScript.GoToCover(targetCover.transform.position);
        //    temp = false;
        //}

    }

    void shoot()
    {
        PlayerDanger targetPlayer = findTarget();
        Debug.Log("take that you evil player character!");

        targetPlayer.getSuppressed(suppressionValue, targetPlayer.isFlanked(gunCheckPos.position, this.transform.parent.gameObject));
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

        return players[Random.Range(0, players.Length)];
    }


}
