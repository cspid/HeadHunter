using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using RootMotion.FinalIK;

public class dodge : MonoBehaviour
{
	public bool punchDetected;
	public InteractionSystem interactionSystem;
	public InteractionObject leanPosition;
	public Strafe strafeScript;
	public FullBodyBipedEffector lEffector;
	public FullBodyBipedEffector rEffector;
	public Transform rightShoulder;
	public Transform leftShoulder;
	float lDistance;
	float rDistance;

	Vector3 lStick;

	void Start()
	{
        
	}

	// Update is called once per frame
	void Update()
	{

        
		float h = CrossPlatformInputManager.GetAxis("HorizontalP1");
        float v = CrossPlatformInputManager.GetAxis("VerticalP1");

		lStick = strafeScript._newVelocity;
		transform.forward =  lStick;

		if (h < 0.2 && h > -0.2 && v < 0.2 && v > -0.2){
			transform.GetChild(0).localPosition = new Vector3(0, 0.5f, 0);
		}else{
			transform.GetChild(0).localPosition = new Vector3(0, 0.35f, 0.25f);
		}

		if (Input.GetButtonDown("Dodge")){
			print("dodging");
			lDistance = Vector3.Distance(transform.GetChild(0).position, leftShoulder.position);
			rDistance = Vector3.Distance(transform.GetChild(0).position, rightShoulder.position);

			if (lDistance < rDistance)
			{
				interactionSystem.StartInteraction(lEffector, leanPosition, true);
			} else{
				interactionSystem.StartInteraction(rEffector, leanPosition, true);

			}

		}

	}
}
