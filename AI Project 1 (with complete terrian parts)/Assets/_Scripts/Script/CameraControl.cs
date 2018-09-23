﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    // Use this for initialization

    public float maxZoom;       //max value for zooming out
    public float minZoom;       //max value for zooming in

    public GameObject player;  // for player
    public Vector3 offset;
    private float heading;

    //Checking tag for environment camera
    public bool isenvoiso = false;

	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

        CheckingIso();
        if (isenvoiso)
        {
            Zoom();
        }
        else
        {
            CameraFollow();
            CameraRotation();
            Zoom();
        }
      
	}

    public void CameraFollow()
    {
      
        this.transform.position = player.transform.position + offset;
    }

    void CheckingIso()
    {
        if(this.gameObject.tag == "MapCam")
        {
            isenvoiso = true;
        }

        else
        {
            isenvoiso = false;
        }
    }

    void Zoom()
    {
       
        /*
         * Please note field of view decreases value for zooming in and increases its value for zooming out. 
         * Ideal value for maxzoom is 75 and ideal value for minzoom is 45 
         */
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            Debug.Log("Zooming in");
            if(this.GetComponent<Camera>().fieldOfView >= minZoom)
            {
                this.GetComponent<Camera>().fieldOfView--;
            }

            if(this.GetComponent<Camera>().fieldOfView <= minZoom)
            {
                Debug.Log("Enough zooming in");
            }
           
        }

        else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            Debug.Log("Zooming out");

            if(this.GetComponent<Camera>().fieldOfView <= maxZoom)
            {
                this.GetComponent<Camera>().fieldOfView++;
            }
            
            else if(this.GetComponent<Camera>().fieldOfView > maxZoom)
            {
                Debug.Log("Max zooming out!");
            }
        }
    }

    void CameraRotation()
    {
         heading = heading + Input.GetAxis("Mouse X") * Time.deltaTime * 180;
    
        this.transform.rotation = Quaternion.Euler(20, heading, 0);
    }
}
