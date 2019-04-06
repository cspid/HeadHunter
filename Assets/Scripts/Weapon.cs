using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject target;

    float MAX_SUPRESS_DISTANCE = 10f;
    float MAX_SUPPRESSION_PERC = .9f;

    bool isShooting = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShooting && Input.GetAxis("Fire1") > 0.2f)
        {
            isShooting = true;
            //startShooting();
            shoot();
        }
        else if (isShooting && Input.GetAxis("Fire1") < 0.2f)
        {
            isShooting = false;
        }
    }

    void shoot()
    {
        Debug.Log("Pew pew");
        if (target)
        {
            supress(this.transform.position, target.transform.position - this.transform.position);

            if (target.GetComponent<Enemy>())
            {
                if (target.GetComponent<Enemy>().isFlanked(this.transform.position, this.gameObject))
                {
                    target.GetComponent<Enemy>().takeDamage();
                }

            }
        }
        
    }

    void setTarget(GameObject tar)
    {
        target = tar;
    }

    void supress(Vector3 linePnt, Vector3 lineDir)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, target.transform.position) <= MAX_SUPRESS_DISTANCE)
            {
                enemy.getSupressed(MAX_SUPPRESSION_PERC * (MAX_SUPRESS_DISTANCE - Vector3.Distance(enemy.transform.position, target.transform.position)) / MAX_SUPRESS_DISTANCE);
            }
        }

        //foreach (Enemy enemy in enemies)
        //{
        //    Vector3 nearestPos = NearestPointOnLine(linePnt, lineDir, enemy.transform.position);
        //    if (Vector3.Distance(nearestPos, enemy.transform.position) <= MAX_SUPRESS_DISTANCE) // Close enough to supress
        //    {
        //        enemy.getSupressed(MAX_SUPPRESSION_PERC * (Vector3.Distance(nearestPos, enemy.transform.position) / MAX_SUPRESS_DISTANCE));
        //    }

        //}
    }

    public static Vector3 NearestPointOnLine(Vector3 linePnt, Vector3 lineDir, Vector3 pnt)
    {
        lineDir.Normalize();//this needs to be a unit vector
        var v = pnt - linePnt;
        var d = Vector3.Dot(v, lineDir);
        return linePnt + lineDir * d;
    }
}
