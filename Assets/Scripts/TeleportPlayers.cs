using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayers : MonoBehaviour
{
    public Transform teleDestination;
    public GameObject player;
    public GameObject nextPlayer;

    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Player1" && other.tag == "Player2") {
        //need to make it check for both players to be in the elevator first
        //Destroy(GameObject.FindWithTag("Player"));
        //GameObject Player = Instantiate(player, teleDestination.position, teleDestination.rotation);
        player.SetActive(false);
        nextPlayer.SetActive(true);
        //lerp camera to next level segment
    }
}
