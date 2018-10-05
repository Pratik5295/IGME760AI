using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

    public GameObject playerIns;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            other.transform.position = new Vector3(100f, -22f, 56f);
        }
    }
}
