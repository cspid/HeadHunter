using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCView : MonoBehaviour
{
    public static event System.Action OnGuardHasSpottedPlayer;
    public float timeToSpotPlayer = .5f;

    public AudioClip caughtSound;

    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;

    bool callSurprise = false;
    public Gun gun;

    float viewAngle;
    float playerVisibleTimer;

    Vector3 playerPos;

    Transform player;
    Color originalSpotlightColour;

    void Start()
    {
        //UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotlight.spotAngle;
        originalSpotlightColour = spotlight.color;

        //StartCoroutine(FollowPath(waypoints));

    }

    void Update()
    {
        if (CanSeePlayer())
        {
            playerVisibleTimer += Time.deltaTime;
            if (callSurprise)
            {
                if (caughtSound != null)
                {
                    AudioSource.PlayClipAtPoint(caughtSound, Camera.main.transform.position);
                }
                callSurprise = false;
            }
        }
        else
        {
            playerVisibleTimer -= Time.deltaTime;
            callSurprise = true;
        }
        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
        spotlight.color = Color.Lerp(originalSpotlightColour, Color.red, playerVisibleTimer / timeToSpotPlayer);

        if (playerVisibleTimer >= timeToSpotPlayer)
        {
            if (OnGuardHasSpottedPlayer != null)
            {
                OnGuardHasSpottedPlayer();
            }
        }
    }

    public bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        { //if player is in guard viewDistance
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            { //if player is in guard ViewAngle
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                { //raycast to player

                    return true;
                }
            }
        }
        return false;
    }
}
