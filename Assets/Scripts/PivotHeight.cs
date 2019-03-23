using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotHeight : MonoBehaviour
{
    public Transform heightToMatch;
    public float offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, heightToMatch.transform.position.y + offset, transform.position.z);
    }
}
