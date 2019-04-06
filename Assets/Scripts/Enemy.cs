using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Image LoadingBar;
    [SerializeField] Transform aimPos;
    float suppression = 0;
    float suppressionNormSpeed = 0.1f;

    //[SerializeField] TextMeshProUGUI suppText;  //Placeholder stuff
    [SerializeField] Transform flankCheckPos;   //This should be at the bottom of the enemy

    // Start is called before the first frame update
    void Start()
    {
        
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
        //suppText.text = suppression.ToString();
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

    public void getSupressed(float amount)
    {
        Debug.Log("Getting suppressed by: " + amount);
        suppression += amount;

        if (suppression > 1)
        {
            suppression = 1;
        }
    }
}
