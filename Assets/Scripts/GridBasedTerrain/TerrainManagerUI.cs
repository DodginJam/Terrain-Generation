using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class TerrainManagerUI : UIManager
{
    public TerrainManager TerrainManagerScript
    {  get; private set; }

    public TerrainInformation TerrainManagerInformation
    { get; private set; }

    [field: SerializeField, Header("Grid Based UI Elements - Grid Controls")]
    public Slider GridXLengthSlider
    { get; private set; }

    [field: SerializeField]
    public Slider GridZLengthSlider
    { get; private set; }

    [field: SerializeField]
    public Slider GridSpacingSlider
    { get; private set; }

    [field: SerializeField]
    public Slider GridHeightSlider
    { get; private set; }


    [field: SerializeField, Header("Grid Based UI Elements - Terrain Curves Presets")]
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
    public Toggle ChangeTerrainInTotal
    { get; private set; }


    [field: SerializeField, Header("Perlin Based UI Elements")]
    public Slider PerlinScaleSlider
    { get; private set; }

    [field: SerializeField]
    public Slider PerlinOffsetXSlider
    { get; private set; }

    [field: SerializeField]
    public Slider PerlinOffsetZSlider
    { get; private set; }

    [field: SerializeField]
    public Slider PerlinSeedSlider
    { get; private set; }

    [field: SerializeField]
    public TMP_InputField RenderDistanceInput
    { get; private set; }


    [field: SerializeField, Header("Perlin Based UI Elements Second Panel")]
    public Slider OctavesSlider
    { get; private set; }

    [field: SerializeField]
    public Slider PersistanceSlider
    { get; private set; }

    [field: SerializeField]
    public Slider LacunaritySlider
    { get; private set; }

    [field: SerializeField]
    public Toggle SmoothingToggle
    { get; private set; }


    [field: SerializeField, Header("Colour Mesh UI Elements")]
    public Toggle ColourMatchHeightToggle
    { get; private set; }

    [field: SerializeField]
    public Slider ColourHeightChangeSlider
    { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        TerrainManagerScript = GameObject.FindFirstObjectByType<TerrainManager>();
        TerrainManagerInformation = TerrainManagerScript.GlobalTerrainInformation;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetListeners();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadScene("MenuScene");
        }

        if (TerrainManagerScript == null)
        {
            GameObject.FindFirstObjectByType<TerrainManager>();
        }
    }

    public void SetObjectInactive(GameObject uiObject)
    {
        bool isActive = uiObject.activeInHierarchy;

        uiObject.SetActive(!isActive);
    }

    public override void InitUIValues()
    {
        base.InitUIValues();
        
        // Set UI slider values in the scene.
        // Grid Based Slider Values.
        SetSliderValues(GridXLengthSlider, 1, 500, TerrainManagerInformation.GridXLength);
        SetTextBoxValue(GridXLengthSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), TerrainManagerInformation.GridXLength);

        SetSliderValues(GridZLengthSlider, 1, 500, TerrainManagerInformation.GridZLength);
        SetTextBoxValue(GridZLengthSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), TerrainManagerInformation.GridZLength);

        SetSliderValues(GridSpacingSlider, 1, 10, TerrainManagerInformation.GridSpacing);
        SetTextBoxValue(GridSpacingSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), TerrainManagerInformation.GridSpacing);

        SetSliderValues(GridHeightSlider, 0, 500, TerrainManagerInformation.GridYHeightRange);
        SetTextBoxValue(GridHeightSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), TerrainManagerInformation.GridYHeightRange);

        // Perlin Based Slider Values.
        SetSliderValues(PerlinScaleSlider, 1, 300, TerrainManagerInformation.PerlinScale);
        SetTextBoxValue(PerlinScaleSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), TerrainManagerInformation.PerlinScale);

        SetSliderValues(PerlinOffsetXSlider, -500, 500, TerrainManagerInformation.OffsetX);
        SetTextBoxValue(PerlinOffsetXSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), TerrainManagerInformation.OffsetX);

        SetSliderValues(PerlinOffsetZSlider, -500, 500, TerrainManagerInformation.OffsetZ);
        SetTextBoxValue(PerlinOffsetZSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), TerrainManagerInformation.OffsetZ);

        SetSliderValues(PerlinSeedSlider, -500, 500, TerrainManagerInformation.Seed);
        SetTextBoxValue(PerlinSeedSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), TerrainManagerInformation.Seed);

        // Perlin Based Slider Values Second Panel Values.
        SetSliderValues(OctavesSlider, 1, 8, TerrainManagerInformation.Octaves);
        SetTextBoxValue(OctavesSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), TerrainManagerInformation.Octaves);

        SetSliderValues(PersistanceSlider, 0, 1, TerrainManagerInformation.Persistance);
        SetTextBoxValue(PersistanceSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), TerrainManagerInformation.Persistance);

        SetSliderValues(LacunaritySlider, 0.15f, 1.25f, TerrainManagerInformation.Lacunarity);
        SetTextBoxValue(LacunaritySlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), TerrainManagerInformation.Lacunarity);

        // Colour Based Slider Values.
        SetSliderValues(ColourHeightChangeSlider, 0, TerrainManagerInformation.GridYHeightRange * 2f, TerrainManagerInformation.HeightColorChange);
        SetTextBoxValue(ColourHeightChangeSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), TerrainManagerInformation.HeightColorChange);
    }

    public void SetListeners()
    {
        // Grid Based Slider Values.
        GridXLengthSlider.onValueChanged.AddListener(value =>
        {
            TerrainManagerInformation.GridXLength = (int)value;
            SetTextBoxValue(GridXLengthSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), value);
        });

        GridZLengthSlider.onValueChanged.AddListener(value =>
        {
            TerrainManagerInformation.GridZLength = (int)value;
            SetTextBoxValue(GridZLengthSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), value);
        });

        GridSpacingSlider.onValueChanged.AddListener(value =>
        {
            TerrainManagerInformation.GridSpacing = value;
            SetTextBoxValue(GridSpacingSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), value);
        });

        GridHeightSlider.onValueChanged.AddListener(value =>
        {
            TerrainManagerInformation.GridYHeightRange = value;
            SetTextBoxValue(GridHeightSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), value);
        });

        // Perlin Based Slider Values.
        PerlinScaleSlider.onValueChanged.AddListener(value =>
        {
            TerrainManagerInformation.PerlinScale = value;
            SetTextBoxValue(PerlinScaleSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), value);
        });

        PerlinOffsetXSlider.onValueChanged.AddListener(value =>
        {
            TerrainManagerInformation.OffsetX = value;
            SetTextBoxValue(PerlinOffsetXSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), value);
        });

        PerlinOffsetZSlider.onValueChanged.AddListener(value =>
        {
            TerrainManagerInformation.OffsetZ = value;
            SetTextBoxValue(PerlinOffsetZSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), value);
        });

        PerlinSeedSlider.onValueChanged.AddListener(value =>
        {
            TerrainManagerInformation.Seed = (int)value;
            SetTextBoxValue(PerlinSeedSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), value);
        });

        RenderDistanceInput.onEndEdit.AddListener(value =>
        {
            int intValue;

            if (Int32.TryParse(value, out intValue))
            {
                intValue = Mathf.Clamp(intValue, 0, 5);
                TerrainManagerScript.TerrainRenderDistance = intValue;
            }
            else
            {
                intValue = 0;
                TerrainManagerScript.TerrainRenderDistance = intValue;
            }

            RenderDistanceInput.text = intValue.ToString();
        });

        // Perlin Based Slider Values Second Panel Values.
        OctavesSlider.onValueChanged.AddListener(value =>
        {
            TerrainManagerInformation.Octaves = (int)value;
            SetTextBoxValue(OctavesSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), value);
        });

        PersistanceSlider.onValueChanged.AddListener(value =>
        {
            TerrainManagerInformation.Persistance = value;
            SetTextBoxValue(PersistanceSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), value);
        });

        LacunaritySlider.onValueChanged.AddListener(value =>
        {
            TerrainManagerInformation.Lacunarity = value;
            SetTextBoxValue(LacunaritySlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), value);
        });

        // Toggle Smoothing.
        SmoothingToggle.onValueChanged.AddListener(value =>
        {
            TerrainManagerInformation.EnableSmoothing = value;
        });

        // Colour Based Values.
        // This listner for the toggle, when set to true, tanks the framerate - needs investigating.
        ColourMatchHeightToggle.onValueChanged.AddListener(value =>
        {
            TerrainManagerInformation.ColourLockToHeight = value;
            
            if (ColourHeightChangeSlider != null)
            {
                if (value)
                {
                    ChangeSliderColours(ColourHeightChangeSlider, Color.grey);
                }
                else
                {
                    ChangeSliderColours(ColourHeightChangeSlider, Color.white);
                }
            }

            void ChangeSliderColours(Slider slider, Color newColor)
            {
                GameObject parent = slider.transform.parent.gameObject;
                TextMeshProUGUI[] texts = parent.GetComponentsInChildren<TextMeshProUGUI>();

                foreach (TextMeshProUGUI text in texts)
                {
                    text.color = newColor;
                }
            }


        });

        ColourHeightChangeSlider.onValueChanged.AddListener(value =>
        {
            TerrainManagerInformation.HeightColorChange = value;
            SetTextBoxValue(ColourHeightChangeSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), value);
        });


        // TerrainCurve based values buttons have listeners attached - will load the terraindata of a specific pre-set.
        RollingHillsCurve.onClick.AddListener(() => { CurveOrAllData(ChangeTerrainInTotal.isOn, TerrainManagerScript.gameObject.GetComponent<TerrainCurveOptions>().RollingHills.Information); });

        MountainsCurve.onClick.AddListener(() => { CurveOrAllData(ChangeTerrainInTotal.isOn, TerrainManagerScript.gameObject.GetComponent<TerrainCurveOptions>().Mountains.Information); });

        DesertCurve.onClick.AddListener(() => { CurveOrAllData(ChangeTerrainInTotal.isOn, TerrainManagerScript.gameObject.GetComponent<TerrainCurveOptions>().DesertDunes.Information); });

        AridRock.onClick.AddListener(() => { CurveOrAllData(ChangeTerrainInTotal.isOn, TerrainManagerScript.gameObject.GetComponent<TerrainCurveOptions>().RockyArid.Information); });

        IslandFlat.onClick.AddListener(() => { CurveOrAllData(ChangeTerrainInTotal.isOn, TerrainManagerScript.gameObject.GetComponent<TerrainCurveOptions>().IslandsFlat.Information); });

        IslandHill.onClick.AddListener(() => { CurveOrAllData(ChangeTerrainInTotal.isOn, TerrainManagerScript.gameObject.GetComponent<TerrainCurveOptions>().IslandsHills.Information); });

        Canyons.onClick.AddListener(() => { CurveOrAllData(ChangeTerrainInTotal.isOn, TerrainManagerScript.gameObject.GetComponent<TerrainCurveOptions>().Canyons.Information); });

        ChangeTerrainInTotal.onValueChanged.AddListener((value) => {  });
    }

    /// <summary>
    /// Method 
    /// </summary>
    /// <param name="allData"></param>
    /// <param name="newTerrainInformation"></param>
    void CurveOrAllData(bool allData, TerrainInformation newTerrainInformation)
    {
        if (allData)
        {
            TerrainManagerScript.LoadNewInformation(newTerrainInformation);
        }
        else 
        {
            TerrainManagerScript.LoadNewAnimationCurve(newTerrainInformation);
        }
    }
}
