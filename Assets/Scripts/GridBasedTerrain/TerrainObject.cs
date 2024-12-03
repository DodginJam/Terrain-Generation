using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TerrainObject : MonoBehaviour
{
    [field: SerializeField]
    public TerrainInformation Information
    { get; set; }

    [field: SerializeField]
    public string TerrainName
    { get; set; }

    [field: SerializeField]
    public MeshRenderer TerrainRenderer
    { get; set; }

    [field: SerializeField]
    public MeshFilter TerrainMeshFilter
    { get; set; }

    public Vector3[,] Vertices2DArray
    { get; private set; }

    public float TerrainHeightMin
    { get; private set; } = float.MaxValue;
    public float TerrainHeightMax
    { get; private set; } = float.MinValue;


    private void Awake()
    {
        TerrainRenderer = gameObject.AddComponent<MeshRenderer>();
        TerrainMeshFilter = gameObject.AddComponent<MeshFilter>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateTerrain();
        StartCoroutine(UpdateMeshOnInputChange());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateTerrain()
    {
        // Apply the terrainMesh mesh to the filter.
        TerrainMeshFilter.mesh = GenerateMesh(Information.GridXLength, Information.GridZLength, Information.GridSpacing, Information.GridYHeightRange, Information.GridYHeightMultiplier, Information.OffsetX, Information.OffsetZ, Information.PerlinScale, Information.TerrainGradient, Information.HeightColorChange, Information.Seed, Information.Octaves, Information.Persistance, Information.Lacunarity, Information.OctaveOffset);
        // TerrainRenderer.material.mainTexture = GenerateTexture(Vertices2DArray, Information.TerrainGradient, Information.HeightColorChange);
        TerrainRenderer.material = Information.TerrainMaterial;
        transform.position = Information.Position;

        TerrainMeshFilter.mesh.RecalculateBounds();
        TerrainMeshFilter.mesh.RecalculateNormals();
    }

    public Vector3[,] GenerateVertices(int xLength, int zLength, float gridSpacing, float gridYHeightRange, float gridYHeightMultiplier, float offsetX, float offsetZ, float scale, int seed, int octaves, float persistance, float lacunarity, Vector2 octaveOffset)
    {
        int xVerticeCount = xLength + 1;
        int zVerticeCount = zLength + 1;

        // These values are stored for scaling perlin from centre point of meshes - they are minused from the X and Z count during normalisation for perlin co-ords calculation.
        float halfLength = xVerticeCount / 2f;
        float halfWidth = zVerticeCount / 2f;

        TerrainHeightMax = float.MinValue;
        TerrainHeightMin = float.MaxValue;

        // Sampled from Sebastian Lague Procedural Landmass Generation (E03: Octaves). Link: https://youtu.be/MRNFcywkUSA?si=-IN_Y8heM2EOnWIs
        // Provides offsets per octave layer to be randomised.
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float octaveOffsetX = prng.Next(-1000000, 1000000) + octaveOffset.x;
            float octaveOffsetZ = prng.Next(-1000000, 1000000) + octaveOffset.y;

            octaveOffsets[i] = new Vector2(octaveOffsetX, octaveOffsetZ);
        }

        Vector3[,] newVertices = new Vector3[xVerticeCount, zVerticeCount];

        for (int xCount = 0; xCount < xVerticeCount; xCount++)
        {
            for (int zCount = 0; zCount < zVerticeCount; zCount++)
            {
                // Amplitude is how much off an effect the lesser octaves have an impact in overall height values - this is affected by the persistance value.
                float amplitude = 1;
                // Frequency is the level of detail each ocatave passes through to the overall noise height values - this is affected by the lacunarity value.
                float frequency = 1;
                // The noise height is added up over multiple layers - octaves - and the later normalised to allow it to be projected as a value for perlin noise.
                float noiseHeight = 0;

                float xCoord = 0;
                float zCoord = 0;

                // Octaves are the layers of Perlin Noise we generate.
                for (int i = 0; i < octaves; i++)
                {
                    // The coord positions are the local space location of the vertices, being the vertices count multiplyed by the spacing.
                    xCoord = (float)xCount * gridSpacing;
                    zCoord = (float)zCount * gridSpacing;

                    // Getting the Perlin Coord for X and Z by normalising it's value - dividing the current vertices position by the total amounts of vertices in that axis.
                    // The count is offset by half the axis length to let perlin scale from centre of mesh.
                    float xPerlinCoord = ((float)xCount - halfLength) / (scale * frequency) + octaveOffsets[i].x + offsetX;
                    float zPerlinCoord = ((float)zCount - halfWidth) / (scale * frequency) + octaveOffsets[i].y + offsetZ;

                    // The perlinNoise coord are multiplied by scale.
                    // The scale is timesed by the frequency to affect the detail of the respective octave layers, with higher frequency allowing more
                    // finer detail to emerge in the noise.
                                                                                      // Helps to allow negative values of the Perlin Noise.
                    float perlinValue = Mathf.PerlinNoise(xPerlinCoord, zPerlinCoord) * 2 - 1 ;

                    // The perlin value is timesed by amplitude to effect how much the other octaves have impact in the overall 
                    // height - i.e. how persistant they are. Lower octaves should propertioanlly have lesser impact.
                    noiseHeight += perlinValue * amplitude;

                    // Per each loop of the octaves, we times the amplitude and frequency by the respective persistance
                    // and lacunarity values to affect the amplitude and the frequency these have per octave layer on the overall height value generated.
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                
                // Max and Min height global reference
                if (noiseHeight > TerrainHeightMax)
                {
                    TerrainHeightMax = noiseHeight;
                }
                if (noiseHeight < TerrainHeightMin)
                {
                    TerrainHeightMin = noiseHeight;
                }

                // The vertices of the mesh are set to the local space coordinates.
                newVertices[xCount, zCount] = new Vector3(xCoord, noiseHeight, zCoord);
            }
        }

        // Loop through the heightvalues and noramalise Y value due to the previous addition above.
        for (int x = 0; x < newVertices.GetLength(0); x++)
        {
            for (int z = 0; z < newVertices.GetLength(1); z++)
            {
                newVertices[x, z].y = Mathf.InverseLerp(TerrainHeightMax, TerrainHeightMin, newVertices[x, z].y) * gridYHeightRange;
            }
        }


        if (Information.EnableSmoothing)
        {
            newVertices = AverageVertexHeights(newVertices);
        }

        return newVertices;
    }

    /// <summary>
    /// Provides and returns 2D array containing Vector3s that have a Y value modified to reflect the average height for each vertices's based upon itself and it's surrounding neighbors.
    /// </summary>
    /// <param name="verticesArray"></param>
    /// <returns></returns>
    public Vector3[,] AverageVertexHeights(Vector3[,] verticesArray)
    {
        int width = verticesArray.GetLength(0);
        int length = verticesArray.GetLength(1);

        // To be filled in with the averaged vertex heights for assigning to GridVertices vector3.Y.
        float[,] vertexAverageHeights = new float[width, length];

        // Represent the indexes of all neighboring adjacent vertices's.
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

        // Floats are added up each vertices's by the current Y value, one pre-smoothing and one post-smoothing.
        // In the end, the average of each height is produced by dividing the total of all height positions by the number of vertices's.
        float originalAverageHeightForEveryVertices = 0;
        float newSmoothedAverageHeightForEveryVertices = 0;

        // Loop over all the gridVertices to get the vertex values.
        for (int xCount = 0; xCount < width; xCount++)
        {
            for (int zCount = 0; zCount < length; zCount++)
            {
                // List to contain all the valid vertices's that are to be averaged for current vertices's.
                List<float> validVerticesHeightsToAverage = new List<float>();

                // Add up the pre-Averaged heights for each vertices's before it is averaged over all vertices's later.
                float preAveragedHeight = verticesArray[xCount, zCount].y;
                originalAverageHeightForEveryVertices += preAveragedHeight;

                // Loop over all the directional indexes to grab the height values from, including self.
                for (int i = 0; i < directionalIndexes.GetLength(0); i++)
                {
                    int[] currentIndexToCheck = { xCount + directionalIndexes[i, 0], zCount + directionalIndexes[i, 1] };

                    // Check if the index being checked from is inside the bounds of the grid of vertices's, and skip if it isn't.
                    if (currentIndexToCheck[0] >= 0 && currentIndexToCheck[0] < width && currentIndexToCheck[1] >= 0 && currentIndexToCheck[1] < length)
                    {
                        validVerticesHeightsToAverage.Add(verticesArray[currentIndexToCheck[0], currentIndexToCheck[1]].y);
                    }
                }

                float postAveragedHeight = validVerticesHeightsToAverage.Average();
                vertexAverageHeights[xCount, zCount] = postAveragedHeight;
                // Add up the post-Averaged heights for each vertices's before it is averaged over all vertices's later.
                newSmoothedAverageHeightForEveryVertices += postAveragedHeight;
            }
        }

        // Here the overall average heights are calculated after all the vertices's heights have been added up.
        originalAverageHeightForEveryVertices /= width * length;
        newSmoothedAverageHeightForEveryVertices /= width * length;

        float heightOffset = originalAverageHeightForEveryVertices - newSmoothedAverageHeightForEveryVertices;

        // Apply the smoothed vertex heights to the GridVertices 2D array, overwriting the non-smoothed values.
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                verticesArray[i,j].y = vertexAverageHeights[i, j] + heightOffset;
            }
        }

        return verticesArray;
    }

    /// <summary>
    /// Returns a single-dimensional array for containing the vertices's positions of the terrain mesh. Due to order of generating the 2D vertices's, the array contains one x position and all related y positions, followed by the next x position, followed by all related next x, y position, etc.
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
    /// Return an array of integers for drawing triangles for the mesh. The integers are the index positions of the vertices's of the mesh when in a single array.
    /// </summary>
    /// <param name="verticesArray"></param>
    /// <param name="width"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public int[] ReturnTriangles(Vector3[] verticesArray, int width, int length)
    {
        List<int> trianglesVerticesIndexes = new List<int>();

        for (int i = 0; i < verticesArray.Length - length; i++)
        {
            // If the current index is an index in the last row, don't generate triangles as there are no vertices's further in the array to connect to for creating a triangle.
            if ((i + 1) % length == 0)
            {
                continue;
            }

            // The index positions are chosen based on how the array was generated - going up in Z axis only needs to add one to index, while going across in X axis needs to go up by length of width (Z axis).
            int bottomLeftIndex = i;
            int topLeftIndex = i + 1;
            int topRightIndex = i + length + 1;
            int bottomRightIndex = i + length;

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
    /// Generates normals for each vertices's of a triangle within the mesh. Returns a respective array of normalized Vector3's for each vertices's of the triangles.
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="triangles"></param>
    /// <returns></returns>
    public Vector3[] GenerateNormals(Vector3[] vertices, int[] trianglesVerticesIndexes)
    {
        // Create an array to hold the normals for each vertex
        Vector3[] normalsValuesForVertices = new Vector3[vertices.Length];

        // Calculate normals for each triangle, looping in increments of 3 for each grouping of triangle vertices's. 
        for (int i = 0; i < trianglesVerticesIndexes.Length; i += 3)
        {
            // Grab the indexes of the triangles vertices.
            int triangleIndex0 = trianglesVerticesIndexes[i];
            int triangleIndex1 = trianglesVerticesIndexes[i + 1];
            int triangleIndex2 = trianglesVerticesIndexes[i + 2];

            // Use these indexes to grab the actual vertices's for a triangle.
            Vector3 vertex0 = vertices[triangleIndex0];
            Vector3 vertex1 = vertices[triangleIndex1];
            Vector3 vertex2 = vertices[triangleIndex2];

            // Calculate the normal using the cross product - a third axis that is perpendicular to two of the lengths of the triangle.
            Vector3 lengthOne = vertex1 - vertex0;
            Vector3 lengthTwo = vertex2 - vertex0;
            Vector3 normal = Vector3.Cross(lengthOne, lengthTwo).normalized;

            // Add the normal to each vertex's normal.
            // The normals values are added with past / future normal values calculated by adjoining triangles. This helps produces a smoothed out edge for the lighting,
            // as each vertices takes into account the cross products of all edges it provides to triangles, before being normalized.
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
    /// The UV value is a normalized value that states where along the mesh, at a particular vertices's point, should the texture project too for a certain indices.
    /// The use of normalized values ensures that the textures is project in a 1:1 manner across the mesh surface
    /// </summary>
    /// <returns></returns>
    Vector2[] GenerateUVs(Vector3[,] vertices)
    {
        Vector2[] uvPoints = new Vector2[vertices.GetLength(0) * vertices.GetLength(1)];

        int counter = 0;
        for (int x = 0; x < vertices.GetLength(0); x++)
        {
            for (int z = 0; z < vertices.GetLength(1); z++)
            {
                uvPoints[counter] = new Vector2((float)x / (vertices.GetLength(0) - 1), (float)z / (vertices.GetLength(1) - 1));
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
        float Old_HeightColorChange;
        float Old_GridYHeightMultiplier;
        bool Old_EnableSmoothing;
        float Old_PerlinScale;
        float Old_OffsetX;
        float Old_OffsetZ;
        Vector3 Old_Position;
        Material Old_TerrainMaterial;
        int Old_Seed;
        int Old_Octaves;
        float Old_Persistance;
        float Old_Lacunarity;
        Vector2 Old_OctaveOffset;


        while (true)
        {
            Old_GridXLength = Information.GridXLength;
            Old_GridZLength = Information.GridZLength;
            Old_GridSpacing = Information.GridSpacing;
            Old_GridYHeightRange = Information.GridYHeightRange;
            Old_HeightColorChange = Information.HeightColorChange;
            Old_GridYHeightMultiplier = Information.GridYHeightMultiplier;
            Old_EnableSmoothing = Information.EnableSmoothing;
            Old_PerlinScale = Information.PerlinScale;
            Old_OffsetX = Information.OffsetX;
            Old_OffsetZ = Information.OffsetZ;
            Old_Position = Information.Position;
            Old_TerrainMaterial = Information.TerrainMaterial;
            Old_Seed = Information.Seed;
            Old_Octaves = Information.Octaves;
            Old_Persistance = Information.Persistance;
            Old_Lacunarity = Information.Lacunarity;
            Old_OctaveOffset = Information.OctaveOffset;


            yield return new WaitForSeconds(timeTillNextCheck);

            bool areValuesSame = Old_GridXLength == Information.GridXLength
                                    && Old_GridZLength == Information.GridZLength
                                    && Old_GridSpacing == Information.GridSpacing
                                    && Old_GridYHeightRange == Information.GridYHeightRange
                                    && Old_HeightColorChange == Information.HeightColorChange
                                    && Old_GridYHeightMultiplier == Information.GridYHeightMultiplier
                                    && Old_EnableSmoothing == Information.EnableSmoothing
                                    && Old_PerlinScale == Information.PerlinScale
                                    && Old_OffsetX == Information.OffsetX
                                    && Old_OffsetZ == Information.OffsetZ
                                    && Old_Position == Information.Position
                                    && Old_TerrainMaterial.Equals(Information.TerrainMaterial)
                                    && Old_Seed == Information.Seed
                                    && Old_Octaves == Information.Octaves
                                    && Old_Persistance == Information.Persistance
                                    && Old_Lacunarity == Information.Lacunarity
                                    && Old_OctaveOffset == Information.OctaveOffset;

            if (areValuesSame)
            {
                continue;
            }

            UpdateTerrain();
        }
    }

    Mesh GenerateMesh(int xLength, int zLength, float gridSpacing, float gridYHeightRange, float gridYHeightMultiplier, float offsetX, float offsetZ, float perlinScale, Gradient terrainGradient, float heightColorChange, int seed, int octaves, float persistance, float lacunarity, Vector2 octaveOffset)
    {
        Vector3[,] newVertices = GenerateVertices(xLength, zLength, gridSpacing, gridYHeightRange, gridYHeightMultiplier, offsetX, offsetZ, perlinScale, seed, octaves, persistance, lacunarity, octaveOffset);

        Vertices2DArray = newVertices;

        int width = newVertices.GetLength(0);
        int length = newVertices.GetLength(1);

        // Mesh to be added to the meshHolder mesh filter.
        Mesh terrainMesh = new Mesh();

        // Set index format to 32 bit instead of 16 bit to support higher vertices count.
        terrainMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        // Add required information to terrainMesh for mesh generation.
        Vector3[] allVertices = TwoDimensionalVectorsToOne(newVertices);
        terrainMesh.vertices = allVertices;

        // Grab vertices's and make triangles.
        terrainMesh.triangles = ReturnTriangles(allVertices, width, length);

        // Grab normals for mesh - to help with how light interacts with the mesh.
        terrainMesh.normals = GenerateNormals(allVertices, terrainMesh.triangles);

        // Generate UVs.
        Vector2[] allUV = GenerateUVs(newVertices);
        terrainMesh.uv = allUV;

        // Generate Colours.
        Color[] colours = GenerateColour(newVertices, terrainGradient, heightColorChange);
        terrainMesh.colors = colours;

        return terrainMesh;
    }

    /// <summary>
    /// Generates a texture for the terrain. Note that the texture is generated via Perlin noise, using the same inputs as the terrain generation.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    Texture2D GenerateTexture(int width, int length, float scale, Color lowColor, Color highColor)
    {
        Texture2D newTexture = new Texture2D(width, length);

        for(int xCount = 0; xCount < width; xCount++)
        {
            for (int zCount = 0; zCount < length; zCount++)
            {
                float xPerlinCoord = (float)xCount / width;
                float zPerlinCoord = (float)zCount / length;

                float colourNoise = Mathf.PerlinNoise(xPerlinCoord * scale + Information.OffsetX, zPerlinCoord * scale + Information.OffsetZ);

                float r = Mathf.Lerp(lowColor.r, highColor.r, colourNoise);
                float g = Mathf.Lerp(lowColor.g, highColor.g, colourNoise);
                float b = Mathf.Lerp(lowColor.b, highColor.b, colourNoise);

                newTexture.SetPixel(xCount, zCount, new Color(r, g, b));
            }
        }

        newTexture.Apply();
        return newTexture;
    }

    /// <summary>
    /// Generates a texture for the terrain. The height of the peaks determine the color gradient being applied.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    Texture2D GenerateTexture(Vector3[,] vertices, Color lowColor, Color highColor, float heightColorChange)
    {
        int width = vertices.GetLength(0);
        int length = vertices.GetLength(1);

        Texture2D newTexture = new Texture2D(width, length);

        float expectedMaxHeight = 0;

        for (int xCount = 0; xCount < width; xCount++)
        {
            for (int zCount = 0; zCount < length; zCount++)
            {
                if (vertices[xCount, zCount].y > expectedMaxHeight)
                {
                    expectedMaxHeight = vertices[xCount, zCount].y;
                }
            }
        }

        for (int xCount = 0; xCount < width; xCount++)
        {
            for (int zCount = 0; zCount < length; zCount++)
            {
                float noramlisedHeight = vertices[xCount, zCount].y / heightColorChange;

                Color normalisedColor = Color.Lerp(lowColor, highColor, noramlisedHeight);

                newTexture.SetPixel(xCount, zCount, normalisedColor);
            }
        }

        newTexture.Apply();
        return newTexture;
    }

    /// <summary>
    /// Gradient based texture generation.
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="terrainGradient"></param>
    /// <param name="heightColorChange"></param>
    /// <returns></returns>
    Texture2D GenerateTexture(Vector3[,] vertices, Gradient terrainGradient, float heightColorChange)
    {
        int width = vertices.GetLength(0);
        int length = vertices.GetLength(1);

        Texture2D newTexture = new Texture2D(width, length);

        for (int xCount = 0; xCount < width; xCount++)
        {
            for (int zCount = 0; zCount < length; zCount++)
            {
                float noramlisedHeight = vertices[xCount, zCount].y / heightColorChange;

                Color normalisedColor = terrainGradient.Evaluate(noramlisedHeight);

                newTexture.SetPixel(xCount, zCount, normalisedColor);
            }
        }

        newTexture.Apply();
        return newTexture;
    }

    /// <summary>
    /// Gradient based vertex colour generation for applying directly to the mesh.
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="terrainGradient"></param>
    /// <param name="heightColorChange"></param>
    /// <returns></returns>
    Color[] GenerateColour(Vector3[,] vertices, Gradient terrainGradient, float heightColorChange)
    {
        int width = vertices.GetLength(0);
        int length = vertices.GetLength(1);

        Color[] newColours = new Color[width * length];

        int counter = 0;
        for (int xCount = 0; xCount < width; xCount++)
        {
            for (int zCount = 0; zCount < length; zCount++)
            {
                // This ensures that the values of the terrain colour that are set between the world height values of zero and the allowed set value for changing colour via the heightColourChange variable.
                float normalisedHeight = Mathf.InverseLerp(0, heightColorChange, vertices[xCount, zCount].y);

                Color normalisedColor = terrainGradient.Evaluate(normalisedHeight);

                newColours[counter] = normalisedColor;

                counter++;
            }
        }

        return newColours;
    }
}
