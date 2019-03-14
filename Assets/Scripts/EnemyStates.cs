using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates : MonoBehaviour
{

    public enum State { Stealth, Attack, Cover, Death };
    public State state;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Stealth:
                Stealth();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Cover:
                Cover();
                break;
            case State.Death:
                Death();
                break;

        }
    }

    void Stealth()
    {
        Debug.Log("Stealth");
    }
    void Attack()
    {
        Debug.Log("Attack");
    }
    void Cover()
    {
        Debug.Log("Cover");
    }
    void Death()
    {
        Debug.Log("Death");
    }
}
