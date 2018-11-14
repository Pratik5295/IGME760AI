
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grid : MonoBehaviour {


    private static Grid instance;
    public static Grid Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("InfluenceMap");

                if (go)
                    instance = go.GetComponent<Grid>();
            }


            return instance;
        }
    }


    public Node[,] graph;
    private float GridPosX;
    private float GridPosY;

   

    private int CellSize = 1;
    public int GridSizeX = 280;     //width of the grid
    public int GridSizeY = 230;     //height of the grid


    private int numberofCellsX;
    private int numberofCellsY;


    private LayerMask obstacles = 512;  //integer value for layer 9 //// 1<<9 would also work

    public Vector3 startOfGrid;
    private Vector3 gridpoint;

    private bool IsWalkable;

    private int valueOfTile;

    //********Tile Prefabs******
    public GameObject tileObjects;
    public GameObject TileMinus4;
    public GameObject TileMinus3;
    public GameObject TileMinus2;
    public GameObject TileMinus1;
    public GameObject Tile0;
    public GameObject Tile1;
    public GameObject Tile2;
    public GameObject Tile3;
    public GameObject Tile4;
    public GameObject TileBlack;

    static GameObject tile;
    bool toggle = false;

    void Awake () {
        tileObjects.transform.parent = GameObject.Find("Environment").transform;
        Debug.Log("Grid is awaken");
        numberofCellsX = GridSizeX / CellSize;
        numberofCellsY = GridSizeY / CellSize;
        graph = new Node[GridSizeX, GridSizeY];
        GridDrawing();
    }

    private void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            if (toggle == false)
            {
                DrawGridOnScreen();
                toggle = true;
            }
        }
        if (Input.GetKeyDown("2"))
        { 
            DeletingTheMap();
            toggle = false;
        }
    }

    void DeletingTheMap()
    {

        foreach(Transform child in tileObjects.transform)
        {
            if(child != null)
            Destroy(child.gameObject);
        }
    }

    void GridDrawing()
    {
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
                        valueOfTile = 100;

                    }


                    else if (rayhit.collider.gameObject.tag == "Ground")
                    {
                        IsWalkable = true;
                        valueOfTile = 0;
                    }

                   else if(rayhit.collider.gameObject.tag == "Team1")
                    {
                        Debug.Log("Collided with a unit");
                        valueOfTile = 3;
                    }

                    else if(rayhit.collider.gameObject.tag == "Team2")
                    {
                        Debug.Log("team 2 unit detected");
                        valueOfTile = 2;
                    }

                    gridpoint.y = rayhit.point.y;
                }
                else
                {

                    IsWalkable = false;

                }

                graph[x, y] = new Node(IsWalkable, gridpoint, x, y,valueOfTile);

            }
        }
    }

    public void SetValueAroundNode(Node node, int range, int value)
    {
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < numberofCellsX && checkY >= 0 && checkY < numberofCellsY)
                {
                    int distance = ChebyshevDistance(node.gridX, node.gridY, checkX, checkY);

                    switch (distance)
                    {
                        case 0:
                            graph[checkX, checkY].value += 4*value;
                            break;
                        case 1:
                            graph[checkX, checkY].value += 3 * value;
                            break;
                        case 2:
                            graph[checkX, checkY].value += 2 * value;
                            break;
                        case 3:
                            graph[checkX, checkY].value += 1 * value;
                            break;

                    }
                }         
            }
        }
    }
    public static int ChebyshevDistance(int xA, int yA, int xB, int yB)
    {
        return Math.Max(Math.Abs(xA - xB), Math.Abs(yA - yB));
    }

    public Node NodeFromWorldPoint(Vector3 PositionInWorld)
    {
        float percentX = (PositionInWorld.x + GridSizeX / 2) / GridSizeX;
        float percentY = (PositionInWorld.z + GridSizeY / 2) / GridSizeY;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((numberofCellsX) * percentX);
        int y = Mathf.RoundToInt((numberofCellsY+1) * percentY);
        return graph[x, y];
    }

    public void DrawGridOnScreen()
    {  
        if (graph != null)
        {
            foreach (Node n in graph)
            {
                if (n.IsWalkable)
                {
                    if(n.value == -4)
                        tile = Instantiate(TileMinus4, new Vector3(n.PositionInWorld.x, n.PositionInWorld.y + 0.3f, n.PositionInWorld.z), TileMinus4.transform.rotation);
                    else if(n.value == -3)
                        tile = Instantiate(TileMinus3, new Vector3(n.PositionInWorld.x, n.PositionInWorld.y + 0.3f, n.PositionInWorld.z), TileMinus3.transform.rotation);
                    else if(n.value == -2)
                        tile = Instantiate(TileMinus2, new Vector3(n.PositionInWorld.x, n.PositionInWorld.y + 0.3f, n.PositionInWorld.z), TileMinus2.transform.rotation);
                    else if(n.value == -1)
                        tile = Instantiate(TileMinus1, new Vector3(n.PositionInWorld.x, n.PositionInWorld.y + 0.3f, n.PositionInWorld.z), TileMinus1.transform.rotation);
                    else if(n.value == 0)
                        tile = Instantiate(Tile0, new Vector3(n.PositionInWorld.x, n.PositionInWorld.y + 0.3f, n.PositionInWorld.z), Tile0.transform.rotation);
                    else if(n.value == 1)
                        tile = Instantiate(Tile1, new Vector3(n.PositionInWorld.x, n.PositionInWorld.y + 0.3f, n.PositionInWorld.z), Tile1.transform.rotation);
                    else if(n.value == 2)
                        tile = Instantiate(Tile2, new Vector3(n.PositionInWorld.x, n.PositionInWorld.y + 0.3f, n.PositionInWorld.z), Tile2.transform.rotation);
                    else if(n.value == 3)
                        tile = Instantiate(Tile3, new Vector3(n.PositionInWorld.x, n.PositionInWorld.y + 0.3f, n.PositionInWorld.z), Tile3.transform.rotation);
                    else if(n.value == 4)
                        tile = Instantiate(Tile4, new Vector3(n.PositionInWorld.x, n.PositionInWorld.y + 0.3f, n.PositionInWorld.z), Tile4.transform.rotation);
                    else if (n.value > 4)
                        tile = Instantiate(Tile4, new Vector3(n.PositionInWorld.x, n.PositionInWorld.y + 0.3f, n.PositionInWorld.z), Tile4.transform.rotation);
                    else if(n.value < -4)
                        tile = Instantiate(TileMinus4, new Vector3(n.PositionInWorld.x, n.PositionInWorld.y + 0.3f, n.PositionInWorld.z), TileMinus4.transform.rotation);
                    tile.transform.parent = tileObjects.transform;                    
                }
                else
                {
                    tile = Instantiate(TileBlack, new Vector3(n.PositionInWorld.x, n.PositionInWorld.y + 0.3f, n.PositionInWorld.z), TileBlack.transform.rotation) as GameObject;
                    tile.transform.parent = tileObjects.transform;
                }

            }

        }

    }

}
  
