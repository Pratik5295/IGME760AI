using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    // Use this for initialization

    private Node[,] graph;
    private float GridPosX;
    private float GridPosY;

    private float CellSize = 1f;
    private float GridSizeX = 30f;
    private float GridSizeY = 30f;

    private float numberofCellsX;
    private float numberofCellsY;



    public Vector3 startOfGrid;


	void Start () {

        startOfGrid = this.transform.position - (transform.right * GridSizeX/2 + transform.forward * GridSizeY/2); // gets the bottom left starting point of the grid

        numberofCellsX = GridSizeX / CellSize;
        numberofCellsY = GridSizeY / CellSize;


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, new Vector3(GridSizeX, 1, GridSizeY));
    }
}
