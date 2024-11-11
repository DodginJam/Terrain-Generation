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

    void GenerateNewTerrainList(int terrainRenderDistance)
    {
        // Calculate the number of terrain meshes to generate based on the render distance. Total Blocks = ((2 * renderdistance) + 1)POW2
        int numberOfTerrains = (int)Mathf.Pow((terrainRenderDistance * 2) + 1, 2);

        for (int i = 0; i < numberOfTerrains; i++)
        {
            string terrainName = $"Terrain{i}";

            GameObject currentTerrain = new GameObject(terrainName);
            TerrainObject terrainObject = currentTerrain.AddComponent<TerrainObject>();
            terrainObject.Information = GlobalTerrainInformation;
            terrainObject.TerrainName = terrainName;

            TerrainsList.Add(currentTerrain);
        }

    }

    public void DisplayTerrain(int terrainRenderDistance)
    {
        // Clear out any exisiting terrain from the array and scene.
        foreach (var terrain in TerrainsList)
        {
            Destroy(terrain);
        }
        TerrainsList.Clear();

        // Calculate the number of terrain meshes to generate based on the render distance. Total Blocks = ((2 * renderdistance) + 1)POW2
        int numberOfTerrains = (int)Mathf.Pow((terrainRenderDistance * 2) + 1, 2);

        for (int i = 0; i < numberOfTerrains; i++)
        {
            string terrainName = $"Terrain{i}";

            // Create new object to attach for terrain to be generated on.
            GameObject currentTerrain = new GameObject(terrainName);

            currentTerrain.AddComponent<TerrainObject>();
            TerrainObject currentTerrainObject = currentTerrain.GetComponent<TerrainObject>();

            currentTerrainObject.Information = GlobalTerrainInformation;
            currentTerrainObject.TerrainName = terrainName;

            TerrainsList.Add(currentTerrain);

            // Set position in world.
            currentTerrainObject.transform.position = new Vector3(0, 0, 0);
        }
    }
}
