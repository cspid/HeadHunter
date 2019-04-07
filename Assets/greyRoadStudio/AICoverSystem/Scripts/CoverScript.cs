using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoverScript : MonoBehaviour
{
    public List<Cover> front = new List<Cover>();
    public List<Cover> rear = new List<Cover>();

    private void Start()
    {
        if (LayerMask.NameToLayer("Cover") == -1)
        {
            Debug.LogError("You must add a Layer named \"Cover\" in the Layer Manager. " +
                "At the top of the Inspector, click the Layer dropdown menu and click \"Add Layer.\"\n" +
                "Then type in \"Cover\" in one of the empty User Layer fields. \"Cover\" must be typed exactly as shown, with a capital \"C\".");
        }
        else
        {

            SetCoverLayer(transform);
            //Debug.Log(LayerMask.NameToLayer("Cover").ToString());
        }

    }

    private void SetCoverLayer(Transform cover)
    {
        cover.gameObject.layer = LayerMask.NameToLayer("Cover");
        foreach(Transform child in cover)
        {
            SetCoverLayer(child);
        }
    }

}

[System.Serializable]
public class Cover
{
    public Transform coverTransform;
    [HideInInspector]
    public bool isOccupied;
    public bool canCrouch;
}
