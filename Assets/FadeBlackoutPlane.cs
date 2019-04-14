using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBlackoutPlane : MonoBehaviour
{
    public Renderer material;
    Color color;
    public GameObject[] planeObject;
    public bool fadeOut;
    // Start is called before the first frame update
    void Start()
    {
         color = planeObject[0].GetComponent<Renderer>().material.color;

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IsPlayer>() != null)
        {
            fadeOut = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(fadeOut == true && color.a > 0) { 
            color.a -= Time.deltaTime;
            
            for (int i = 0; i < planeObject.Length; i++)
            {
                planeObject[i].GetComponent<Renderer>().material.color = color;
            }
        }
    }
}
