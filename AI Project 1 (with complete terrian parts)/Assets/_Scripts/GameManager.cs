using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStates {playing, death};

public class GameManager : MonoBehaviour {

    // Use this for initialization

    public static GameManager GM;

    public gameStates gameState;

    public GameObject playerIns;

    public GameObject mainCanvas;

    public GameObject introCanvas;

    public GameObject referCanvas;

    public GameObject deathCanvas;


    //Cameras
    public Camera thirdpersonCamera;
    public Camera IsoCamera;
    public bool isocam = false;
    public bool thirdcam = true;
    public Camera isoplayerfollow;
    public bool isoplayer = false;

	void Start () {
        GM = this;
        gameState = gameStates.playing;

        if (mainCanvas) {
            mainCanvas.SetActive(true);
        }

        if (introCanvas) {
            introCanvas.SetActive(false);
        }

        if (referCanvas) {
            referCanvas.SetActive(false);
        }

        if (deathCanvas) {
            deathCanvas.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {

        CameraSwitch();
	}


    //Camera Switching code
    void CameraSwitch()
    {
        //keyboard input

        /* if (Input.GetKeyDown(KeyCode.M))
         {
             isocam = !isocam;
             thirdcam = !thirdcam;

         }
         */

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (thirdcam)
            {
                isocam = true;
                thirdcam = false;
                isoplayer = false;
            }


            else  if (isocam)
            {
                thirdcam = false;
                isoplayer = true;
                isocam = false;
            }

         else if (isoplayer)
            {
                isocam = false;
                thirdcam = true;
                isoplayer = false;
            }
        }
        if (isocam)
        {
            thirdpersonCamera.gameObject.SetActive(false);
            IsoCamera.gameObject.SetActive(true);
            isoplayerfollow.gameObject.SetActive(false);
            
        }

        if (thirdcam)
        {
            thirdpersonCamera.gameObject.SetActive(true);
            IsoCamera.gameObject.SetActive(false);
            isoplayerfollow.gameObject.SetActive(false);

        }

        if (isoplayer)
        {
            thirdpersonCamera.gameObject.SetActive(false);
            IsoCamera.gameObject.SetActive(false);
            isoplayerfollow.gameObject.SetActive(true);
        }
    }

   

}
