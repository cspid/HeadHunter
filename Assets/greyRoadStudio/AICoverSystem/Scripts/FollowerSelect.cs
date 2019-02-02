//This FollowerSelect script is solely intended help demostrate the CoverSystem by greyRoad Studio.
//No support will be provided for this script.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowerSelect : MonoBehaviour
{
    private RaycastHit rayHit;// = new RaycastHit();
    private NavMeshHit navHit;
    private Transform follower;

	private void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseClick();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            follower = null;
        }
    }

    private void OnMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out rayHit))
        {
            if (rayHit.transform.GetComponent<FollowerMovement>() != null)
            {
                follower = rayHit.transform;
            }

            if (follower != null && rayHit.transform.GetComponent<FollowerMovement>() == null)
            {
                if (NavMesh.SamplePosition(rayHit.point, out navHit, 4f, NavMesh.AllAreas))
                {
                    follower.GetComponent<FollowerMovement>().destinationPosition = navHit.position;                
                }
            }




        }
    }
}
