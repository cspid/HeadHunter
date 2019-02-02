using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]
public class SeekCoverBehavior : MonoBehaviour
{
    private NavMeshAgent agent;
    private ThirdPersonCharacter character;
    private Collider[] detectionColliders;
    private Collider[] coverColliders;

    private float capturedStoppingDist;

    private bool shouldSeekCover = true;
    private bool coverFound;
    private bool shouldCrouch;

    //public for debug
    private Transform fleeTarget;
    private Cover coverObject;
    private Vector3 coverPosition = Vector3.zero;
    //public Vector3 fleePosition = Vector3.zero;
    private NavMeshHit navHit;
    private LayerMask coverLayer;

    [Tooltip("Remember to Tag your Player Character with the \"Player\" Tag. This AI will flee from GameObjects with Applicable Tags in Applicable Layers. " +
        "You can create NPC tags in the Tag Manager and type them here if you want this AI to flee from NPCs.")]
    public string[] applicableTags = { "Player" };

    [Tooltip("Select layers that this AI can detect. This AI will flee from GameObjects in Applicable Layers with Applicable Tags. "+
        "You must add the Player layer and any NPC layers if you've changed their layers.)")]
    public LayerMask applicableLayers = 1;
    
    [Tooltip("How far this AI can see, how close enemies must be before this AI flees and seeks cover. Detection Range must be greater than Flee Range. " +
        "For best results, keep Detection Range at 5 to 10 meters higher than Flee Range. A Detection Range of 20 to 30 meters is good for a midsize level.")]
    [Range(10, 55)]
    public float detectionRange = 20.0f;

    [Tooltip("How close enemies must be before this AI flees from behind cover. Flee Range must be less than Detection Range. " +
        "For best results, keep Flee Range at 5 to 10 meters lower than Detection Range. A Flee Range of 10 to 20 meters is good for a midsize level.")]
    [Range(5, 50)]
    public float fleeRange = 10.0f;

    private float checkRate = 0.5f;
    private float nextCheck;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();

        capturedStoppingDist = agent.stoppingDistance;

        if (applicableTags.Length == 0)
        {
            Debug.LogError("You must have at least one Tag in the \"Applicable Tags\" array under the SeekCoverBehavior Component of the " +
                transform.name + " GameObject");
        }

        if (LayerMask.NameToLayer("Cover") == -1)
        {
            Debug.LogError("You must add a Layer named \"Cover\" in the Layer Manager. " +
                "At the top of the Inspector, click the Layer dropdown menu and click \"Add Layer.\"\n" +
                "Then type in \"Cover\" in one of the empty User Layer fields. \"Cover\" must be typed exactly as shown, with a capital \"C\".");
        }
        else
        {
            coverLayer |= (1 << LayerMask.NameToLayer("Cover"));            
        }

        if(detectionRange <= fleeRange)
        {
            Debug.LogWarning("Detection Range cannot be less than or equal to Flee Range. Detection Range has been set at Flee Range plus 10.");
            detectionRange += 10;
        }

