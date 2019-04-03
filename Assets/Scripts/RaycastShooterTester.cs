using UnityEngine;
using System.Collections;

public class RaycastShooterTester : MonoBehaviour
{
    // Apply a force to a clicked rigidbody object.

    // The force applied to an object when hit.
    public float hitForce;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(ray.direction * hitForce);
                }
            }
        }
    }
}
