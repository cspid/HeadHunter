using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guardNav : MonoBehaviour
{
    public static event System.Action OnGuardHasSpottedPlayer;

    public float speed = 5;
    public float waitTime = .3f;
    public float turnSpeed = 90;
    public float timeToSpotPlayer = .5f;

    public AudioClip caughtSound;

    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;

    bool callSurprise = false;

    float viewAngle;
    float playerVisibleTimer;

    Vector3 playerPos;

    public Transform pathHolder;
    Transform player;
    Color originalSpotlightColour;
    public Transform goal;

    void Start()
    {
        //UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotlight.spotAngle;
        originalSpotlightColour = spotlight.color;

        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }

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
                playerPos = player.transform.position;
                playerPos.z = -0.769f;
                Debug.Log(playerPos);
                //move guard to playerPos
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

    bool CanSeePlayer()
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

    IEnumerator Investigate()
    {
        while (this.transform.position != playerPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);

            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }
}
