using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Strafe : MonoBehaviour {

	private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
	public Vector3 m_Move;
	float vel = 1;
	Rigidbody rb;
	public Transform torso;
	public Text topRot;
	public Text bottomRot;
	public float angleDeadZone = 30;
	public float catchUpSpeed = 0.1f;
	public bool catchUp;
	public Vector3 _newVelocity;
	public float startVel = 1;
	public float runVel = 2;
	StrafeAnimController strafeAnimController;
	float strafeAnimSpeed;
	float strafeAnimStartSpeed;
    public float h;
    public float v;

    public void setH(float val)
    {
        h = val;
    }
    public void setV(float val)
    {
        v = val;
    }
    private void Start()
    {
		strafeAnimController = GetComponent<StrafeAnimController>();
		strafeAnimStartSpeed = strafeAnimController.animSpeed;
		startVel = vel;
		rb = GetComponent<Rigidbody>();
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

       
    }
   

	// Update is called once per frame
	void Update()
	{

		//Leg Movement
		// h = CrossPlatformInputManager.GetAxis("HorizontalP1");
		// v = CrossPlatformInputManager.GetAxis("VerticalP1");
        // set by a blueprint 

		// calculate move direction to pass to character
		if (m_Cam != null)
		{
			// calculate camera relative direction to move:
			m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;

		}
		else
		{
			// we use world-relative directions in the case of no main camera
			m_Move = v * Vector3.forward + h * Vector3.right;
		}

		// _newVelocity = new Vector3();

		_newVelocity = v * m_CamForward * vel + h * m_Cam.right * vel;

		rb.velocity = _newVelocity;
//        print(_newVelocity);

		if (Input.GetButton("Run"))
		{
			print("RUN");
			    vel = runVel;
			    strafeAnimSpeed = strafeAnimStartSpeed * (vel/startVel);
		} else {
			    vel = startVel;
			    strafeAnimSpeed = strafeAnimStartSpeed;
	         }
        
		//show movement
        Color color1 = new Color(255, 255, 1, 1);
		Debug.DrawRay(transform.position, _newVelocity * 10, color1);
        Debug.DrawRay(torso.position, (torso.forward) * 10, color1);

		//show directions
		Color color = new Color(255, 1, 1, 1);
		Debug.DrawRay(transform.position, transform.forward * 10, color);
   		Debug.DrawRay(torso.position, (torso.forward) * 10, color);

        //Get angle difference
		Vector3.Angle(torso.forward, transform.forward);
              
		if (Vector3.Angle(torso.forward, transform.forward) > angleDeadZone)
        {
			catchUp = true;
        }

		if (catchUp == true)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, torso.forward, catchUpSpeed, 0.0f);
            Vector3 offsetDir = Vector3.RotateTowards(torso.forward, transform.forward, catchUpSpeed, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDir);
            torso.rotation = Quaternion.LookRotation(offsetDir);

			if (Vector3.Angle(torso.forward, transform.forward) < 10) 
			{ 
				catchUp = false; 
			}
        }




	}
}
