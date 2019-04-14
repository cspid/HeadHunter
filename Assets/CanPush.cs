using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanPush : MonoBehaviour
{
    Vector3 angleToPlayer;
    Rigidbody rb;
    public float force = 10f;
    public bool isSand;
    public bool isStone;
    public bool isMetal;
    public bool isFlesh;
    public bool isWood;
    public bool isTile;
    GameObject particles;
    float Timer;
    ParticleSystem particleEmitter;
    ParticleSystem.EmissionModule emmisionModule;
    bool isemitting;
    public float emissionRate = 10f;
    public int hitCount;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        particles = transform.GetChild(0).gameObject;
        particleEmitter = particles.GetComponent<ParticleSystem>();
        emmisionModule = particleEmitter.emission;
   
        //if (particles.gameObject.activeSelf == true)
        //{
        //    particles.gameObject.SetActive(false);
        //}
    }
    private void Update()
    {

        //if(isemitting == true)
        //{

        //}

        //if (isemitting == false)
        //{

        //}

    }

    // Update is called once per frame
    void Hit()
    {
        //angleToPlayer = 
    }

    public void getHit(Vector3 shooterPos)
    {
        particles.gameObject.SetActive(true);
        angleToPlayer = transform.position - shooterPos;
        rb.AddForce(angleToPlayer, ForceMode.Impulse);
        //Debug.Log("got hit! i explode in a visually impressive fashion!.");

        if (isSand) {
            print(particles.name);
            particles.transform.forward = angleToPlayer;
        }
        if (isMetal)
        {
            print(particles.name);
            particles.transform.forward = angleToPlayer;
        }
        if (isTile)
        {
            hitCount++;
           if(hitCount >= 1)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                print("fall");
            }
        }
    }
    public void StartParticles()
    {
        //isemitting = true;
        //particleEmitter.Play();
        //particles.gameObject.SetActive(true);
        if (isWood)
        {
            emmisionModule.rateOverTime = emissionRate;
            //print("Emit " + gameObject.name);
        }
        if (isMetal)
        {
            particles.SetActive(true);
        }
        if (isSand)
        {

        }
        if (isTile)
        {
            if (particles.activeSelf == false)
            {
                particles.SetActive(true);
            }
            emmisionModule.rateOverTime = emissionRate;
        }
    }

    public void StopParticles()
    {
        //isemitting = false;
        //particleEmitter.Stop();
        //particles.gameObject.SetActive(false);

        print("Stop Emit " + gameObject.name);
        if (isWood)
        {
            emmisionModule.rateOverTime = 0;
            //print("Emit " + gameObject.name);
        }
        if (isMetal)
        {
            particles.SetActive(false);
        }
        if (isSand)
        {
            emmisionModule.rateOverTime = 0;
            particles.SetActive(false);
        }
        if (isTile)
        {
            emmisionModule.rateOverTime = 0;
            //particles.SetActive(false);
        }
    }
}
