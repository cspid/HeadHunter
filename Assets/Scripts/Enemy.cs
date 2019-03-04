using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform aimPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
