using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public Transform firePoint;
    bool targeted;
    public Color lineColor;

    // Update is called once per frame
    void Update()
    {
        {
            RaycastHit hit;
            var ray = new Ray(firePoint.position, firePoint.forward);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.rigidbody != null)
                {
                    print(hit.rigidbody.gameObject.name);
                    lineColor = Color.green;
                }
                else {
                lineColor = Color.red;
                }
            }

            Vector3 forward = firePoint.TransformDirection(Vector3.forward) * 10;
            Debug.DrawRay(firePoint.position, forward, lineColor);
        }
    }
}
