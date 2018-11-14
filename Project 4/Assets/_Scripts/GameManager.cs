using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// public enum gameStates {playing, death};

public class GameManager : MonoBehaviour {

    // Use this for initialization
    /*
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
    /*
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
*/
    int range = 0;
    private GameManager GM;
    public Canvas influenceCanvas;
    public GameObject blackUnit1;
    public GameObject yellowUnit1;
    public GameObject blueUnit1;
    public GameObject whiteUnit1;
    public GameObject blackUnit2;
    public GameObject yellowUnit2;
    public GameObject blueUnit2;
    public GameObject whiteUnit2;

    private GameObject pointPrefab;

    public Text toGenText;
    public string toGenString;

    public Text teamNumText;
    public string teamNumString;
    void Start () {
        GM = this;
        influenceCanvas.gameObject.SetActive(true);

        toGenString = toGenText.text;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            GameObject point = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.point);
                Node objNode = Grid.Instance.NodeFromWorldPoint(hit.point);
                point = GameObject.Instantiate(pointPrefab, objNode.PositionInWorld, transform.rotation) as GameObject;
                //Node node = Grid.Instance.NodeFromWorldPoint(point.transform.position);
                if(pointPrefab.tag == "Team1")
                    Grid.Instance.SetValueAroundNode(objNode, range-1, 1);
                else
                    Grid.Instance.SetValueAroundNode(objNode, range-1, -1);


            }
        }
    }

    // Unit Selection and Canvas Text change
    public void toGenBlack() {
        toGenString = "black";
        toGenText.text = toGenString;
        if (teamNumText.text == "team1")
        {
            pointPrefab = blackUnit1;
            range = 4;
        }
        else {
            pointPrefab = blackUnit2;
            range = 4;
        }

    }
    public void toGenYellow()
    {
        toGenString = "yellow";
        toGenText.text = toGenString;
        if (teamNumText.text == "team1")
        {
            pointPrefab = yellowUnit1;
            range = 3;
        }
        else
        {
            pointPrefab = yellowUnit2;
            range = 3;
        }
    }
    public void toGenBlue()
    {
        toGenString = "blue";
        toGenText.text = toGenString;
        if (teamNumText.text == "team1")
        {
            pointPrefab = blueUnit1;
            range = 2;
        }
        else
        {
            pointPrefab = blueUnit2;
            range = 2;
        }
    }
    public void toGenWhite()
    {
        toGenString = "white";
        toGenText.text = toGenString;
        if (teamNumText.text == "team1")
        {
            pointPrefab = whiteUnit1;
            range = 1;
        }
        else
        {
            pointPrefab = whiteUnit2;
            range = 1;
        }
    }
    public void teamSelection1() {
        teamNumString = "team1";
        teamNumText.text = teamNumString;
    }

    public void teamSelection2()
    {
        teamNumString = "team2";
        teamNumText.text = teamNumString;
    }
}
