using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluGrid : MonoBehaviour {

    // Use this for initialization
    public Node current_node;
    public GameObject gridObject;

    public GameObject greenTile;
    public Vector2[] quaduv = new Vector2[4];

    public Vector3[] quadvert = new Vector3[4];





    void Start () {

        current_node = gridObject.GetComponent<Grid>().NodeFromWorldPoint(this.transform.position);
        Debug.Log(current_node.PositionInWorld);

    }
	
	// Update is called once per frame
	void Awake () {

       
      
	}

    void Update()
    {
        /*quadvert[0] = new Vector3(this.transform.position.x - 5, this.transform.position.y, this.transform.position.z + 2);
        quadvert[1] = new Vector3(this.transform.position.x - 5, this.transform.position.y, this.transform.position.z - 6);
        quadvert[2] = new Vector3(this.transform.position.x + 5, this.transform.position.y, this.transform.position.z + 2);
        quadvert[3] = new Vector3(this.transform.position.x + 5, this.transform.position.y, this.transform.position.z - 6);*/
        CreateGreenTile();
    }

    void DrawInfluMap()
    {
        Mesh[] meshShape = new Mesh[3];
        GameObject influobj = new GameObject("InfluenceUnit");
        MeshRenderer mr = influobj.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        influobj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.3f, this.transform.position.z);
        Mesh influmesh = new Mesh();

         influobj.AddComponent<MeshFilter>().mesh = meshShape[3];

        //quadvert[0] = current_node.PositionInWorld + new Vector3(3, 0, 0);
        // quadvert[1] = current_node.PositionInWorld + new Vector3(3, 0, 3);
        // quadvert[2] = current_node.PositionInWorld + new Vector3(-3, 0, 0);
        // quadvert[3] = current_node.PositionInWorld + new Vector3(-3, 0, -3);



        influmesh.vertices = quadvert;

        quaduv[0] = new Vector2(0, 0);
        quaduv[1] = new Vector2(0, 1);
        quaduv[2] = new Vector2(1, 0);
        quaduv[3] = new Vector2(1, 1);

        influmesh.uv = quaduv;

        influmesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        //mf.mesh = influmesh;

        influmesh.RecalculateBounds();
        influmesh.RecalculateNormals();

    }

    void CreateGreenTile()
    {
        //draw green tiles around the cube
        //draw below the cube
        Instantiate(greenTile, new Vector3(current_node.PositionInWorld.x, current_node.PositionInWorld.y + 0.4f, current_node.PositionInWorld.z), greenTile.transform.rotation);
    }
}
