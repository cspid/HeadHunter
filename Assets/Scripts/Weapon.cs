using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Weapon : MonoBehaviour
{
    [SerializeField] int playerId = 0;
    [SerializeField] GameObject target;
    [SerializeField] Transform muzzlePos;

    [SerializeField] Transform pivot;

    //[SerializeField] Transform firePoint;
    [SerializeField] ParticleSystem muzzleEffect;

    float LOCK_ANGLE = 40f;

    float MAX_SUPRESS_DISTANCE = 10f;
    float MAX_SUPPRESSION_PERC = .15f;

    float maxSuppressionAngle = 30f;

    int MAX_AMMO = 30;
    int ammo;

    float fireRate = 0.1f;
    float shootCounter = 0;

    bool isReloading = false;

    float reloadTime = 3.0f;
    float reloadCounter = 0;

    bool isShooting = false;
    //GameObject muzzleFlash;

    Player player;

    //soundController AudioController;
    AudioSource audioSource;
        
        // Start is called before the first frame update
    void Start()
    {
        //AudioController = GameObject.Find("SoundsController").GetComponent<soundController>();
        audioSource = GetComponent<AudioSource>();
        player = ReInput.players.GetPlayer(playerId);
        ammo = MAX_AMMO;
        //muzzleFlash = firePoint.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // TOO EXPENSIVE >>> DECREASE CALL AMOUNT LATER >>>>
        findTarget();

        if (player.GetButtonDown("R1")) //HACK
        {
            muzzleEffect.Play();
            if (target)
            {
                target.GetComponentInParent<Enemy>().die();
                findTarget();
            }
        }


        if (!isShooting && player.GetButtonDown("R2"))
            //Input.GetAxis("Fire1") > 0.2f)
        {
            isShooting = true;
            audioSource.Play();
            //muzzleFlash.SetActive(true);
            //startShooting();
        }
        else if (isShooting && player.GetButtonUp("R2"))
            //Input.GetAxis("Fire1") < 0.2f)
        {
            isShooting = false;
            audioSource.Stop();
            //muzzleFlash.SetActive(false);
        }


        if (isShooting && !isReloading)
        {
            shootCounter += Time.deltaTime;

            if (shootCounter >= fireRate)
            {
                shoot();
                ammo--;
                shootCounter = 0;
               // Debug.Log("ammo: " + ammo);

                if (ammo <= 0)
                {
                    isReloading = true;
                }
            }
        }
        else if (isReloading)
        {
            reloadCounter += Time.deltaTime;

            if (reloadCounter >= reloadTime)
            {
                isReloading = false;
                ammo = MAX_AMMO;
                reloadCounter = 0;
            }
        }
    }

    void findTarget()
    {
        Enemy[] Enemies = FindObjectsOfType<Enemy>();
        float minAngle = 180;
        Enemy lockedEnemy = null;
        foreach (Enemy enemy in Enemies)
        {
            if (Vector3.Angle(pivot.forward, enemy.getChest().transform.position - pivot.transform.position) < minAngle
                && Vector3.Angle(pivot.forward, enemy.getChest().transform.position - pivot.transform.position) < LOCK_ANGLE)
            {
                minAngle = Vector3.Angle(pivot.forward, enemy.getChest().transform.position - pivot.transform.position);
                lockedEnemy = enemy;
            }
        }

        if (target && target.GetComponentInParent<Enemy>())
        {
            if ((lockedEnemy && target != lockedEnemy.gameObject) || (lockedEnemy == null))
            {
                target.GetComponentInParent<Enemy>().cancelTarget();
            }
        }


        if (lockedEnemy)
        {
            //lookAtScript.solver.target = lockedEnemy.getAimPos();
            //lookAtScript.solver.IKPositionWeight = 0.5f;
            target = lockedEnemy.getChest();
            //target.GetComponent<Enemy>().getTargeted();
            lockedEnemy.getTargeted();
        }
        else
        {
            target = null;
        }
    }

    void shoot()
    {
        Debug.Log("Pew pew");
        muzzleEffect.Play();
        if (target)
        {
            target.GetComponentInParent<Enemy>().StrikableObjects(target.GetComponentInParent<Enemy>().getEyePos().position, 1.5f, muzzlePos.transform.position);

            RaycastHit hit;
            Physics.Raycast(GetComponent<PlayerDanger>().getEyePos().position, target.GetComponentInParent<Enemy>().getEyePos().position - GetComponent<PlayerDanger>().getEyePos().position, out hit, Mathf.Infinity);
            Debug.DrawRay(GetComponent<PlayerDanger>().getEyePos().position, target.GetComponentInParent<Enemy>().getEyePos().position - GetComponent<PlayerDanger>().getEyePos().position, Color.yellow, 10f);
            //Debug.Log("eyeline check: " + hit.transform.gameObject);
            if (hit.transform.IsChildOf(target.transform) || hit.transform.gameObject == target.transform || hit.transform == target.transform.parent)
            {
                supress(muzzlePos.transform.position, target.transform.position - pivot.transform.position);

                //if (target.GetComponent<Enemy>())
                {
                    if (target.GetComponentInParent<Enemy>().isFlanked(muzzlePos.position, this.transform.gameObject))
                    {
                        target.GetComponentInParent<Enemy>().takeDamage();
                        findTarget();
                    }

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
            if (Vector3.Distance(enemy.transform.position, target.transform.position) <= MAX_SUPRESS_DISTANCE
                && Vector3.Angle(target.transform.position - pivot.transform.position, enemy.transform.position - pivot.transform.position) <= maxSuppressionAngle)
            {
                enemy.getSupressed(MAX_SUPPRESSION_PERC * 
                    ((MAX_SUPRESS_DISTANCE - Vector3.Distance(enemy.transform.position, target.transform.position)) / MAX_SUPRESS_DISTANCE)
                    * ((maxSuppressionAngle - Vector3.Angle(target.transform.position - pivot.transform.position, enemy.transform.position - pivot.transform.position)) / maxSuppressionAngle),
                    muzzlePos.transform.position);
               // Debug.Log("suppression: " + MAX_SUPPRESSION_PERC + " * " + ((MAX_SUPRESS_DISTANCE - Vector3.Distance(enemy.transform.position, target.transform.position)) / MAX_SUPRESS_DISTANCE) + " * " + (maxSuppressionAngle - Vector3.Angle(target.transform.position - pivot.transform.position, enemy.transform.position - pivot.transform.position) / maxSuppressionAngle));
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
