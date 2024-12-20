using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerUI : UIManager
{
    [field: SerializeField, Header("UserInterface")]
    public TMP_Dropdown RenderDistance
    { get; private set; }

    [field: SerializeField]
    public Button RollingHillsCurve
    { get; private set; }

    [field: SerializeField]
    public Button MountainsCurve
    { get; private set; }

    [field: SerializeField]
    public Button DesertCurve
    { get; private set; }

    [field: SerializeField]
    public Button AridRock
    { get; private set; }

    [field: SerializeField]
    public Button IslandFlat
    { get; private set; }

    [field: SerializeField]
    public Button IslandHill
    { get; private set; }

    [field: SerializeField]
    public Button Canyons
    { get; private set; }

    [field: SerializeField]
    public TMP_Dropdown WalkerSelectionDropdown
    { get; private set; }



    [field: SerializeField, Header("Script References")]
    public TerrainManager TerrainMangerObject
    { get; private set; }

    [field: SerializeField]
    public TerrainCurveOptions TerrainCurveOptions
    { get; private set; }

    /// <summary>
    /// The current selected preset of terrain information chosen.
    /// </summary>
    public TerrainCurveOptions.TerrainRange SelectedPreSet
    { get; private set; }

    public int SeedValue
    { get; private set; }

    [field: SerializeField]
    public GameObject SpiderWalkerNormal
    { get; private set; }

    [field: SerializeField]
    public GameObject SpiderWalkerLarge
    { get; private set; }

    public enum SpiderWalker
    { 
        Normal,
        Large
    }

    public SpiderWalker SelectedWalker
    { get; private set; }

    [field: SerializeField]
    public GameObject CollectablePrefab
    { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        InitUIValues();

        // Default value for render distance.
        RenderDistance.value = 1;
    }

    // Start is called before the first frame update
    protected override void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadScene("MenuScene");
        }

        // Only onces the GameManager has loads the meshes is this set to true, allowing player walker to spawn.
        if (TerrainManager.IsTerrainLoaded == true)
        {
            SpawnWalker();
            TerrainManager.IsTerrainLoaded = false;
            GetComponentInChildren<Canvas>().worldCamera = GameObject.FindWithTag("Player").GetComponentInChildren<Camera>();


            // Spawn game objects around the scene that can be collected.
            foreach (GameObject terrain in TerrainManager.TerrainsList)
            {
                if (terrain == null)
                {
                    break;
                }

                MeshFilter terrainMesh = terrain.GetComponent<MeshFilter>();

                Vector3 randomVertex = terrainMesh.mesh.vertices[Random.Range(0, terrainMesh.mesh.vertices.Length)];

                Vector3 worldSpace = terrain.transform.TransformPoint(randomVertex);

                Instantiate(CollectablePrefab, worldSpace + new Vector3(0, 0.3f, 0), Quaternion.identity);
            }

        }
    }

    public override void InitUIValues()
    {
        base.InitUIValues();

        RenderDistance.onValueChanged.AddListener((value) => { RenderDistance.value = value; });

        RollingHillsCurve.onClick.AddListener(() => {
            SelectPresetTerrainValues(TerrainCurveOptions.RollingHills);
            TerrainSelectionSetup(SelectedPreSet);
        });
        MountainsCurve.onClick.AddListener(() => {
            SelectPresetTerrainValues(TerrainCurveOptions.Mountains);
            TerrainSelectionSetup(SelectedPreSet);
        });
        DesertCurve.onClick.AddListener(() => {
            SelectPresetTerrainValues(TerrainCurveOptions.DesertDunes);
            TerrainSelectionSetup(SelectedPreSet);
        });
        AridRock.onClick.AddListener(() => {
            SelectPresetTerrainValues(TerrainCurveOptions.RockyArid);
            TerrainSelectionSetup(SelectedPreSet);
        });
        IslandFlat.onClick.AddListener(() => {
            SelectPresetTerrainValues(TerrainCurveOptions.IslandsFlat);
            TerrainSelectionSetup(SelectedPreSet);
        });
        IslandHill.onClick.AddListener(() => {
            SelectPresetTerrainValues(TerrainCurveOptions.IslandsHills);
            TerrainSelectionSetup(SelectedPreSet);
        });
        Canyons.onClick.AddListener(() => {
            SelectPresetTerrainValues(TerrainCurveOptions.Canyons);
            TerrainSelectionSetup(SelectedPreSet);
        });

        WalkerSelectionDropdown.onValueChanged.AddListener((value) =>
        {
            if (value == 0)
            {
                SelectedWalker = SpiderWalker.Normal;
            }
            else if (value == 1)
            {
                SelectedWalker = SpiderWalker.Large;
            }
        });
    }

    public void SelectPresetTerrainValues(TerrainCurveOptions.TerrainRange chosenRange)
    {
        SelectedPreSet = chosenRange;
    }

    public void TerrainSelectionSetup(TerrainCurveOptions.TerrainRange currentTerrainRange)
    {
        TerrainInformation newInformation = TerrainCurveOptions.ParseTerrainRangeIntoInformation(currentTerrainRange);

        TerrainMangerObject.GlobalTerrainInformation = newInformation;
        TerrainMangerObject.GlobalTerrainInformation.Seed = SeedValue;
        TerrainMangerObject.TerrainRenderDistance = Mathf.Clamp(RenderDistance.value, 0, 20);

        TerrainMangerObject.transform.gameObject.SetActive(true);
    }

    public void ToggleGameObjectActiveState(GameObject disable)
    {
        disable.SetActive(!disable.activeInHierarchy);
    }

    public void SpawnWalker()
    {
        Vector3 position = GameObject.Find("Terrain0").GetComponent<MeshRenderer>().bounds.center;

        if (SelectedWalker == SpiderWalker.Normal)
        {
            Instantiate(SpiderWalkerNormal, position, Quaternion.identity);
        }
        else if (SelectedWalker == SpiderWalker.Large)
        {
            Instantiate(SpiderWalkerLarge, position, Quaternion.identity);
        }
    }
}
