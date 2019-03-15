﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class ErdemGun : MonoBehaviour
{
    [SerializeField] Transform gunBarrel, aimPoint;
    bool isShooting = false;

    [SerializeField] float verticalSpread, horizontalSpread;

    Quaternion defaultRot;

    [SerializeField] LookAtIK lookAtScript;
    Quaternion defaultGunCntrlRot;
    float LOCK_ANGLE = 20f;

    [SerializeField] GameObject trailObj;


    float MAX_SUPRESS_DISTANCE = 10f;
    float MAX_SUPPRESSION_PERC = .9f;

    // Start is called before the first frame update
    void Start()
    {
        defaultRot = gunBarrel.transform.localRotation;
        //lookAtScript = transform.parent.GetComponent<LookAtIK>();
        defaultGunCntrlRot = lookAtScript.transform.localRotation;

    }

    // Update is called once per frame
    void Update()
    {
        

        Enemy[] Enemies = FindObjectsOfType<Enemy>();
        float minAngle = 180;
        Enemy lockedEnemy = null;
        foreach (Enemy enemy in Enemies)
        {
            if (Vector3.Angle(aimPoint.position - gunBarrel.position, enemy.transform.position - gunBarrel.position) < minAngle && Vector3.Angle(aimPoint.position - gunBarrel.position, enemy.transform.position - gunBarrel.position) < LOCK_ANGLE)
            {
                minAngle = Vector3.Angle(aimPoint.position - gunBarrel.position, enemy.transform.position - gunBarrel.position);
                lockedEnemy = enemy;
            }
        }
        if (lockedEnemy)
        {
            lookAtScript.solver.target = lockedEnemy.getAimPos();
            lookAtScript.solver.IKPositionWeight = 0.5f;
        }
        else
        {
            lookAtScript.solver.target = null;
            lookAtScript.solver.IKPositionWeight = 0f;
        }




        if (!isShooting && Input.GetAxis("Fire1") > 0.2f)
        {
            isShooting = true;
            //startShooting();
            shoot();
        }
        else if (isShooting && Input.GetAxis("Fire1") < 0.2f)
        {
            isShooting = false;
            gunBarrel.transform.localRotation = defaultRot;
            Debug.Log("spread reset");
        }

        //RaycastHit hit;
        //if (Physics.SphereCast(aimPoint.position, 1000f, (aimPoint.position - gunBarrel.position) * 100f, out hit))
        //{
        //    if (hit.transform.GetComponent<Enemy>() || hit.transform.GetComponentInParent<Enemy>())
        //    {
        //        Debug.Log("Locked to enemy!");
        //        lookAtScript.solver.target = hit.transform;
        //        lookAtScript.solver.IKPositionWeight = 0.5f;
        //    }
        //    else
        //    {
        //        lookAtScript.transform.localRotation = defaultGunCntrlRot;
        //        lookAtScript.solver.IKPositionWeight = 0;
        //    }
        //}
        //else
        //{
        //    lookAtScript.transform.localRotation = defaultGunCntrlRot;
        //    lookAtScript.solver.IKPositionWeight = 0;
        //}


        //GetComponent<LookAtIK>().solver.target 
    }

    public void startShooting()
    {



        // dir = Quaternion.AngleAxis(Random.Range(-horizontalSpread, horizontalSpread), this.transform.forward));

        // Check if something is hit
        


        // Calculate suppression
    }

    public void shoot()
    {
        gunBarrel.Rotate(Vector3.up * Random.Range(-horizontalSpread, horizontalSpread));
        gunBarrel.Rotate(Vector3.right * Random.Range(-verticalSpread, verticalSpread));
        Vector3 dir = aimPoint.position - gunBarrel.position;
        dir.Normalize();

        RaycastHit hit;

        GetComponent<Gun>().fire(new Ray(gunBarrel.position, dir * 1000));

        if (Physics.Raycast(gunBarrel.position, dir, out hit))
        {
            


            if (hit.transform.GetComponent<Enemy>())
            {
                hit.transform.GetComponent<Enemy>().takeDamage();
            }
            else if (hit.transform.GetComponent<HealthManager>())
            {
                // Implement damage
            }
            
        }
        GameObject trail = Instantiate(trailObj, gunBarrel.transform.position, Quaternion.identity);

        var broadcaster = hit.collider;

        if (broadcaster != null)
        {
            Debug.Log("moving trail to hit point");
            trail.GetComponent<TrailRenderer>().AddPosition(hit.point);
        }
        else
        {
            Debug.Log("moving trail to  forward");
            trail.GetComponent<TrailRenderer>().AddPosition(gunBarrel.position + (dir * 50));
        }



        //trail.transform.position = hit.transform.position;

        Debug.DrawRay(gunBarrel.position, dir * 100, Color.yellow, 10f);

    }


    void supress(Vector3 linePnt, Vector3 lineDir)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            Vector3 nearestPos = NearestPointOnLine(linePnt, lineDir, enemy.transform.position);
            if (Vector3.Distance(nearestPos, enemy.transform.position) <= MAX_SUPRESS_DISTANCE) // Close enough to supress
            {
                enemy.getSupressed(MAX_SUPPRESSION_PERC * (Vector3.Distance(nearestPos, enemy.transform.position) / MAX_SUPRESS_DISTANCE));
            }

        }
    }

    public static Vector3 NearestPointOnLine(Vector3 linePnt, Vector3 lineDir, Vector3 pnt)
    {
        lineDir.Normalize();//this needs to be a unit vector
        var v = pnt - linePnt;
        var d = Vector3.Dot(v, lineDir);
        return linePnt + lineDir * d;
    }
}