        agent.avoidancePriority = Random.Range(0, 50);
        agent.updateRotation = false;
        agent.updatePosition = true;

    }

    private void Update()
    {
        if(Time.time > nextCheck)
        {
            nextCheck = Time.time + checkRate;
            Detect();
            CheckIfTargetIsInRange();
        }

        CheckIfDestinationReached();

    }

    private void Detect()
    {
        detectionColliders = Physics.OverlapSphere(transform.position, detectionRange, applicableLayers);

        detectionColliders = detectionColliders.OrderBy(col => Vector3.Distance(transform.position, col.transform.position)).ToArray();

        foreach (Collider col in detectionColliders)
        {
            foreach (string tag in applicableTags)
            {
                if (col.CompareTag(tag))
                {
                    fleeTarget = col.transform;
                    return;
                }
            }
        }
    }

    private void CheckIfTargetIsInRange()
    {
        if(fleeTarget == null)
        {
            return;
        }
        
        float distToTarget = Vector3.Distance(transform.position, fleeTarget.position);
        if(distToTarget <= fleeRange)
        {
            //if (Time.time > nextCheck)
            //{
            //    nextCheck = Time.time + checkRate;
            //    Debug.Log("fleeing distToTarget: " + distToTarget.ToString());
            //}

            Flee();
        }
        else if(distToTarget <= detectionRange && distToTarget > fleeRange)
        {
            //if (Time.time > nextCheck)
            //{
            //    nextCheck = Time.time + checkRate;
            //    Debug.Log("seeking Cover distToTarget: " + distToTarget.ToString());
            //}

            ResetSeekCover();
            SeekCover();
        }


    }

    private void CheckIfDestinationReached()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            if (coverObject != null && coverObject.canCrouch && coverFound)
            {
                shouldCrouch = true;
            }
            else
            {
                shouldCrouch = false;
            }

            agent.isStopped = true;
            character.Move(Vector3.zero, shouldCrouch, false);



            //coverPosition = Vector3.zero;
            //fleePosition = Vector3.zero;

        }
        else
        {
            agent.isStopped = false;
            character.Move(agent.desiredVelocity, false, false);
        }
    }


    private void SeekCover()
    {
        if (!shouldSeekCover)
        {
            return;
        }

        if (!coverFound)
        {
            coverColliders = Physics.OverlapSphere(transform.position, detectionRange, coverLayer);

            if (coverColliders.Length > 0)
            {
                EvaluateCover();
            }
            else
            {
                Flee();
            }
        }
        
    }

    private void EvaluateCover()
    {
        coverColliders = coverColliders.OrderByDescending(col => Vector3.Distance(transform.position, col.transform.position)).ToArray();

        for (int i = 0; i < coverColliders.Length; i++)
        {
             
            if (coverColliders[i].GetComponentInParent<CoverScript>() != null)
            {
                CoverScript coverScript = coverColliders[i].GetComponentInParent<CoverScript>();
                Vector3 toEnemyTarget = fleeTarget.position - coverScript.transform.position;
                Vector3 coverForward = coverScript.transform.TransformDirection(Vector3.forward);

                //if enemy is in front of cover 75 degrees
                if (Vector3.Dot(coverForward, toEnemyTarget) > 0.25882f)
                {
                    for (int k = 0; k < coverScript.rear.Count; k++)
                    {
                        if (!coverScript.rear[k].isOccupied)
                        {
                            //Collider[] colliders = Physics.OverlapSphere(coverScript.transform.position, detectionRange, applicableLayers);
                            //if(colliders.Length == 0)
                            //{ 
                            if (Vector3.Distance(coverScript.transform.position, fleeTarget.position) > detectionRange)
                            {
                                coverObject = coverScript.rear[k];
                                coverObject.isOccupied = true;
                                coverPosition = coverObject.coverTransform.position;
                                coverFound = true;

                                break;
                            }
                        }
                    }
                    if (coverFound)
                    {
                        break;
                    }

                }
                //if enemy is behind cover 105 degrees
                else if(Vector3.Dot(coverForward, toEnemyTarget) < -0.25882f)
                {
                    for (int k = 0; k < coverScript.front.Count; k++)
                    {
                        if (!coverScript.front[k].isOccupied)
                        {
                            //Collider[] colliders = Physics.OverlapSphere(coverScript.transform.position, detectionRange, applicableLayers);
                            //if (colliders.Length == 0)
                            //{

                            if (Vector3.Distance(coverScript.transform.position, fleeTarget.position) > detectionRange)
                            {
                                coverObject = coverScript.front[k];
                                coverObject.isOccupied = true;
                                coverPosition = coverObject.coverTransform.position;
                                coverFound = true;

                                break;
                            }
                        }
                    }
                }
                if (coverFound)
                {
                    break;
                }
            }
        }


        if (coverFound)
        {
            GoToCover();
        }
        else
        {            
            Flee();
        }

    }

    private void GoToCover()
    {
            agent.stoppingDistance = 0.5f;
            agent.SetDestination(coverPosition);
    }

    private void Flee()
    {
        shouldSeekCover = false;
        agent.stoppingDistance = capturedStoppingDist;
        UnoccupyCover();
        Vector3 directionAwayFromEnemy = transform.position - fleeTarget.position;
        Vector3 awayPos = transform.position + directionAwayFromEnemy;

        if (NavMesh.SamplePosition(awayPos, out navHit, 5.0f, NavMesh.AllAreas))
        {
            //fleePosition = navHit.position;
            agent.SetDestination(navHit.position);
        }
        else
        {
            if(NavMesh.SamplePosition(transform.position, out navHit, 15.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
            }
        }

    }
    

    private void ResetSeekCover()
    {        
        shouldSeekCover = true;
    }

    private void UnoccupyCover()
    {
        coverFound = false;

        if(coverObject != null)
        {
            coverObject.isOccupied = false;
            coverObject = null;

        }
    }

}
