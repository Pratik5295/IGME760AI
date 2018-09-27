using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node  {


    public Vector3 PositionInWorld;
    public bool IsWalkable;
    

    public Node(bool status, Vector3 pos)
    {
        IsWalkable = status;
        PositionInWorld = pos;
    }
}
