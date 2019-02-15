using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTo : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform goal;

    void Start()
    {
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;
    }
}
