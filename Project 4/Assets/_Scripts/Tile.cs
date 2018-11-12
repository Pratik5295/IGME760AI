using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public GameObject playerObject;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        Instantiate(playerObject,this.gameObject.transform);
        Node node = Grid.Instance.NodeFromWorldPoint(this.transform.position);
        List<Node> neighbors = Grid.Instance.GetNeighbors(node);
        foreach (Node n in neighbors)
        {
            n.value = 3;
        }
    }   
}
