using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour {

    public GameObject playerPrefab;

    private GameObject playerIns;

    public void PlayerReborn() {
        playerIns = Instantiate(playerPrefab, new Vector3(20.0f, 10.0f, 30.0f), Quaternion.identity) as GameObject;
    }
}
