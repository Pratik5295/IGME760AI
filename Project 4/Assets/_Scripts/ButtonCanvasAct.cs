using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCanvasAct : MonoBehaviour {

    public GameObject actCanvas;

    public GameObject disCanvas;

    public void ButtonClick() {
        //Debug.Log("0");

        actCanvas.SetActive(true);
        disCanvas.SetActive(false);
        
        //Debug.Log("1");
    }
	
}
