using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform aimPos;
    float suppression = 0;
    float suppressionNormSpeed = 0.1f;

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

    public void getSupressed(float amount)
    {
        suppression += amount;

        if (suppression > 1)
        {
            suppression = 1;
        }
    }
}
