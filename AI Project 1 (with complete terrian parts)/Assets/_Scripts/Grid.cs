using System.Collections;
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
    private int GridSizeX = 75;     //width of the grid
    private int GridSizeY = 75;     //height of the grid


    public int numberofCellsX;
    private int numberofCellsY;

    private LayerMask obstacles = 512;  //integer value for layer 9 //// 1<<9 would also work

    public Vector3 startOfGrid;
    private Vector3 gridpoint;

  
    //to check raycasts hit some collider
    private bool HitsAir;

    

	void Start () {

        
        
        numberofCellsX =GridSizeX/ CellSize;
        numberofCellsY = GridSizeY / CellSize;







    }

    private void Update()
    {
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
                bool IsWalkable = !(Physics.CheckSphere(gridpoint, CellSize, obstacles));


                //raycasting code
                Ray checkray = new Ray(gridpoint + transform.up * 200, (-transform.up));
                RaycastHit rayhit;
                if(Physics.Raycast(checkray,out rayhit, 250))
                {
                    if(rayhit.collider.gameObject.tag == "Obstacle")
                    {
                        IsWalkable = false;
                    }
                  

                    if(rayhit.collider.gameObject.tag == "Ground")
                    {
                        IsWalkable = true;
                    }

                    gridpoint.y = rayhit.point.y;
                }
                else
                {
                    HitsAir = true;
                }



                graph[x, y] = new Node(IsWalkable, gridpoint);




            }
        }
    }

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
