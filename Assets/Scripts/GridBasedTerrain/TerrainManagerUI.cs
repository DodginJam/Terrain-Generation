using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.UI;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class TerrainManagerUI : UIManager
{
    public TerrainManager TerrainManagerScript
    {  get; private set; }

    [field: SerializeField]
    public TerrainInformation TerrainManagerInformation
    { get; private set; }

    [field: SerializeField, Header("Grid Based UI Elements")]
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



    public override void Awake()
    {
        base.Awake();
        TerrainManagerScript = GameObject.FindFirstObjectByType<TerrainManager>();
        TerrainManagerInformation = TerrainManagerScript.GlobalTerrainInformation;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetListeners();
    }

    // Update is called once per frame
    void Update()
    {
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

        SetSliderValues(GridSpacingSlider, 1, 500, TerrainManagerInformation.GridSpacing);
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
    }
}
