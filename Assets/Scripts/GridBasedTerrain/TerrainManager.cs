using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TerrainCurveOptions;

public class TerrainManager : MonoBehaviour
{
    [field: SerializeField]
    public TerrainInformation GlobalTerrainInformation
    { get; set; }

    [field: SerializeField]
    public static List<GameObject> TerrainsList
    { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public int TerrainRenderDistance
    { get; set; } = 1;

    [field: SerializeField]
    public GameObject UIGameObject
    { get; private set; }

    public static bool IsTerrainLoaded { get; set; } = false;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        // Generate a grid of terrain meshes
        GenerateNewTerrainList(TerrainRenderDistance);

        // Coroutine here looks for changes in terrain information, and if detected, generates a new terrain mesh grid.
        StartCoroutine(UpdateMeshOnInputChange());

        // Grab reference to UI Manager script in the scene.
        if (GameObject.Find("UIManager").TryGetComponent<UIManager>(out UIManager ui))
        {
            UIGameObject = ui.gameObject;
            // Initalise the UI values of the current UI manager for the - these UI values will reflect and influence the Terrain Information of the Terrain Manager.
            UIGameObject.GetComponent<UIManager>().InitUIValues();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Generate a new list of TerrainObjects - assign them global information modified for their local positions in the world.
    /// </summary>
    /// <param name="terrainRenderDistance"></param>
    void GenerateNewTerrainList(int terrainRenderDistance)
    {
        IsTerrainLoaded = false;

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
                GlobalTerrainInformation.TerrainCurve,
                GlobalTerrainInformation.ColourLockToHeight,
                GlobalTerrainInformation.HeightNormalisation
                );

            // Pass the currentTerrainInformation, which should be a modified version of global data, to the terrainObject being generated.
            terrainObject.Information = currentTerrainInformation;

            terrainObject.TerrainName = terrainName;

            TerrainsList.Add(currentTerrain);
        }

        // Update UI if it exists.
        if (UIGameObject != null)
        {
            UIGameObject.GetComponent<UIManager>().InitUIValues();
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
    
                            perlinXOffset[completedTerrains] = (positionalOffsets[completedTerrains].x / GlobalTerrainInformation.GridSpacing);
                            perlinZOffset[completedTerrains] = (positionalOffsets[completedTerrains].z / GlobalTerrainInformation.GridSpacing);
                        

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
        float timeTillNextCheck = 0.1f;

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
                                        GlobalTerrainInformation.TerrainCurve,
                                        GlobalTerrainInformation.ColourLockToHeight,
                                        GlobalTerrainInformation.HeightNormalisation
                                        );

            // Storing the old Colours/Time and Alpha/Time values.
            GradientColorKey[] oldColours = new GradientColorKey[GlobalTerrainInformation.TerrainGradient.colorKeys.Length];
            GradientAlphaKey[] oldAlphas = new GradientAlphaKey[GlobalTerrainInformation.TerrainGradient.alphaKeys.Length];

            for (int i = 0; i < GlobalTerrainInformation.TerrainGradient.colorKeys.Length; i++)
            {
                oldColours[i].color = GlobalTerrainInformation.TerrainGradient.colorKeys[i].color;
                oldColours[i].time = GlobalTerrainInformation.TerrainGradient.colorKeys[i].time;

                oldAlphas[i].alpha = GlobalTerrainInformation.TerrainGradient.alphaKeys[i].alpha;
                oldAlphas[i].time = GlobalTerrainInformation.TerrainGradient.alphaKeys[i].time;
            }


            int oldTerrainRenderDistance = TerrainRenderDistance;

            yield return new WaitForSeconds(timeTillNextCheck);

            // Storing the new Colours/Time and Alpha/Time values.
            GradientColorKey[] newColours = new GradientColorKey[GlobalTerrainInformation.TerrainGradient.colorKeys.Length];
            GradientAlphaKey[] newAlphas = new GradientAlphaKey[GlobalTerrainInformation.TerrainGradient.alphaKeys.Length];

            for (int i = 0; i < GlobalTerrainInformation.TerrainGradient.colorKeys.Length; i++)
            {
                newColours[i].color = GlobalTerrainInformation.TerrainGradient.colorKeys[i].color;
                newColours[i].time = GlobalTerrainInformation.TerrainGradient.colorKeys[i].time;

                newAlphas[i].alpha = GlobalTerrainInformation.TerrainGradient.alphaKeys[i].alpha;
                newAlphas[i].time = GlobalTerrainInformation.TerrainGradient.alphaKeys[i].time;
            }

            static bool AreValuesTheSameGC(GradientColorKey[] arrayOld, GradientColorKey[] arrayNew)
            {
                if (arrayOld.Length != arrayNew.Length)
                {
                    return false;
                }

                for (int i = 0; i < arrayOld.Length; i++)
                {
                    if (arrayNew[i].color != arrayOld[i].color)
                    {
                        return false;
                    }
                    if (arrayNew[i].time != arrayOld[i].time)
                    {
                        return false;

                    }
                }

                return true;
            }

            static bool AreValuesTheSameGA(GradientAlphaKey[] arrayOld, GradientAlphaKey[] arrayNew)
            {
                if (arrayOld.Length != arrayNew.Length)
                {
                    return false;
                }

                for (int i = 0; i < arrayOld.Length; i++)
                {
                    if (arrayNew[i].alpha != arrayOld[i].alpha)
                    {
                        return false;
                    }
                    if (arrayNew[i].time != arrayOld[i].time)
                    {
                        return false;
                    }
                }

                return true;
            }

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
                        && oldInformation.ColourLockToHeight == GlobalTerrainInformation.ColourLockToHeight

                        // The below doesn't work as it is a reference comparision.
                        && oldInformation.TerrainCurve == GlobalTerrainInformation.TerrainCurve

                        && oldTerrainRenderDistance == TerrainRenderDistance
                        && AreValuesTheSameGC(oldColours, newColours) && AreValuesTheSameGA(oldAlphas, newAlphas)

                        && oldInformation.HeightNormalisation == GlobalTerrainInformation.HeightNormalisation;


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

    /// <summary>
    /// Used to clear out the terrain gameobject and re-load a new set of meshes.
    /// </summary>
    public void ClearAndLoad()
    {
        ClearTerrainData();

        GenerateNewTerrainList(TerrainRenderDistance);
    }

    public void LoadNewAnimationCurve(TerrainInformation newInformation)
    {
        ClearTerrainData();

        GlobalTerrainInformation.TerrainCurve = newInformation.TerrainCurve;

        GenerateNewTerrainList(TerrainRenderDistance);
    }

    public void LoadNewInformation(TerrainInformation newInformation)
    {
        ClearTerrainData();

        GlobalTerrainInformation = newInformation;

        GenerateNewTerrainList(TerrainRenderDistance);
    }

    public void LoadNewInformation(TerrainInformation newInformation, int renderDistance)
    {
        ClearTerrainData();

        GlobalTerrainInformation = newInformation;

        TerrainRenderDistance = renderDistance;

        GenerateNewTerrainList(TerrainRenderDistance);
    }
}
