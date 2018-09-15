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
		
	}
}
