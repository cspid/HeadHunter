using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour {

	public float timeLength = 1;
	float currentTime;
	bool On;
	
	// Update is called once per frame
	void Update () {
		currentTime = timeLength - Time.deltaTime;
		if(currentTime <= 0){
			if(On == true){
				On = false;
			} else {
				On = true;
			}
		}
		if(On == true){
			transform.GetChild(0).gameObject.SetActive(true);         
		}else{
			transform.GetChild(0).gameObject.SetActive(false);         
		}                
	}
}
