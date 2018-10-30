using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIsoFollow : MonoBehaviour {

    // Use this for initialization

    public Vector3 offset;
    private GameObject player;
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void LateUpdate () {

        CameraFollow();
	}

    void CameraFollow()
    {
        
        this.transform.position = player.transform.position + offset;
       // this.transform.LookAt(player.transform);
    }
}
