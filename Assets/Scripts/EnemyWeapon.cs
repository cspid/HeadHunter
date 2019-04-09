using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{

    //float MAX_SUPRESS_DISTANCE = 5f;
    float MAX_SUPPRESSION_PERC = .25f;

    bool isShooting = false;
    [SerializeField] PlayerDanger target;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void shoot()
    {
        Debug.Log("enemy pew pew");
        target.getSuppressed(MAX_SUPPRESSION_PERC, target.isFlanked(this.transform.position, this.gameObject));
    }
}
