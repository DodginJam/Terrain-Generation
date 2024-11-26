using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [field: SerializeField]
    public TerrainInformation GlobalTerrainInformation
    { get; private set; }

    [field: SerializeField]
    public List<GameObject> TerrainsList
    { get; private set; } = null;

    [field: SerializeField]
    public int TerrainRenderDistance
    { get; private set; } = 1;

    // Start is called before the first frame update
    void Start()
    {
        // Generate a grid of terrain meshes
        GenerateNewTerrainList(TerrainRenderDistance);
        // Coroutine here looks for changes in terrain information, and if detected, generates a new terrain mesh grid.
        StartCoroutine(UpdateMeshOnInputChange());
    }

    // Update is called once per frame
    void Update()
    {
        // Can manually re-generate the mesh on input.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var terrain in TerrainsList)
            {
                Destroy(terrain.gameObject);
            }

            TerrainsList.Clear();

            GenerateNewTerrainList(TerrainRenderDistance);
        }
    }

    /// <summary>
    /// Generate a new list of TerrainObjects - assign them global information modified for their local positions in the world.
    /// </summary>
    /// <param name="terrainRenderDistance"></param>
    void GenerateNewTerrainList(int terrainRenderDistance)
    {
        // Calculate the number of terrain meshes to generate based on the render distance. Total Blocks = ((2 * renderdistance) + 1)POW2
        int numberOfTerrains = (int)Mathf.Pow((terrainRenderDistance * 2) + 1, 2);

        GeneratePositionalOffsets(terrainRenderDistance, out Vector3[] terrainPositions, out float[] perlinX, out float[] perlinZ);

        for (int i = 0; i < numberOfTerrains; i++)
        {
            string terrainName = $"Terrain{i}";

            // Generate a new gameobject in the world for containing TerrainObject script and add the script.
            GameObject currentTerrain = new GameObject(terrainName);

            TerrainObject terrainObject = currentTerrain.AddComponent<TerrainObject>();

            // Create a new TerrainInformation and pass the global variables to it.
            TerrainInformation currentTerrainInformation = new TerrainInformation(
                GlobalTerrainInformation.GridXLength, 
                GlobalTerrainInformation.GridZLength, 
                GlobalTerrainInformation.GridSpacing, 
                GlobalTerrainInformation.PerlinScale,
                GlobalTerrainInformation.OffsetX + perlinX[i],
                GlobalTerrainInformation.OffsetZ + perlinZ[i],
                GlobalTerrainInformation.GridYHeightRange,
                GlobalTerrainInformation.GridYHeightMultiplier,
                GlobalTerrainInformation.TerrainColourLow,
                GlobalTerrainInformation.TerrainColourHigh,
                GlobalTerrainInformation.TerrainGradient,
                GlobalTerrainInformation.HeightColorChange,
                GlobalTerrainInformation.EnableSmoothing,
                GlobalTerrainInformation.Position + terrainPositions[i]
                );

            // Pass the currentTerrainInformation, which should be a modified version of global data, to the terrainObject being generated.
            terrainObject.Information = currentTerrainInformation;

            terrainObject.TerrainName = terrainName;

            TerrainsList.Add(currentTerrain);

            // currentTerrain.transform.position = new Vector3(0, 0, terrainObject.Information.GridXLength) * (i + 1) * terrainObject.Information.GridSpacing;
        }
    }

    /// <summary>
    /// Create a set of Vector3 coordinates and perlin offsets based on the position of the Terrain around the centre.
    /// </summary>
    /// <param name="maxDistanceFromCentre"></param>
    /// <param name="positionalOffsets"></param>
    /// <param name="perlinXOffset"></param>
    /// <param name="perlinZOffset"></param>
    void GeneratePositionalOffsets(int maxDistanceFromCentre, out Vector3[] positionalOffsets, out float[] perlinXOffset, out float[] perlinZOffset)
    {
        int totalTerrainsToRender = (int)Mathf.Pow((maxDistanceFromCentre * 2) + 1, 2);

        // The arrays to be sent out of method.
        positionalOffsets = new Vector3[totalTerrainsToRender];
        perlinXOffset = new float[totalTerrainsToRender];
        perlinZOffset = new float[totalTerrainsToRender];

        // The first centre position is always going to be centered with northing additional to it's values.
        positionalOffsets[0] = new Vector3(0, 0, 0);
        perlinXOffset[0] = GlobalTerrainInformation.OffsetX;
        perlinZOffset[0] = GlobalTerrainInformation.OffsetZ;

        int completedTerrains = 1; 

        if (maxDistanceFromCentre > 0)
        {
            for (int currentLayer = 1; currentLayer <= maxDistanceFromCentre; currentLayer++)
            {
                //int numberOfBlocksInCurrentLayer = currentLayer * 8;

                // Treating the layer like a whole grid, and then performing boundary checks within the next for loop to ensure only the edge of the grid, the actual terrain of the layer, get's a positional and perlin offset generated.
                int xLength = 2 * currentLayer + 1;
                int zLength = 2 * currentLayer + 1;

                int lowerXBoundry = -(xLength / 2);
                int higherXBoundry = xLength / 2;

                int lowerZBoundry = -(zLength / 2);
                int higherZBoundry = zLength / 2;


                for (int xCount = lowerXBoundry; xCount <= higherXBoundry; xCount++)
                {
                    for (int zCount = lowerZBoundry; zCount <= higherZBoundry; zCount++)
                    {
                        if (xCount == lowerXBoundry || zCount == lowerZBoundry || xCount == higherXBoundry || zCount == higherZBoundry)
                        {
                            // Apply the positional offset of the terrain objects.
                            positionalOffsets[completedTerrains] = new Vector3(xCount * GlobalTerrainInformation.GridXLength, 0, zCount * GlobalTerrainInformation.GridZLength) * GlobalTerrainInformation.GridSpacing;

                            // Need to calculate the Perlin noise offset to apply here.
                            perlinXOffset[completedTerrains] = (xCount * GlobalTerrainInformation.GridXLength / GlobalTerrainInformation.GridSpacing);
                            perlinZOffset[completedTerrains] = (zCount * GlobalTerrainInformation.GridZLength / GlobalTerrainInformation.GridSpacing);

                            //Debug.Log($"Perlin X Offset for Terrain {completedTerrains}: {perlinXOffset[completedTerrains]}");
                            //Debug.Log($"Perlin Z Offset for Terrain {completedTerrains}: {perlinZOffset[completedTerrains]}");


                            completedTerrains++;
                        }
                    }
                }

                //Debug.Log($"Layer {currentLayer}, Terrains in Layer: {numberOfBlocksInCurrentLayer}, Terrains Completed: {completedTerrains}");
            }

            //Debug.Log($"Total Terrains Rendered: {completedTerrains}");
        }
    }

    /// <summary>
    /// For any value changed that impacts the mesh, remove the existing mesh and generate a new mesh.
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateMeshOnInputChange()
    {
        float timeTillNextCheck = 0.01f;

        while (true)
        {
            TerrainInformation oldInformation = new TerrainInformation
                                        (
                                        GlobalTerrainInformation.GridXLength,
                                        GlobalTerrainInformation.GridZLength,
                                        GlobalTerrainInformation.GridSpacing,
                                        GlobalTerrainInformation.PerlinScale,
                                        GlobalTerrainInformation.OffsetX,
                                        GlobalTerrainInformation.OffsetZ,
                                        GlobalTerrainInformation.GridYHeightRange,
                                        GlobalTerrainInformation.GridYHeightMultiplier,
                                        GlobalTerrainInformation.TerrainColourLow,
                                        GlobalTerrainInformation.TerrainColourHigh,
                                        GlobalTerrainInformation.TerrainGradient,
                                        GlobalTerrainInformation.HeightColorChange,
                                        GlobalTerrainInformation.EnableSmoothing,
                                        GlobalTerrainInformation.Position
                                        );

            yield return new WaitForSeconds(timeTillNextCheck);

            bool areValuesSame = oldInformation.GridXLength == GlobalTerrainInformation.GridXLength
                        && oldInformation.GridZLength == GlobalTerrainInformation.GridZLength
                        && oldInformation.GridSpacing == GlobalTerrainInformation.GridSpacing
                        && oldInformation.GridYHeightRange == GlobalTerrainInformation.GridYHeightRange
                        && oldInformation.TerrainColourLow == GlobalTerrainInformation.TerrainColourLow
                        && oldInformation.TerrainColourHigh == GlobalTerrainInformation.TerrainColourHigh
                        && oldInformation.HeightColorChange == GlobalTerrainInformation.HeightColorChange
                        && oldInformation.TerrainGradient == GlobalTerrainInformation.TerrainGradient
                        && oldInformation.GridYHeightMultiplier == GlobalTerrainInformation.GridYHeightMultiplier
                        && oldInformation.EnableSmoothing == GlobalTerrainInformation.EnableSmoothing
                        && oldInformation.PerlinScale == GlobalTerrainInformation.PerlinScale
                        && oldInformation.OffsetX == GlobalTerrainInformation.OffsetX
                        && oldInformation.OffsetZ == GlobalTerrainInformation.OffsetZ
                        && oldInformation.Position == GlobalTerrainInformation.Position;

            Debug.Log(areValuesSame);

            if (areValuesSame)
            {
                continue;
            }

            foreach (GameObject terrain in TerrainsList)
            {
                Destroy(terrain.gameObject);
            }

            TerrainsList.Clear();

            GenerateNewTerrainList(TerrainRenderDistance);
        }
    }
}
