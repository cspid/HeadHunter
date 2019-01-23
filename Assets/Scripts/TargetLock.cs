using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetLock : MonoBehaviour
{
   
    public static void CheckRadius(Transform self,Transform center,Transform target,float radius,  bool right, float offset= .06f)
    {
        Collider[] colliders = Physics.OverlapSphere(center.position, radius);
        bool gotHit = false;
        foreach(Collider c in colliders)
        {
            HitTarget hitTarget = c.GetComponent<HitTarget>();

            if(hitTarget != null && hitTarget.transform != self)
            {
               
                Vector3 relativePos = center.InverseTransformPoint(hitTarget.transform.position);

                if (relativePos.z > 0 && ((right == true && relativePos.x < center.localPosition.x + offset) || (right == false && relativePos.x < -center.localPosition.x + offset)))
                {
                    gotHit = true;
                    target.position = new Vector3(hitTarget.transform.position.x, target.position.y, hitTarget.transform.position.z);

                }
            }
        }
        if(gotHit == false)
        {
            target.localPosition = new Vector3(0, 0, radius);

        }

    }
}
