using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    // Use this for initialization

    public float maxZoom;       //max value for zooming out
    public float minZoom;       //max value for zooming in

 


    public Camera gameCamera;
   

	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

        CameraZoom();
        CameraMovement();
      
	}

    void CameraMovement()
    {

        //Press and hold Mouse button 0 and move mouse for camera movement
        if (Input.GetMouseButton(0))
        {
            this.transform.position += this.transform.right * Input.GetAxis("Mouse X") ;
            this.transform.position += this.transform.up * Input.GetAxis("Mouse Y");
        }

    }

    void CameraZoom()
    {
        //Scroll mouse to zoom in and zoom out
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (gameCamera.GetComponent<Camera>().orthographicSize >= minZoom)
            {
                gameCamera.GetComponent<Camera>().orthographicSize--;       //zoom in
            }

            
               
        }

        else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (gameCamera.GetComponent<Camera>().orthographicSize <= maxZoom)
            {
                gameCamera.GetComponent<Camera>().orthographicSize++;           //zoom out
            }
               
        }
        
    }


   
    
}
