using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    /// <summary>
    /// The length of how many vertices in the X direction.
    /// </summary>
    [field: SerializeField, Header("Grid Layout"), Range(1, 200)]
    public int GridXLength
    { get; private set; } = 50;

    /// <summary>
    /// The length of how many vertices in the Z direction.
    /// </summary>
    [field: SerializeField, Range(1, 200)]
    public int GridZLength
    { get; private set; } = 50;

    /// <summary>
    /// GridSpacing determines the distance between the vertices - essentially the resolution of the grid.
    /// </summary>
    [field: SerializeField, Range(0.1f, 200)]
    public float GridSpacing
    { get; private set; } = 1.1f;

    /// <summary>
    /// PerlinScale changes the detail or resolution of the perlin noise being used in application to the height of the terrain.
    /// </summary>
    [field: SerializeField, Range(0.1f, 200)]
    public float PerlinScale
    { get; private set; } = 1.0f;
    [field: SerializeField]
    public float OffsetX
    { get; private set; } = 0.0f;
    [field: SerializeField]
    public float OffsetZ
    { get; private set; } = 0.0f;

    /// <summary>
    /// Real height range pre-smoothing.
    /// </summary>
    [field: SerializeField, Range(-1000.0f, 1000.0f)]
    public float GridYHeightRange
    { get; private set; } = 1.0f;

    /// <summary>
    /// Multiplier tied to the height range.
    /// </summary>
    [field: SerializeField, Range(0.0f, 1.0f)]
    public float GridYHeightMultiplier
    { get; private set; } = 1.0f;

    [field: SerializeField]
    public Color TerrainColourLow
    { get; private set; } = Color.green;

    [field: SerializeField]
    public Color TerrainColourHigh
    { get; private set; } = Color.white;

    [field: SerializeField]
    public float HeightColorChange
    { get; private set; } = 100.0f;

    [field: SerializeField]
    public bool EnableSmoothing
    { get; private set; } = true;

    [field: SerializeField] public List<GameObject> DisplayTerrains
    { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public int TerrainRenderDistance
    { get; private set; } = 1;

    // Start is called before the first frame update
    void Start()
    {
        DisplayTerrain(TerrainRenderDistance);
        StartCoroutine(UpdateMeshesOnInputChange());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayTerrain(int terrainRenderDistance)
    {
        // Clear out any exisiting terrain from the array and scene.
        foreach (var terrain in DisplayTerrains)
        {
            Destroy(terrain);
        }
        DisplayTerrains.Clear();

        // Calculate the number of terrain meshes to generate based on the render distance. Total Blocks = ((2 * renderdistance) + 1)POW2
        int numberOfTerrains = (int)Mathf.Pow((terrainRenderDistance * 2) + 1, 2);

        for (int i = 0; i < numberOfTerrains; i++)
        {
            string terrainName = $"Terrain{i}";

            // Create new object to attach for terrain to be generated on.
            GameObject currentTerrain = new GameObject(terrainName);
            DisplayTerrains.Add(currentTerrain);

            currentTerrain.AddComponent<TerrainObject>();
            TerrainObject currentTerrainObject = currentTerrain.GetComponent<TerrainObject>();

            currentTerrainObject.Information = new TerrainInformation(GridXLength, GridZLength, GridSpacing, PerlinScale, OffsetX, OffsetZ, GridYHeightRange, GridYHeightMultiplier, TerrainColourLow, TerrainColourHigh, HeightColorChange, EnableSmoothing);
            currentTerrainObject.TerrainName = terrainName;

            // Set position in world.
            currentTerrainObject.transform.position = new Vector3(0, 0, 0);
        }
    }

    IEnumerator UpdateMeshesOnInputChange()
    {
        float timeTillNextCheck = 0.01f;

        int Old_GridXLength;
        int Old_GridZLength;
        float Old_GridSpacing;
        float Old_GridYHeightRange;
        Color Old_TerrainColourLow;
        Color Old_TerrainColourHigh;
        float Old_HeightColorChange;
        float Old_GridYHeightMultiplier;
        bool Old_EnableSmoothing;
        float Old_PerlinScale;
        float Old_OffsetX;
        float Old_OffsetZ;

        while (true)
        {
            Old_GridXLength = GridXLength;
            Old_GridZLength = GridZLength;
            Old_GridSpacing = GridSpacing;
            Old_GridYHeightRange = GridYHeightRange;
            Old_TerrainColourLow = TerrainColourLow;
            Old_TerrainColourHigh = TerrainColourHigh;
            Old_HeightColorChange = HeightColorChange;
            Old_GridYHeightMultiplier = GridYHeightMultiplier;
            Old_EnableSmoothing = EnableSmoothing;
            Old_PerlinScale = PerlinScale;
            Old_OffsetX = OffsetX;
            Old_OffsetZ = OffsetZ;

            yield return new WaitForSeconds(timeTillNextCheck);

            bool areValuesSame = Old_GridXLength == GridXLength
                                    && Old_GridZLength == GridZLength
                                    && Old_GridSpacing == GridSpacing
                                    && Old_GridYHeightRange == GridYHeightRange
                                    && Old_TerrainColourLow == TerrainColourLow
                                    && Old_TerrainColourHigh == TerrainColourHigh
                                    && Old_HeightColorChange == HeightColorChange
                                    && Old_GridYHeightMultiplier == GridYHeightMultiplier
                                    && Old_EnableSmoothing == EnableSmoothing
                                    && Old_PerlinScale == PerlinScale
                                    && Old_OffsetX == OffsetX
                                    && Old_OffsetZ == OffsetZ;

            if (areValuesSame)
            {
                continue;
            }

            DisplayTerrain(TerrainRenderDistance);
        }
    }
}
