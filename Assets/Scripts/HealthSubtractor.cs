using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSubtractor : MonoBehaviour
{

	public int punchDamage = 15;
	public bool canDamage = true;
	HealthManager thisManager;
	public float delayTime = 1.0f;
	float startDelayTime = 1.0f;


    void Start()
    {
		startDelayTime = delayTime;
		thisManager = GetComponentInParent<HealthManager>();
    }

	void Update()
	{
        //if (canDamage == false)
      //  {
      //      DamageWait();
      //  }
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 9)
		{
			if (canDamage == true)
			{
				print("damage");
				//Ensure we don't change our own script
				if (collision.gameObject.transform.GetComponentInParent<HealthManager>() != thisManager)
				{
					collision.gameObject.transform.GetComponentInParent<HealthManager>().LoseHealth(punchDamage);
                    //canDamage = false;   //wait a second so we dont trigger multiple instances
                    canDamage = false;

                }
			}
		}
	}

	void DamageWait(){
		delayTime -= Time.deltaTime;
		if (delayTime <= 0){
			canDamage = true;
			delayTime = startDelayTime;
		}
	}
}
