using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDanger : MonoBehaviour
{
    [SerializeField] Transform flankCheckPos;   //This should be near the bottom of the player
    float dangerLevel = 0f;
    float dangerNormSpeed = 0.05f;


    float coverDangerMultiplier = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dangerLevel > 0)
        {
            dangerLevel -= Time.deltaTime * dangerNormSpeed;

            if (dangerLevel < 0)
            {
                dangerLevel = 0;
            }

        }
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

    public void getSuppressed(float amount, bool isFlanked)
    {
        Debug.Log("Getting threated by: " + amount);

        if (isFlanked)
        {
            amount *= coverDangerMultiplier;
        }

        dangerLevel += amount;

        if (dangerLevel > 1 && isFlanked)
        {
            // DEATH!!!!

        }
    }

}
