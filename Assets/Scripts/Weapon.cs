﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject target;

    [SerializeField] Transform pivot;
    float LOCK_ANGLE = 40f;

    float MAX_SUPRESS_DISTANCE = 10f;
    float MAX_SUPPRESSION_PERC = .3f;

    bool isShooting = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // TOO EXPENSIVE >>> DECREASE CALL AMOUNT LATER >>>>
        findTarget();




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

    void findTarget()
    {
        Enemy[] Enemies = FindObjectsOfType<Enemy>();
        float minAngle = 180;
        Enemy lockedEnemy = null;
        foreach (Enemy enemy in Enemies)
        {
            if (Vector3.Angle(pivot.forward, enemy.transform.position - this.transform.position) < minAngle
                && Vector3.Angle(pivot.forward, enemy.transform.position - this.transform.position) < LOCK_ANGLE)
            {
                minAngle = Vector3.Angle(pivot.forward, enemy.transform.position - this.transform.position);
                lockedEnemy = enemy;
            }
        }

        if (target)
        {
            if ((lockedEnemy && target != lockedEnemy.gameObject) || (lockedEnemy == null))
            {
                target.GetComponent<Enemy>().cancelTarget();
            }
        }


        if (lockedEnemy)
        {
            //lookAtScript.solver.target = lockedEnemy.getAimPos();
            //lookAtScript.solver.IKPositionWeight = 0.5f;
            target = lockedEnemy.gameObject;
            target.GetComponent<Enemy>().getTargeted();
        }
        else
        {
            target = null;
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
