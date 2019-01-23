using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RootMotion.Dynamics;
public class RoundManager : MonoBehaviour
{
    public UnityEvent OnAllDead;
    public List<PuppetMaster> enemies;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        bool alldead = true;
        foreach(PuppetMaster puppet in enemies)
        {
            if (puppet.isAlive)
            {
                alldead = false;
            }
        }
        if (alldead)
        {
            OnAllDead.Invoke();
        }
	}
}
