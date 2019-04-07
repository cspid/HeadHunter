using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RadialProgress : MonoBehaviour
{

    

    [Tooltip("The representation of the loading circle.")]
    public Image LoadingBar;

    [Tooltip("The current value of the fill of the circle image.")]
    float currentValue;

    [Tooltip("The number that indicates how fast the circle will fill.")]
    public float speed;

    // Use this for initialization
    void Start()
    {
        currentValue = 100;
    }

    // Update is called once per frame
    void Update()
    {
 
        //Checks to see if current value is still above 0.
        if(currentValue > 0)
        {
            currentValue -= speed * Time.deltaTime;

        }

        //The fill amount is equal to the current value.
        LoadingBar.fillAmount = currentValue * 0.01f;
    }
}
