using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public lerpPosition lerpPosScript;
    // Start is called before the first frame update
    void Start()
    {
        //lerpPosScript = GetComponent<lerpPosition>();
    }

    public void MoveCam(Transform newPos)
    {
        lerpPosScript.startPositon = lerpPosScript.transform.position;
        lerpPosScript.endPosition = newPos.position;
        lerpPosScript.isLerping = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == lerpPosScript.endPosition)
        {
            lerpPosScript.isLerping = false;
        }
    }
}
