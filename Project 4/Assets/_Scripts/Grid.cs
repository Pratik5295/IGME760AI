﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    // Use this for initialization

    private Node[,] graph;
    private float GridPosX;
    private float GridPosY;

    public Terrain terrain;

    private float terrainHeight;
    private float terrainWidth;

   

    private int CellSize = 1;
    public int GridSizeX = 75;     //width of the grid
    public int GridSizeY = 75;     //height of the grid


    private int numberofCellsX;
    private int numberofCellsY;

    private int walkingstatus;

    private LayerMask obstacles = 512;  //integer value for layer 9 //// 1<<9 would also work

    public Vector3 startOfGrid;
    private Vector3 gridpoint;

    private bool IsWalkable;

    //to check raycasts hit some collider
    private bool HitsAir;

    

	void Awake () {
        Debug.Log("Grid is awaken");
        numberofCellsX = GridSizeX / CellSize;
        numberofCellsY = GridSizeY / CellSize;
        graph = new Node[GridSizeX, GridSizeY];
        GridDrawing();
    }

  

    void GridDrawing()
    {
        graph = new Node[GridSizeX ,GridSizeY];

        startOfGrid = this.transform.position - transform.right *GridSizeX / 2 - transform.forward * GridSizeY / 2; // gets the bottom left starting point of the grid

        for (int x = 0; x < numberofCellsX; x++)
        {
            for(int y = 0; y< numberofCellsY; y++)
            {
                gridpoint = startOfGrid + Vector3.right * (x *CellSize + 0.5f) + Vector3.forward * (y*CellSize + 0.5f);

                IsWalkable = !(Physics.CheckSphere(gridpoint, CellSize, obstacles));


                //raycasting code
                Ray checkray = new Ray(gridpoint + transform.up * 200, (-transform.up));
                RaycastHit rayhit;
                if (Physics.Raycast(checkray, out rayhit, 250))
                {
                    if (rayhit.collider.gameObject.tag == "Obstacle")
                    {
                        IsWalkable = false;
                        //Debug.Log("Hits obstacle " + rayhit.collider.gameObject.name + gridpoint);
                        walkingstatus = 1;

                    }


                    if (rayhit.collider.gameObject.tag == "Ground")
                    {
                        IsWalkable = true;
                        walkingstatus = 0;
                    }

                    gridpoint.y = rayhit.point.y;
                }
                else
                {
                    HitsAir = true;

                    IsWalkable = false;

                }

                graph[x, y] = new Node(IsWalkable, gridpoint, x, y);

            }
        }
    }
    
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <=1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < numberofCellsX && checkY >= 0 && checkY < numberofCellsY)
                {
                    neighbors.Add(graph[checkX, checkY]);          
                }
            }
        }
        return neighbors;
    }
    public Node NodeFromWorldPoint(Vector3 PositionInWorld)
    {
        float percentX = (PositionInWorld.x + GridSizeX / 2) / GridSizeX;
        float percentY = (PositionInWorld.z + GridSizeY / 2) / GridSizeY;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((numberofCellsX-1) * percentX);
        int y = Mathf.RoundToInt((numberofCellsY-1) * percentY);
        //Debug.Log("X:" + x);
        //Debug.Log("Y:" + y);
        return graph[x, y];
    }

    public List<Node> path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, new Vector3(GridSizeX, 1, GridSizeY));

        if(graph != null)
        {
            foreach(Node n in graph)
            {
                if (n.IsWalkable)
                {
                    Gizmos.color = Color.blue;
                    if (path != null)
                        if (path.Contains(n))
                        {
                            Debug.Log("Path exists");
                            Gizmos.color = Color.red;
                            Gizmos.DrawCube(n.PositionInWorld, Vector3.one * 0.8f);
                        }
                          
                    Gizmos.DrawCube(n.PositionInWorld, Vector3.one * 0.8f);
                }

                else if(!n.IsWalkable)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.PositionInWorld, Vector3.one * 0.8f);
                }

            }
        }
    }
}
