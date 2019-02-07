using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTarget : MonoBehaviour
{
    public Transform playerPosition;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Direction(playerPosition.position);

    }

    public bool Direction(Vector3 playerPos)
    {

        Vector3 dir = playerPos - this.transform.position;
        dir.Normalize();


        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "Player")
            {
                Debug.DrawRay(transform.position, dir * hit.distance, Color.yellow);
                Debug.Log("Player Hit");
                return true;
            }
        }

            Debug.DrawRay(transform.position, dir * 1000, Color.white);
            Debug.Log("Did not Hit");
        return false;



    }
}
