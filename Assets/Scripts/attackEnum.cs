using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class attackEnum : MonoBehaviour
{

    public enum attacks { leftPunch, rightPunch ,leftKick, rightKick }
    public UnityEvent leftPunch;
    public UnityEvent rightPunch;
    public UnityEvent leftKick;
    public UnityEvent rightKick;



    public void callLeftPunch()
    {
        leftPunch.Invoke();
    }
    public void callRightPunch()
    {
        rightPunch.Invoke();

    }
    public void callLeftKick()
    {
        leftKick.Invoke();

    }
    public void callRightKick()
    {
        rightKick.Invoke();

    }
}
