using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeEnemy : MonoBehaviour
{
    public Renderer rend;
    float a = 0;
    float opaque = 1;
    bool EnemyVisable;
    float speed = 0.1f;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if(EnemyVisable == true)
        {
            if(rend.material.color.a < opaque)
            {
                a = a + speed;
                rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, a);
            }
            else
            {
                if (rend.material.color.a > 0)
                {
                    a = a - speed;
                    rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, a);
                }
            }
        }
    }

    public void EnemySpotted()
    {
        print("spotted");
        EnemyVisable = true;
    }

    public void EnemyHidden()
    {
        print("hidden");

        EnemyVisable = false;
    }
}