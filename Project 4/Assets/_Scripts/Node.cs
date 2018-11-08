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

    public int value;

    public int walkstatus;
  
    

    public Node(bool status, Vector3 pos, int _gridX, int _gridY, int _value)
    {
        IsWalkable = status;
        PositionInWorld = pos;
        gridX = _gridX;
        gridY = _gridY;

        value = _value;


       
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
