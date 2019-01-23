using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlowCanvas;
using UnityEngine.AI;
using UnityEngine.Events;

public class TriggerAdvance : MonoBehaviour 
{
	public class playerMoveData
    {
		public Transform player;
		public Vector3 destination;

		public playerMoveData(Transform player, Vector3 destination)
		{
			this.player = player;
			this.destination = destination;
		}
	}
	int numberOfPlayers;
	//int playersInTrigger = 0;
	public List<playerMoveData> players = new List<playerMoveData>();
	bool waveCleared = true;
	public Transform newPos;
	[SerializeField]
	int currentWaypoint = 0;
	public float camSpeed = 1;
	public GameObject[] wayPoints;
	bool isAdvancing = false;
	public UnityEvent NextWave;
	public Transform cam;
	public GameObject UIAdvance;
	//bool moveCam;

	void OnEnable()
	{
	    	
	}

	void Start()
	{
		numberOfPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
    }
    void Update()
    {
		if (isAdvancing == true)
		{
			CamAdvance();
		}

		if(isAdvancing)
		{
			bool allDone = true;
			foreach (playerMoveData player in players)
			{
				if(Vector3.Distance(player.player.position,player.destination) > 1)
				{
					allDone = false;
				}
			}

			if(allDone)
			{
				Debug.Log(" im done yo");
				NextWave.Invoke();
				isAdvancing = false;
				foreach (playerMoveData player in players)
                {
                    player.player.GetComponent<Rigidbody>().isKinematic = false;
                    player.player.GetComponent<NavMeshAgent>().enabled = false;
                    player.player.gameObject.GetComponentInParent<FlowScriptController>().enabled = true;
                    //player.player.gameObject.GetComponent<NavMeshAgent>().SetDestination(player.destination);
                }
				players.Clear();
			}
		}
	}
	void OnTriggerEnter (Collider other) 
	{
		if (waveCleared == true)
        {
		    if(other.tag == "Player")
			{
           // playersInTrigger++;
				players.Add(new playerMoveData(other.transform,newPos.position));
    			if (players.Count >= numberOfPlayers)
    			{
    				Advance();
    			}
			}
		}
	}

	void OnTriggerExit(Collider other)
    {
		if(isAdvancing  == false)
		{
			if (other.tag == "Player")
            {
                //playersInTrigger--;
                //players.Remove(other.transform);
                playerMoveData toRemove = null;
                foreach (playerMoveData player in players)
                {
                    if (player.player == other.transform)
                    {
                        toRemove = player;
                    }
                }
                    if (toRemove != null)
                    {
                        players.Remove(toRemove);
                    }
            }
		}
        
    }
	
	void Advance () 
	{
        isAdvancing = true;
        //Disable the players' input
        foreach(playerMoveData player in players)
        {
            player.player.GetComponent<Rigidbody>().isKinematic = true;
            player.player.GetComponent<NavMeshAgent>().enabled = true;
            player.player.gameObject.GetComponentInParent<FlowScriptController>().enabled = false;
            player.player.gameObject.GetComponent<NavMeshAgent>().SetDestination(player.destination);
        }
        //walk the players to the next scene

        //disable flashing sign
    }

    void WaveCleared()
    {
        waveCleared = true;
    }
	float timer;
	float lerpTime = 10f;

    void CamAdvance()
    {
		print(wayPoints.Length -1);
		if (currentWaypoint < wayPoints.Length)
		{
			cam.transform.position = Vector3.MoveTowards(cam.transform.position, wayPoints[currentWaypoint].transform.position, Time.deltaTime * camSpeed);
		}

        if(Vector3.Distance(wayPoints[currentWaypoint].transform.position, cam.transform.position) < 1)
        {
			if(currentWaypoint < wayPoints.Length -1)
			{
				currentWaypoint++;
			}

        }
		if (currentWaypoint >= wayPoints.Length - 1)
        {
            timer += Time.deltaTime;
            print(timer);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, wayPoints[2].transform.rotation, timer / lerpTime);
        }
	}
}
