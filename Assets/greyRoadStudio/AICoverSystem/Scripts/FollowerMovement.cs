//This FollowerMovement script is solely intended help demostrate the CoverSystem by greyRoad Studio.
//No support will be provided for this script.


using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]
public class FollowerMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private ThirdPersonCharacter character;
    private float capturedStoppingDist;

    [HideInInspector]
    public Vector3 destinationPosition;

    public Transform player;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();

        agent.updateRotation = false;
        agent.updatePosition = true;
        capturedStoppingDist = agent.stoppingDistance;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            destinationPosition = Vector3.zero;
        }

        if(destinationPosition == Vector3.zero)
        {
            agent.stoppingDistance = 3f;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.stoppingDistance = capturedStoppingDist;
            agent.SetDestination(destinationPosition);
        }


        ShoulIStayOrShouldIGoNow();
        
    }

    private void ShoulIStayOrShouldIGoNow() //If I go, there'll be trouble.
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            agent.isStopped = true;
            character.Move(Vector3.zero, false, false);

        }
        else
        {
            agent.isStopped = false;
            character.Move(agent.desiredVelocity, false, false);
        }
    }


}