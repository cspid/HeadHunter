using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swapDestructable : MonoBehaviour {

	public Transform brokenVersion;

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.layer == 9)	{
			brokenVersion.gameObject.SetActive(true);
			Destroy(gameObject);
		}
	}
}
