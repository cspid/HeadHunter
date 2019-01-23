using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionFollowUp : MonoBehaviour
{
    public Transform end;
    public Transform start;
    public float lerpTime = 1;
    public UnityEvent InteractionDone;
    public AnimationCurve myCurve;
    public bool lerp;
    public float timer;
    public void Update()
    {
        if (lerp)
        {
            timer += Time.deltaTime;
            if(timer >= lerpTime)
            {
                lerp = false;
                //timer = 0;
                OnInteractionDone();
            }
            transform.position = Vector3.Lerp(start.position, end.position, LerpUtility.Curve(timer, lerpTime, myCurve));
        }
    }
    public void OnInteractionDone()
    {
        InteractionDone.Invoke();
    }
    [ContextMenu("start Lerp")]

    public void StartLerp()
    {
        lerp = true;
    }
    public void ResetLerp()
    {
        timer = 0;

        transform.position = Vector3.Lerp(start.position, end.position, LerpUtility.Curve(timer, lerpTime, myCurve));

    }
   
}
