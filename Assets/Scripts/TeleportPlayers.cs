using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayers : MonoBehaviour
{
    public Transform teleDestination;

    private void Awake()
    {
        teleDestination = GetComponent<Transform>();
    }

    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        other.transform.position = teleDestination.position;
        //need to make it check for both players to be in the elevator first
    }
}
