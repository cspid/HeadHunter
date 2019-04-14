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
    float startDelay = 0.2f;
    float startDelayAtStart = 0.2f;
    float stopDelay = 0.2f;
    float stopDelayAtStart = 0.2f;
    bool startDelaybool;
    bool stopDelaybool;
    public GameObject trail;
    GameObject bullet;
    lerpPosition lerpPosScript;




    // Start is called before the first frame update
    void Start()
    {
        startDelayAtStart = startDelay;

        stopDelayAtStart = stopDelay;

        rb = GetComponent<Rigidbody>();
        if (transform.childCount > 0)
        {
            particles = transform.GetChild(0).gameObject;
            particleEmitter = particles.GetComponent<ParticleSystem>();
            emmisionModule = particleEmitter.emission;
        }

        //if (particles.gameObject.activeSelf == true)
        //{
        //    particles.gameObject.SetActive(false);
        //}
    }
    void Update()
    {
        if(startDelaybool == true)
        {
            WaitToStart();
        }

        if (stopDelaybool == true)
        {
            WaitToEnd();
        }
    }

    // Update is called once per frame
    void Hit()
    {
        //angleToPlayer = 
    }

    public void getHit(Vector3 shooterPos)
    {
        if (particles != null)
        {
            particles.gameObject.SetActive(true);
        }
        angleToPlayer = transform.position - shooterPos;
        rb.AddForce(angleToPlayer, ForceMode.Impulse);
        //Debug.Log("got hit! i explode in a visually impressive fashion!.");

        if (isSand) {
            if (particles != null)
            {
                print(particles.name);
                particles.transform.forward = angleToPlayer;
            }
        }
        if (isMetal)
        {
            if (particles != null)
            {
                print(particles.name);
                particles.transform.forward = angleToPlayer;
            }
        }
        if (isTile)
        {
            hitCount++;
            if (hitCount >= 1)
            {
                if (particles != null)
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    print("fall");
                }
            }
        }
        bullet = Instantiate(trail, shooterPos, Quaternion.identity);
        lerpPosScript = bullet.GetComponent<lerpPosition>();
        lerpPosScript.startPositon = shooterPos;
        lerpPosScript.endPosition = transform.position;
        lerpPosScript.lerpDuration = 0.2f;
        lerpPosScript.isLerping = true;

    }
    public void StartParticles()
    {
        startDelaybool = true;
    }

    public void StopParticles()
    {
        stopDelaybool = true;
    }
    void WaitToStart()
    {
        startDelay -= Time.deltaTime;
        if(startDelay <= 0)
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
                if (particles != null)
                {
                    particles.SetActive(true);
                }
            }
            if (isSand)
            {

            }
            if (isTile)
            {
                if (particles != null)
                {
                    if (particles.activeSelf == false)
                    {
                        particles.SetActive(true);
                    }
                    emmisionModule.rateOverTime = emissionRate;
                }
            }
            startDelaybool = false;
        }
    }

    void WaitToEnd()
    {
        if (particles != null)
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
        stopDelaybool = false;
    }
}
