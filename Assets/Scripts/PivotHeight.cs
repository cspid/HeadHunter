using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotHeight : MonoBehaviour
{
    public Transform heightToMatch;
    public float offset;
    public float buffer = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y - heightToMatch.position.y < -buffer || transform.position.y - heightToMatch.position.y > buffer)
        {
             transform.position = new Vector3(transform.position.x, heightToMatch.transform.position.y + offset, transform.position.z);
        }
    }
}
