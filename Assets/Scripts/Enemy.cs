using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//MAIN ENEMY BEHAVIOUR
public class Enemy : MonoBehaviour
{
    [SerializeField] Image LoadingBar;
    [SerializeField] Image targetIcon;
    [SerializeField] Transform aimPos;
    float suppression = 0;
    float suppressionNormSpeed = 0.1f;
    public float strikableRadius = 1;
    bool issuppressed = false;  // Not the bool for actual mechanic
    bool canStrike = true;      // Again, not related to mechanics
    int randomItem;
    public float debrisTimer = 0.2f;
    float debrisTimerAtStart;
    CanPush selectedTarget;
    
    //[SerializeField] TextMeshProUGUI suppText;  //Placeholder stuff
    [SerializeField] Transform flankCheckPos;   //This should be near the bottom of the enemy

    // Start is called before the first frame update
    void Start()
    {
        targetIcon.enabled = false;

        debrisTimerAtStart = debrisTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (suppression > 0)
        {
            suppression -= Time.deltaTime * suppressionNormSpeed;

            if (suppression < 0)
            {
                suppression = 0;
            }
        }
        LoadingBar.fillAmount = suppression;


        WaitAndStrike();
        //suppText.text = suppression.ToString();
    }
    private void LateUpdate()
    {
       // issuppressed = false;

    }
    // IMPLEMENT DMG LATER
    public void takeDamage()
    {
        Debug.Log("Enemy has taken damage");
    }

    public Transform getAimPos()
    {
        return aimPos;
    }

    public bool isFlanked(Vector3 gunCheckPos, GameObject attacker)
    {
        RaycastHit hit;
        Debug.DrawRay(flankCheckPos.position, gunCheckPos - flankCheckPos.position, Color.green);

        if (Physics.Raycast(flankCheckPos.position, gunCheckPos - flankCheckPos.position, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject == attacker)
            {
                Debug.Log("flanked.");
                return true;
            }
        }
        return false;
    }

    public void getSupressed(float amount, Vector3 shooterPos)
    {
        //Debug.Log("Getting suppressed by: " + amount);
        suppression += amount;

        if (suppression > 1)
        {
            suppression = 1;
        }
        
        issuppressed = true;

        if (targetIcon.enabled == true)
        {
            print("should work");
          StrikableObjects(transform.position, strikableRadius, shooterPos);
        }
    }

    void OnDrawGizmos()
    {
        if (issuppressed == true && targetIcon.enabled == true)
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, strikableRadius);
        }
       
    }
    public void getTargeted()
    {
        targetIcon.enabled = true;
    }

    public void cancelTarget()
    {
        targetIcon.enabled = false;
    }

    void StrikableObjects(Vector3 center, float radius, Vector3 shooterPos)
    {
        if (canStrike == true)
        {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

            List<CanPush> pushables = new List<CanPush>();

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.GetComponent<CanPush>())
                {
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
                if(selectedTarget)
                selectedTarget.StopParticles();
            }
        }
    }
}
