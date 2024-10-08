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

        // Generate a new gameObject with mesh components.
        GameObject meshHolder = new GameObject("meshHolder");
        MeshRenderer renderer = meshHolder.AddComponent<MeshRenderer>();
        MeshFilter filter = meshHolder.AddComponent<MeshFilter>();

        // Mesh to be added to the meshHolder mesh filter.
        Mesh terrainMesh = new Mesh();

        // Add required information to terrainMesh for mesh generation.
        Vector3[] allVertices = TwoDimensionalVectorsToOne(GridVertices);
        terrainMesh.vertices = allVertices;

        // Grab vertices and make triangles.
        terrainMesh.triangles = ReturnTriangles(allVertices);

        // Grab normals for mesh.
        terrainMesh.normals = GenerateNormals(allVertices);


        filter.mesh = terrainMesh;
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
            counter++;
        }
        return newSingleArray;
    }

    /// <summary>
    /// Return an array of integers for drawing triangles for the mesh. Based on an index 
    /// </summary>
    /// <param name="verticesArray"></param>
    /// <returns></returns>
    public int[] ReturnTriangles(Vector3[] verticesArray)
    {
        List<int> trianglesVertices = new List<int>();

        for (int i = 0; i < verticesArray.Length - GridVertices.GetLength(1); i++)
        {
            if ((i + 1) % GridVertices.GetLength(1) == 0)
            {
                continue;
            }

            int bottomLeftIndex = i;
            int topLeftIndex = i + 1;
            int topRightIndex = i + GridVertices.GetLength(1) + 1;
            int bottomRightIndex = i + GridVertices.GetLength(1);

            // First triangle
            trianglesVertices.Add(bottomLeftIndex);
            trianglesVertices.Add(topLeftIndex);
            trianglesVertices.Add(bottomRightIndex);

            // Second triangle
            trianglesVertices.Add(topLeftIndex);
            trianglesVertices.Add(topRightIndex);
            trianglesVertices.Add(bottomRightIndex);

        }

        return trianglesVertices.ToArray();
    }

    Vector3[] GenerateNormals(Vector3[] vertices)
    {
        Vector3[] normalsArray = new Vector3[vertices.Length];

        for (int i = 0; i < vertices.Length; i ++ )
        {
            normalsArray[i] = -Vector3.up;
        }

        return normalsArray;
    }


    // From chatGPT - need to study and understand.
    public Vector3[] GenerateNormals(Vector3[] vertices, int[] triangles)
    {
        // Create an array to hold the normals for each vertex
        Vector3[] normals = new Vector3[vertices.Length];

        // Calculate normals for each triangle
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int index0 = triangles[i];
            int index1 = triangles[i + 1];
            int index2 = triangles[i + 2];

            // Get the vertices for the triangle
            Vector3 vertex0 = vertices[index0];
            Vector3 vertex1 = vertices[index1];
            Vector3 vertex2 = vertices[index2];

            // Calculate the normal using the cross product
            Vector3 edge1 = vertex1 - vertex0;
            Vector3 edge2 = vertex2 - vertex0;
            Vector3 normal = Vector3.Cross(edge1, edge2).normalized;

            // Add the normal to each vertex's normal
            normals[index0] += normal;
            normals[index1] += normal;
            normals[index2] += normal;
        }

        // Normalize the normals to ensure they have a length of 1
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i].Normalize();
        }

        return normals;
    }
}
