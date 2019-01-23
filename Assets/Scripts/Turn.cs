using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Turn : MonoBehaviour {
	private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
	public float turnSpeed =1;
	public float rotSpeed = 1;
	public float rotOffset = -90;
    float rh;
    float rv;
    public void setH(float val)
    {
        rh = val;
    }
    public void setV(float val)
    {
        rv = val;
    }
    // Use this for initialization
    void Start () {
		if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
			print(m_Cam.gameObject.name);
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

	}
	
	// Update is called once per frame
	void Update () {
		//float rh = CrossPlatformInputManager.GetAxis("RightStickHorizontalP1");
       //  float rv = CrossPlatformInputManager.GetAxis("RightStickVerticalP1");
         //set by a blueprint 

	
        //Cam Relative
		if(Mathf.Abs(rh) >= Mathf.Epsilon && Mathf.Abs(rv) >= Mathf.Epsilon)
		{
			Vector3 targetDir = new Vector3();
            targetDir = rv * m_CamForward + rh * m_Cam.right;


            Quaternion offSet = new Quaternion(0, rotOffset, 0, 0);


            var targetDirection = new Vector3(rh, 0f, rv);
            targetDirection = Camera.main.transform.TransformDirection(targetDirection);
            targetDirection.y = 0.0f;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed /** Time.deltaTime*/);
            
            transform.rotation = newRotation;	
		}

		//Left-Right Spin
		//transform.Rotate(Vector3.up, rh * -rotSpeed);
		//transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + (rh * -rotSpeed), 0);



    }
}

//transform.RotateAround(Vector3.up, rh);
//transform.RotateAround(Vector3.up, rh);
