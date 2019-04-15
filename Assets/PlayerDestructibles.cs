using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestructibles : MonoBehaviour
{
    // Start is called before the first frame update
    bool canStrike = true;
    CanPush selectedTarget;
    public float debrisTimer = 0.35f;
    float debrisTimerAtStart;



    public void StrikableObjects(Vector3 center, float radius, Vector3 shooterPos)
    {
        if (canStrike == true)
        {
            print("Player Destruction was accessed");
            Collider[] hitColliders = Physics.OverlapSphere(center, radius);
            var go = new GameObject();
            go.transform.position = center;
            List<CanPush> pushables = new List<CanPush>();

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.GetComponent<CanPush>())
                {
                    print(hitCollider.name + " Collider");
                    pushables.Add(hitCollider.gameObject.GetComponent<CanPush>());
                }

            }

            if (pushables.Count > 0)
            {
                selectedTarget = pushables[Random.Range(0, pushables.Count)];
                selectedTarget.getHit(shooterPos);
                selectedTarget.StartParticles();
            }

            canStrike = false;
            //WaitAndStrike();
        }

    }

    //Delay between bullets striking stuff
    void WaitAndStrike()
    {
        //print(rb.gameObject.name + "hit");

        if (!canStrike)
        {
            debrisTimer -= Time.deltaTime;

            if (debrisTimer <= 0)
            {
                debrisTimer = debrisTimerAtStart;
                canStrike = true;
                if (selectedTarget)
                    selectedTarget.StopParticles();
            }
        }
    }
}
