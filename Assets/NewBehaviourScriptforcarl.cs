using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScriptforcarl : MonoBehaviour
{
    public PierInputManager manager;
    public PierInputManager.ButtonName myButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.GetButtonDown(myButton))
        {

        }
    }
}
