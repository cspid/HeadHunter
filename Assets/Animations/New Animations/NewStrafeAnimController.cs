using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewStrafeAnimController : MonoBehaviour {

	Animator animator;
	public NavMeshAgent myAgent;
	Rigidbody rb;
	public float animSpeed = 1.5f;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		animator.speed = animSpeed;
		if(rb.isKinematic == false)
		{
			animator.SetFloat("Forward", transform.InverseTransformDirection(rb.velocity).z);
            animator.SetFloat("Right", transform.InverseTransformDirection(rb.velocity).x);
		}
		else if (myAgent != null)
		{
			animator.SetFloat("Forward", transform.InverseTransformDirection(myAgent.velocity).z);
			animator.SetFloat("Right", transform.InverseTransformDirection(myAgent.velocity).x);
		}
		//print(rb.velocity.x);


	}
}
