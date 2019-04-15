using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RootMotion.Dynamics;

//MAIN ENEMY BEHAVIOUR
public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] Image LoadingBar;
    [SerializeField] Image targetIcon;
    [SerializeField] Transform aimPos;
    float suppression = 0;
    float suppressionNormSpeed = 0.1f;

    [SerializeField] GameObject chest;

    float suppNormDelay = 0.5f;
    float suppNormCounter = 0;

    public float strikableRadius = 1;
    bool issuppressed = false;  // Not the bool for actual mechanic
    bool canStrike = true;      // Again, not related to mechanics
    int randomItem;
    public float debrisTimer = 0.35f;
    float debrisTimerAtStart;
    CanPush selectedTarget;
    float gruntTimer = 0f;
    soundController AudioController;

    //[SerializeField] TextMeshProUGUI suppText;  //Placeholder stuff
    [SerializeField] Transform flankCheckPos;   //This should be near the bottom of the enemy

    // Start is called before the first frame update
    void Start()
    {
        targetIcon.enabled = false;

        debrisTimerAtStart = debrisTimer;
        AudioController = GameObject.Find("SoundsController").GetComponent<soundController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (suppNormCounter > suppNormDelay)    // Dont normalize immediately
        {
            if (suppression > 0)
            {
                suppression -= Time.deltaTime * suppressionNormSpeed;

                if (suppression < 0)    // Can't be less than 0
                {
                    suppression = 0;
                }
            }
            
        }
        else
        {
            suppNormCounter += Time.deltaTime;
        }
        LoadingBar.fillAmount = suppression;


        WaitAndStrike();
        //suppText.text = suppression.ToString();

        if (gruntTimer <= 2f)
        {
            gruntTimer += Time.deltaTime;
        }
    }
    private void LateUpdate()
    {
       // issuppressed = false;

    }
    // IMPLEMENT DMG LATER
    public void takeDamage()
    {
        Debug.Log("Enemy has taken damage");
        if (suppression >= 0.9f)
        {
            if (GetComponentInChildren<PuppetMaster>())
            {
                AudioController.playSound(AudioController.death);

                Debug.Log("enemy ded.");
                GetComponentInChildren<PuppetMaster>().state = PuppetMaster.State.Dead;
                Destroy(GetComponentInChildren<EnemyBehavior>());
                Destroy(canvas);
                Destroy(this);
            }
            else    // just for dummies
            {
                Debug.Log("Ugh. Im dead. i was a dummy");
            }

        }
    }

    public Transform getAimPos()
    {
        return aimPos;
    }

    public bool isFlanked(Vector3 gunCheckPos, GameObject attacker)
    {
        RaycastHit hit;
        Physics.Raycast(gunCheckPos - flankCheckPos.position, flankCheckPos.position, out hit, Mathf.Infinity);
        Debug.Log("flank check: :" + hit.transform.gameObject + " " + hit.transform.parent.gameObject + "Compared to attacker: " + attacker);
        //if (Physics.Raycast(gunCheckPos - flankCheckPos.position, flankCheckPos.position, out hit, Mathf.Infinity))
        Debug.DrawRay(gunCheckPos, flankCheckPos.position - gunCheckPos, Color.red, 1.5f);

        if (Physics.Raycast(gunCheckPos, flankCheckPos.position - gunCheckPos, out hit, Mathf.Infinity))
        {
            //Debug.DrawRay(flankCheckPos.position, gunCheckPos - flankCheckPos.position, Color.blue, 1.5f);
           // Debug.DrawLine(flankCheckPos.position, hit.point,Color.blue,10);
            if (hit.transform.IsChildOf(this.transform) || hit.transform.gameObject == this.gameObject || hit.transform.gameObject == this.transform.parent)
            {
                Debug.Log("flanked.");
                return true;
            }
        }
        else
        {
            Debug.DrawRay(flankCheckPos.position, gunCheckPos - flankCheckPos.position, Color.red, 1.5f);

        }
        Debug.Log("not flanked");
        return false;
    }

    public void getSupressed(float amount, Vector3 shooterPos)
    {
        //Debug.Log("Getting suppressed by: " + amount);
        suppression += amount;
        if (gruntTimer >= 1f)
        {
            AudioController.playSound(AudioController.grunts[Random.Range(0, AudioController.grunts.Length)]);
            gruntTimer = 0f;
        }

        suppNormCounter = 0;

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

    public float getSuppressionVar()
    {
        return suppression;
    }

    public GameObject getChest()
    {
        return chest;
    }
}
