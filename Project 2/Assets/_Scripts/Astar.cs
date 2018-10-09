using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Astar : MonoBehaviour {

    RequestPathManager requestManager;

    public Transform seeker, target;
    Grid grid;

    void Awake()
    {
        //Debug.Log("Astar has awaken");
        requestManager = GetComponent<RequestPathManager>();
        grid = GetComponent<Grid>();
    }

    //private void LateUpdate()
    //{
    //    FindPath(seeker.position, target.position);
    //}

    public void BeginFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }
    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] ways = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode.IsWalkable && targetNode.IsWalkable)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closeSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closeSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbor in grid.GetNeighbors(currentNode))
                {
                    if (!neighbor.IsWalkable || closeSet.Contains(neighbor))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                    if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }
            yield return null;
            
            if (pathSuccess)
            {
                ways = RetracePath(startNode, targetNode);
            }
            requestManager.FinishedProcessingPath(ways, pathSuccess);
        }


    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;    
        }
        Vector3[] ways = SimplyPath(path);
        Array.Reverse(ways);
        if(path == null)
            Debug.Log("PATH IS NULL!");

        return ways;

        //seeker = GetComponent<>();
    }

    Vector3[] SimplyPath(List<Node> path)
    {
        List<Vector3> ways = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                ways.Add(path[i].PositionInWorld);
            }
            directionOld = directionNew;
        }
        return ways.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return (14 * dstY + 10 * (dstX - dstY));

        return (14 * dstX + 10 * (dstY - dstX));
    }
}
