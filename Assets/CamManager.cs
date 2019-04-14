using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public lerpPosition lerpPosScript;
    public lerpRotation lerpRotScript;

    // Start is called before the first frame update
    void Start()
    {
        //lerpPosScript = GetComponent<lerpPosition>();
    }

    public void MoveCam(Transform newPos)
    {
        //position
        lerpPosScript.startPositon = lerpPosScript.transform.position;
        lerpPosScript.endPosition = newPos.position;

        //rotation
        lerpRotScript.startRotation = lerpPosScript.transform.rotation;
        lerpRotScript.endRotation = newPos.rotation;

        lerpPosScript.isLerping = true;
        lerpRotScript.isLerping = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == lerpPosScript.endPosition)
        {
            lerpPosScript.isLerping = false;
        }
        if (transform.rotation == lerpRotScript.endRotation)
        {
            lerpRotScript.isLerping = false;
        }
    }
}
