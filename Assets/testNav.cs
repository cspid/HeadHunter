using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class testNav : MonoBehaviour {

	public Transform spot;
	// Use this for initialization
	void Start () {
		GetComponent<NavMeshAgent>().SetDestination(spot.position);
	}

}
