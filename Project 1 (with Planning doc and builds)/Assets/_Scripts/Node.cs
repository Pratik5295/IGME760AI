using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node  {


    public Vector3 PositionInWorld;
    public bool IsWalkable;
    public int gridX;
    public int gridY;
    public Node parent;

    public int gCost;
    public int hCost;

    public int walkstatus;
  
    

    public Node(bool status, Vector3 pos, int _gridX, int _gridY)
    {
        IsWalkable = status;
        PositionInWorld = pos;
        gridX = _gridX;
        gridY = _gridY;

       
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
