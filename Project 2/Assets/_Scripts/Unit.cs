using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public Transform target;
    private Vector3 pos;
    float speed = 5;
    Vector3[] path;
    int targetIndex;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("MouseDown");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "GameController")
                {
                    print("12312");
                }
                else if (hit.collider.gameObject.tag == "Terrain" || hit.collider.gameObject.tag == "Slope")
                {
                    pos = hit.point;
                    RequestPathManager.RequestPath(this.transform.position, pos, OnPathFound);
                }
            }
        }    
    }
    private void Start()
    {
        Debug.Log("Unit has started");     
        RequestPathManager.RequestPath(this.transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWays = path[0];
        targetIndex = 0;
        while(true)
        {
            if(transform.position == currentWays)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWays = path[targetIndex];

            }

            transform.position = Vector3.MoveTowards(transform.position, currentWays, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
