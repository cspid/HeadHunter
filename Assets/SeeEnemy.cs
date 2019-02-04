using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeEnemy : MonoBehaviour
{
    public GameObject body;
    public Renderer rend;
    public float a = 0;
    float opaque = 1;
    public bool EnemyVisable;
    float speed = 0.1f;
    bool canClone;
    GameObject bodClone;

    private void Start()
    {
        rend = body.GetComponent<Renderer>();
    }

    void Update()
    {
        // rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, a);
        if (EnemyVisable == true)
        {
            canClone = true;

            if (rend.material.color.a < opaque)
            {
                a = a + speed;
                rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, a);
            }
        }
        if (EnemyVisable == false)
        {
            //if (rend.material.color.a > 0)
            //{
            //    a = a - speed;
            //    rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, a);
            //}

            if (rend.material.color.a > 0.3)
            {
                a = a - speed;
                rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, a);
            }
            else
            {
                if (canClone == true)
                {
                    bodClone = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
                    bodClone.GetComponent<Animator>().enabled= false;
                    bodClone.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

                    a = 0;
                    canClone = false;
                }
            }
            
        }
        if (rend.material.color.a > opaque) rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, opaque);
        if (rend.material.color.a < 0) rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, 0);




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