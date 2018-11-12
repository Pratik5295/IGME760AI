using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateUnit : MonoBehaviour
{
    public GameObject pointPrefab;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GameObject point = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.point);
                point = GameObject.Instantiate(pointPrefab, hit.point, transform.rotation) as GameObject;

                //Node[,] graph1 = Grid.Instance.graph;
                //Node node = Grid.Instance.NodeFromWorldPoint(point.transform.position);
                //List<Node> neighbors = Grid.Instance.GetNeighbors(node);
                //Node[,] graph2 = Grid.Instance.graph;
                //if (graph1 == graph2)
                //{
                //    Debug.Log("equal");
                //}
            }

           


        }





    }
}





