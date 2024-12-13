using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [field: SerializeField]
    public TerrainInformation GlobalTerrainInformation
    { get; private set; }

    [field: SerializeField]
    public static List<GameObject> TerrainsList
    { get; private set; } = new List<GameObject>();

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
            ClearAndLoad();
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
                GlobalTerrainInformation.TerrainGradient,
                GlobalTerrainInformation.HeightColorChange,
                GlobalTerrainInformation.EnableSmoothing,
                GlobalTerrainInformation.Position + terrainPositions[i],
                GlobalTerrainInformation.TerrainMaterial,
                GlobalTerrainInformation.Seed,
                GlobalTerrainInformation.Octaves,
                GlobalTerrainInformation.Persistance,
                GlobalTerrainInformation.Lacunarity,
                GlobalTerrainInformation.OctaveOffset,
                GlobalTerrainInformation.TerrainCurve
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

        int completedTerrains = 0;

        for (int currentLayer = 0; currentLayer <= maxDistanceFromCentre; currentLayer++)
        {
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
                        positionalOffsets[completedTerrains] = new Vector3((float)xCount * GlobalTerrainInformation.GridXLength, 0, (float)zCount * GlobalTerrainInformation.GridZLength) * (float)GlobalTerrainInformation.GridSpacing;

                        // Assign the perlin offsets that would be required for the meshes adjacent and further away from centre.
                        perlinXOffset[completedTerrains] = (positionalOffsets[completedTerrains].x / GlobalTerrainInformation.GridSpacing / GlobalTerrainInformation.PerlinScale);
                        perlinZOffset[completedTerrains] = (positionalOffsets[completedTerrains].z / GlobalTerrainInformation.GridSpacing / GlobalTerrainInformation.PerlinScale);

                        completedTerrains++;
                    }
                }
            }
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
                                        GlobalTerrainInformation.TerrainGradient,
                                        GlobalTerrainInformation.HeightColorChange,
                                        GlobalTerrainInformation.EnableSmoothing,
                                        GlobalTerrainInformation.Position,
                                        GlobalTerrainInformation.TerrainMaterial,
                                        GlobalTerrainInformation.Seed,
                                        GlobalTerrainInformation.Octaves,
                                        GlobalTerrainInformation.Persistance,
                                        GlobalTerrainInformation.Lacunarity,
                                        GlobalTerrainInformation.OctaveOffset,
                                        GlobalTerrainInformation.TerrainCurve
                                        );

            yield return new WaitForSeconds(timeTillNextCheck);

            bool areValuesSame = oldInformation.GridXLength == GlobalTerrainInformation.GridXLength
                        && oldInformation.GridZLength == GlobalTerrainInformation.GridZLength
                        && oldInformation.GridSpacing == GlobalTerrainInformation.GridSpacing
                        && oldInformation.GridYHeightRange == GlobalTerrainInformation.GridYHeightRange
                        && oldInformation.HeightColorChange == GlobalTerrainInformation.HeightColorChange

                        // The below doesn't work as it is a reference comparision.
                        && oldInformation.TerrainGradient == GlobalTerrainInformation.TerrainGradient

                        && oldInformation.GridYHeightMultiplier == GlobalTerrainInformation.GridYHeightMultiplier
                        && oldInformation.EnableSmoothing == GlobalTerrainInformation.EnableSmoothing
                        && oldInformation.PerlinScale == GlobalTerrainInformation.PerlinScale
                        && oldInformation.OffsetX == GlobalTerrainInformation.OffsetX
                        && oldInformation.OffsetZ == GlobalTerrainInformation.OffsetZ
                        && oldInformation.Position == GlobalTerrainInformation.Position

                        // The below doesn't work as it is a reference comparision.
                        && oldInformation.TerrainMaterial.Equals(GlobalTerrainInformation.TerrainMaterial)

                        && oldInformation.Seed == GlobalTerrainInformation.Seed
                        && oldInformation.Octaves == GlobalTerrainInformation.Octaves
                        && oldInformation.Persistance == GlobalTerrainInformation.Persistance
                        && oldInformation.Lacunarity == GlobalTerrainInformation.Lacunarity
                        && oldInformation.OctaveOffset == GlobalTerrainInformation.OctaveOffset

                        // The below doesn't work as it is a reference comparision.
                        && oldInformation.TerrainCurve == GlobalTerrainInformation.TerrainCurve;


            if (areValuesSame)
            {
                continue;
            }

            ClearAndLoad();

        }
    }
    void ClearTerrainData()
    {
        foreach (GameObject terrain in TerrainsList)
        {
            Destroy(terrain.gameObject);
        }

        TerrainsList.Clear();
    }

    public void ClearAndLoad()
    {
        ClearTerrainData();

        GenerateNewTerrainList(TerrainRenderDistance);
    }
}
