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

    public bool AllowTerrainUpdate
    { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        GenerateNewTerrainList(TerrainRenderDistance);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Generate a new list of TerrainObjects - assign them global information modifyed for their local positions in the world.
    /// </summary>
    /// <param name="terrainRenderDistance"></param>
    void GenerateNewTerrainList(int terrainRenderDistance)
    {
        // Calculate the number of terrain meshes to generate based on the render distance. Total Blocks = ((2 * renderdistance) + 1)POW2
        int numberOfTerrains = (int)Mathf.Pow((terrainRenderDistance * 2) + 1, 2);

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
                GlobalTerrainInformation.OffsetX,
                GlobalTerrainInformation.OffsetZ,
                GlobalTerrainInformation.GridYHeightRange,
                GlobalTerrainInformation.GridYHeightMultiplier,
                GlobalTerrainInformation.TerrainColourLow,
                GlobalTerrainInformation.TerrainColourHigh,
                GlobalTerrainInformation.HeightColorChange,
                GlobalTerrainInformation.EnableSmoothing
                );


            // Need to modify the information passed into the currentTerrainInformation TerrainInformation variable to reflect the perlin offsets, perlin scale, spacing and gridX and gridZ the object will need to maintain correct positioning and terrain continuation.

            // Pass the currentTerrainInformation to the terrainObject being generated.
            terrainObject.Information = currentTerrainInformation;
            terrainObject.TerrainName = terrainName;

            TerrainsList.Add(currentTerrain);

            currentTerrain.transform.position = new Vector3(0, 0, terrainObject.Information.GridXLength) * (i + 1);
        }
    }
}
