using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    /// <summary>
    /// The length of how many vertices in the X direction.
    /// </summary>
    [field: SerializeField, Header("Grid Layout"), Range(1, 200)] public int GridXLength
    { get; private set; } = 5;

    /// <summary>
    /// The length of how many vertices in the Z direction.
    /// </summary>
    [field: SerializeField, Range(1, 200)] public int GridZLength
    { get; private set; } = 3;

    /// <summary>
    /// GridSpacing determines the distance between the vertices in world space.
    /// </summary>
    [field: SerializeField, Range(0.1f, 20)] public float GridSpacing
    { get; private set; } = 1.1f;

    /// <summary>
    /// PerlinScale changes the detail or resolution of the perlin noise being used in application to the height of the terrain.
    /// </summary>
    [field: SerializeField, Range(0.1f, 20)] public float PerlinScale
    { get; private set; } = 1.0f;

    /// <summary>
    /// Real height range pre-smoothing.
    /// </summary>
    [field: SerializeField, Range(0.0f, 100.0f)] public float GridYHeightRange
    { get; private set; } = 1.0f;

    /// <summary>
    /// Multiplier tied to the height range.
    /// </summary>
    [field: SerializeField, Range(0.0f, 1.0f)] public float GridYHeightMultiplier
    { get; private set; } = 1.0f;

    [field: SerializeField] public Color TerrainColour
    { get; private set; } = Color.green;

    [field: SerializeField] public bool EnableSmoothing
    { get; private set; } = true;

    public Vector3[,] GridVertices
    { get; private set; }

    [field: SerializeField] public Material TextureApplied
    { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        GenerateTerrain();
        StartCoroutine(UpdateMeshOnInputChange());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateVertices()
    {
        GridVertices = new Vector3[GridXLength + 1, GridZLength + 1];

        for (int xCount = 0; xCount < GridVertices.GetLength(0); xCount++)
        {
            for (int zCount = 0; zCount < GridVertices.GetLength(1); zCount++)
            {
                float xCoord = xCount * GridSpacing;
                float zCoord = zCount * GridSpacing;

                float xPerlinCoord = (float)xCoord / GridVertices.GetLength(0);
                float zPerlinCoord = (float)zCoord / GridVertices.GetLength(1);

                float yCoord = Mathf.PerlinNoise(xPerlinCoord * PerlinScale, zPerlinCoord * PerlinScale) * GridYHeightRange;

                // Create depth in the terrain heights alongside heights.
                yCoord = (yCoord - 0.5f) * 2.0f;

                GridVertices[xCount, zCount] = new Vector3(xCoord, yCoord * GridYHeightMultiplier, zCoord);
            }
        }
    }

    public void AverageVertexHeights()
    {
        // To be filled in with the averaged vertex heights for assigning to GridVertices vector3.Y.
        float[,] vertexAverageHeights = new float[GridVertices.GetLength(0), GridVertices.GetLength(1)];

        // Represent the indexes of all neighbouring adjacent vertices.
        int[,] directionalIndexes = new int[,]
        {
            { 0, 0 },
            { 0, 1 },
            { 1, 1 },
            { 1, 0 },
            { 1, -1 },
            { 0, -1 },
            { -1, -1 },
            { -1, 0 },
            { -1, 1 }
        };

        // Floats are added up each vertices by the current Y value, one pre-smoothing and one post-smoothing.
        // In the end, the average of each height is produced by dividing the total of all height positions by the number of vertices.
        float originalAverageHeightForEveryVertices = 0;
        float newSmoothedAverageHeightForEveryVertices = 0;

        // Loop over all the gridVertices to get the vertex values.
        for (int xCount = 0; xCount < GridVertices.GetLength(0); xCount++)
        {
            for (int zCount = 0; zCount < GridVertices.GetLength(1); zCount++)
            {
                // List to contain all the valid vertices that are to be averaged for current vertices.
                List<float> validVerticesHeightsToAverage = new List<float>();

                // Add up the pre-Averaged heights for each vertices before it is averaged over all vertices later.
                float preAveragedHeight = GridVertices[xCount, zCount].y;
                originalAverageHeightForEveryVertices += preAveragedHeight;

                // Loop over all the directional indexes to grab the height values from, including self.
                for (int i = 0; i < directionalIndexes.GetLength(0); i++)
                {
                    int[] currentIndexToCheck = { xCount + directionalIndexes[i, 0], zCount + directionalIndexes[i, 1] };

                    // Check if the index being checked from is inside the bounds of the grid of vertices, and skip if it isn't.
                    if (currentIndexToCheck[0] >= 0 && currentIndexToCheck[0] < GridVertices.GetLength(0) && currentIndexToCheck[1] >= 0 && currentIndexToCheck[1] < GridVertices.GetLength(1))
                    {
                        validVerticesHeightsToAverage.Add(GridVertices[currentIndexToCheck[0], currentIndexToCheck[1]].y);
                    }
                }

                float postAveragedHeight = validVerticesHeightsToAverage.Average();
                vertexAverageHeights[xCount, zCount] = postAveragedHeight;
                // Add up the post-Averaged heights for each vertices before it is averaged over all vertices later.
                newSmoothedAverageHeightForEveryVertices += postAveragedHeight;
            }
        }

        // Here the overall average heights are calculated after all the vertices heights have been added up.
        originalAverageHeightForEveryVertices /= GridVertices.GetLength(0) * GridVertices.GetLength(1);
        newSmoothedAverageHeightForEveryVertices /= GridVertices.GetLength(0) * GridVertices.GetLength(1);

        float heightOffset = originalAverageHeightForEveryVertices - newSmoothedAverageHeightForEveryVertices;

        // Apply the smoothed vertex heights to the GridVertices 2D array, overwriting the non-smoothed values.
        for (int i = 0; i < GridVertices.GetLength(0); i++)
        {
            for (int j = 0; j < GridVertices.GetLength(1); j++)
            {
                GridVertices[i,j].y = vertexAverageHeights[i, j] + heightOffset;
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

    /// <summary>
    /// Returns a single-dimensional array for containing the vertices positions of the terrain mesh. Due to order of generating the 2D vertices, the array contains one x position and all related y positions, followed by the next x position, followed by all related next x, y position, etc.
    /// </summary>
    /// <param name="twoDimensionalVectors"></param>
    /// <returns></returns>
    public Vector3[] TwoDimensionalVectorsToOne(Vector3[,] twoDimensionalVectors)
    {
        Vector3[] newSingleArray = new Vector3[twoDimensionalVectors.GetLength(0) * twoDimensionalVectors.GetLength(1)];

        int counter = 0;
        foreach (Vector3 vertices in twoDimensionalVectors)
        {
            newSingleArray[counter] = vertices;
            counter++;
        }
        return newSingleArray;
    }

    /// <summary>
    /// Return an array of integers for drawing triangles for the mesh. The integers are the index positions of the vertices of the mesh when in a single array.
    /// </summary>
    /// <param name="verticesArray"></param>
    /// <returns></returns>
    public int[] ReturnTriangles(Vector3[] verticesArray)
    {
        List<int> trianglesVerticesIndexes = new List<int>();

        // 
        for (int i = 0; i < verticesArray.Length - GridVertices.GetLength(1); i++)
        {
            // If the current index is an index in the last row, don't generate triangles as there are no vertices further in the array to connect to for creating a triangle.
            if ((i + 1) % GridVertices.GetLength(1) == 0)
            {
                continue;
            }

            // The index positions are chosen based on how the array was generated - going up in Z axis only needs to add one to index, while going across in X axis needs to go up by length of width (Z axis).
            int bottomLeftIndex = i;
            int topLeftIndex = i + 1;
            int topRightIndex = i + GridVertices.GetLength(1) + 1;
            int bottomRightIndex = i + GridVertices.GetLength(1);

            // First triangle
            trianglesVerticesIndexes.Add(bottomLeftIndex);
            trianglesVerticesIndexes.Add(topLeftIndex);
            trianglesVerticesIndexes.Add(bottomRightIndex);

            // Second triangle
            trianglesVerticesIndexes.Add(topLeftIndex);
            trianglesVerticesIndexes.Add(topRightIndex);
            trianglesVerticesIndexes.Add(bottomRightIndex);

        }

        return trianglesVerticesIndexes.ToArray();
    }

    /// <summary>
    /// Generates normals for each vertices of a triangle within the mesh. Returns a respective array of normalised Vector3's for each vertices of the triangles.
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="triangles"></param>
    /// <returns></returns>
    public Vector3[] GenerateNormals(Vector3[] vertices, int[] trianglesVerticesIndexes)
    {
        // Create an array to hold the normals for each vertex
        Vector3[] normalsValuesForVertices = new Vector3[vertices.Length];

        // Calculate normals for each triangle, looping in increments of 3 for each grouping of triangle vertices. 
        for (int i = 0; i < trianglesVerticesIndexes.Length; i += 3)
        {
            // Grab the indexes of the triangles vertices.
            int triangleIndex0 = trianglesVerticesIndexes[i];
            int triangleIndex1 = trianglesVerticesIndexes[i + 1];
            int triangleIndex2 = trianglesVerticesIndexes[i + 2];

            // Use these indexes to grab the actual vertices for a triangle.
            Vector3 vertex0 = vertices[triangleIndex0];
            Vector3 vertex1 = vertices[triangleIndex1];
            Vector3 vertex2 = vertices[triangleIndex2];

            // Calculate the normal using the cross product - a third axis that is perpendicular to two of the lengths of the triangle.
            Vector3 lengthOne = vertex1 - vertex0;
            Vector3 lengthTwo = vertex2 - vertex0;
            Vector3 normal = Vector3.Cross(lengthOne, lengthTwo).normalized;

            // Add the normal to each vertex's normal.
            // The normals values are added with past / future normal values calulated by adjorning triangles. This helps produces a smoothed out edge for the lighting,
            // as each vertices takes into account the cross products of all edges it provides to triangles, before being normalised.
            normalsValuesForVertices[triangleIndex0] += normal;
            normalsValuesForVertices[triangleIndex1] += normal;
            normalsValuesForVertices[triangleIndex2] += normal;
        }

        // Normalize the normals to ensure they have a length of 1
        for (int i = 0; i < normalsValuesForVertices.Length; i++)
        {
            normalsValuesForVertices[i].Normalize();
        }

        return normalsValuesForVertices;
    }

    /// <summary>
    /// The UV value is a normalised value that states where along the mesh, at a particular vertices point, should the texture project too for a certain indices.
    /// The use of normalised values ensures that the textures is project in a 1:1 manner across the mesh surface
    /// </summary>
    /// <returns></returns>
    Vector2[] GenerateUVs()
    {
        Vector2[] uvPoints = new Vector2[GridVertices.GetLength(0) * GridVertices.GetLength(1)];

        int counter = 0;
        for (int x = 0; x < GridVertices.GetLength(0); x++)
        {
            for (int z = 0; z < GridVertices.GetLength(1); z++)
            {

                uvPoints[counter] = new Vector2((float)x / (GridVertices.GetLength(0) - 1), (float)z / (GridVertices.GetLength(1) - 1));
                counter++;
            }
        }

        return uvPoints;
    }

    /// <summary>
    /// For any value changed that impacts the mesh, remove the existing mesh and generate a new mesh.
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateMeshOnInputChange()
    {
        float timeTillNextCheck = 0.01f;

        int Old_GridXLength;
        int Old_GridZLength;
        float Old_GridSpacing;
        float Old_GridYHeightRange;
        Color Old_TerrainColour;
        float Old_GridYHeightMultiplier;
        bool Old_EnableSmoothing;
        float Old_PerlinScale;

        while (true)
        {
            Old_GridXLength = GridXLength;
            Old_GridZLength = GridZLength;
            Old_GridSpacing = GridSpacing;
            Old_GridYHeightRange = GridYHeightRange;
            Old_TerrainColour = TerrainColour;
            Old_GridYHeightMultiplier = GridYHeightMultiplier;
            Old_EnableSmoothing = EnableSmoothing;
            Old_PerlinScale = PerlinScale;

            yield return new WaitForSeconds(timeTillNextCheck);

            bool areValuesSame = Old_GridXLength == GridXLength
                                    && Old_GridZLength == GridZLength
                                    && Old_GridSpacing == GridSpacing
                                    && Old_GridYHeightRange == GridYHeightRange
                                    && Old_TerrainColour == TerrainColour
                                    && Old_GridYHeightMultiplier == GridYHeightMultiplier
                                    && Old_EnableSmoothing == EnableSmoothing
                                    && Old_PerlinScale == PerlinScale;

            if (areValuesSame)
            {
                continue;
            }

            GenerateTerrain();
        }
    }

    void GenerateTerrain()
    {
        GenerateVertices();

        if (EnableSmoothing)
        {
            AverageVertexHeights();
        }

        // GenerateGameObjectsAtVertices();

        if (GameObject.Find("meshHolder") != null)
        {
            Destroy(GameObject.Find("meshHolder"));
        }

        // Generate a new gameObject with mesh components.
        GameObject meshHolder = new GameObject("meshHolder");
        MeshRenderer renderer = meshHolder.AddComponent<MeshRenderer>();
        // renderer.material.color = TerrainColour;
        renderer.GetComponent<MeshRenderer>().material = TextureApplied;
        MeshFilter filter = meshHolder.AddComponent<MeshFilter>();

        // Mesh to be added to the meshHolder mesh filter.
        Mesh terrainMesh = new Mesh();

        // Add required information to terrainMesh for mesh generation.
        Vector3[] allVertices = TwoDimensionalVectorsToOne(GridVertices);
        terrainMesh.vertices = allVertices;

        // Grab vertices and make triangles.
        terrainMesh.triangles = ReturnTriangles(allVertices);

        // Grab normals for mesh - to help with how light interacts with the mesh.
        terrainMesh.normals = GenerateNormals(allVertices, terrainMesh.triangles);

        Vector2[] allUV = GenerateUVs();

        terrainMesh.uv = allUV;

        filter.mesh = terrainMesh;

        meshHolder.transform.position = Vector3.zero;
    }
}
