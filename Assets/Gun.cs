using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;


public class Gun : MonoBehaviour
{

    public Transform firePoint;
    bool targeted;
    public Color lineColor;
    public Animator recoilAnimator;

    public LayerMask layers;
    public float unpin = 10f;
    public float force = 10f;
    public ParticleSystem blood;

    // Update is called once per frame
    void Update()

    {
        RaycastHit hit1;
        var ray1 = new Ray(firePoint.position, firePoint.forward);

        if (Physics.Raycast(ray1, out hit1))
        {
            Debug.DrawRay(firePoint.position, firePoint.forward * 50f, lineColor);

            if (hit1.rigidbody != null)
            {
                //print(hit1.rigidbody.gameObject.name);
                lineColor = Color.green;
            }
            else
            {
                lineColor = Color.red;
            }
        }

        //Vector3 forward = firePoint.TransformDirection(Vector3.forward) * 10;
        //Debug.DrawRay(firePoint.position, forward, lineColor);

        if (Input.GetButtonDown("Fire1"))
        {
            print("fire");
            //recoilAnimator.SetTrigger("Go");
            var ray = new Ray(firePoint.position, firePoint.forward);

            // Raycast to find a ragdoll collider
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 100f, layers))
            {
                var broadcaster = hit.collider.attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>();
                //Debug.DrawRay(firePoint.position, firePoint.forward, lineColor);

                if (broadcaster != null)
                {
                    broadcaster.Hit(unpin, ray.direction * force, hit.point);

                    blood.transform.position = hit.point;
                    blood.transform.rotation = Quaternion.LookRotation(ray.direction);
                    blood.Emit(20);
                }
            }
        }
    }
}


