using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform gunBarrel, aimPoint;
    bool isShooting = false;

    [SerializeField] float verticalSpread, horizontalSpread;

    Quaternion defaultRot;
    // Start is called before the first frame update
    void Start()
    {
        defaultRot = gunBarrel.transform.localRotation;
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
            gunBarrel.transform.localRotation = defaultRot;
            Debug.Log("spread reset");
        }
    }

    public void startShooting()
    {



        // dir = Quaternion.AngleAxis(Random.Range(-horizontalSpread, horizontalSpread), this.transform.forward));

        // Check if something is hit
        


        // Calculate suppression
    }

    void shoot()
    {
        gunBarrel.Rotate(Vector3.up * Random.Range(-horizontalSpread, horizontalSpread));
        gunBarrel.Rotate(Vector3.right * Random.Range(-verticalSpread, verticalSpread));
        Vector3 dir = aimPoint.position - gunBarrel.position;
        dir.Normalize();

        RaycastHit hit;
        if (Physics.Raycast(gunBarrel.position, dir, out hit))
        {
            if (hit.transform.GetComponent<Enemy>())
            {
                hit.transform.GetComponent<Enemy>().takeDamage();
            }
            
        }
        Debug.DrawRay(gunBarrel.position, dir * 100, Color.red, 10f);

    }
}
