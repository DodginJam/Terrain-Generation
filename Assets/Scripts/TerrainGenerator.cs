using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [field: SerializeField, Header("Grid Layout")] public int GridXLength
    { get; private set; } = 5;

    [field: SerializeField] public int GridZLength
    { get; private set; } = 3;

    [field: SerializeField] public float GridSpacing
    { get; private set; } = 1.1f;

    [field: SerializeField, Range(0.0f, 3.0f)] public float GridYHeightRange
    { get; private set; } = 0.0f;

    public Vector3[,] GridVertices
    { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        GenerateVertices();
        GenerateGameObjectsAtVertices();

        // Generate a new mesh based on the vertices generated previously.
        Mesh terrainMesh = new Mesh();

        Vector3[] allVertices = TwoDimensionalVectorsToOne(GridVertices);

        terrainMesh.vertices = allVertices;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateVertices()
    {
        GridVertices = new Vector3[GridXLength, GridZLength];

        for (int xCount = 0; xCount < GridXLength; xCount++)
        {
            for (int zCount = 0; zCount < GridZLength; zCount++)
            {
                GridVertices[xCount, zCount] = new Vector3(xCount * GridSpacing, Random.Range(0, GridYHeightRange), zCount * GridSpacing);
            }
        }
    }

    public void GenerateGameObjectsAtVertices()
    {
        // Reset the cube container if one already exists.
        GameObject cubeContainer;

        if (GameObject.Find("CubeContainer") != null)
        {
            Destroy(GameObject.Find("CubeContainer"));
        }

        cubeContainer = new GameObject("CubeContainer");
        cubeContainer.transform.parent = transform;

        // Generate cubes objects at position of vertices.
        for (int xCount = 0; xCount < GridVertices.GetLength(0); xCount++)
        {
            for (int zCount = 0; zCount < GridVertices.GetLength(1); zCount++)
            {
                GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                newCube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                newCube.transform.position = GridVertices[xCount, zCount];
                newCube.transform.SetParent(cubeContainer.transform, true);
            }
        }
    }

    public Vector3[] TwoDimensionalVectorsToOne(Vector3[,] twoDimensionalVectors)
    {
        Vector3[] newSingleArray = new Vector3[twoDimensionalVectors.GetLength(0) * twoDimensionalVectors.GetLength(1)];

        int counter = 0;
        foreach (Vector3 vertices in GridVertices)
        {
            newSingleArray[counter] = vertices;
            Debug.Log(newSingleArray[counter]);
            counter++;
        }
        return newSingleArray;
    }
}
