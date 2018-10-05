using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour {

    public GameObject playerPrefab;

    private GameObject playerIns;

    public void PlayerReborn() {
        playerIns = Instantiate(playerPrefab, new Vector3(100f, 0f, 56f), Quaternion.identity) as GameObject;
    }
}
