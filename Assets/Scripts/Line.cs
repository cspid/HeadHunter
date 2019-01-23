using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

    //public GameObject cube;
    public LineRenderer lineRendererSpine;
    public LineRenderer lineRendererLegs;
    public LineRenderer lineRendererArms;


    //	public int lengthOfLineRenderer = 2;
    public Vector3 [] spine;
    public Vector3[] legs;
    public Vector3[] arms;
    //public Vector3[] legR;



    public Transform hips;
	public Transform Spine;
	public Transform spine1;
	public Transform spine2;
	public Transform neck;
	public Transform head;
	public Transform head1;

    public Transform toeL2;
    public Transform toeL1;
    public Transform footL;
    public Transform legL;
    public Transform upperLegL;

    public Transform upperLegR;
    public Transform legR;
    public Transform footR;
    public Transform toeR1;
    public Transform toeR2;

    public Transform handL;
    public Transform forearmL;
    public Transform armL;
    public Transform shoulderL;

    public Transform shoulderR;
    public Transform armR;
    public Transform forearmR;
    public Transform handR;









    //public GameObject point8;

    //	Vector3 point1;
    //	Vector3 point2;
    // Use this for initialization
    void Start () {
		//lineRenderer = GetComponent<LineRenderer> ();
		//points = new Vector3[2];
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		
		spine = new[]{ 
            hips.position, Spine.position, spine1.transform.position, spine2.transform.position, neck.transform.position, head.transform.position, head1.transform.position //point8.transform.localPosition, point3.transform.localPosition,
        };
       
        legs = new[]{
            toeL2.position, toeL1.position, footL.transform.position, legL.transform.position, upperLegL.transform.position, hips.transform.position, upperLegR.transform.position, legR.transform.position, footR.transform.position, toeR1.transform.position, toeR2.transform.position //point8.transform.localPosition, point3.transform.localPosition,
        };

        arms = new[]{
            handL.position, forearmL.position, armL.transform.position, shoulderL.transform.position, neck.transform.position, shoulderR.transform.position, armR.transform.position, forearmR.transform.position, handR.transform.position 
        };

        lineRendererSpine.SetPositions(spine);
        lineRendererLegs.SetPositions(legs);
        lineRendererArms.SetPositions(arms);


    }

}
