using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.UI;
using UnityEngine.UI;
using TMPro;

public class TerrainManagerUI : UIManager
{
    public TerrainManager TerrainManagerScript
    {  get; private set; }

    [field: SerializeField]
    public TerrainInformation TerrainManagerInformation
    { get; private set; }

    [field: SerializeField]
    public Slider GridXLengthSlider
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
        SetSliderValues(GridXLengthSlider, 1, 2000, TerrainManagerInformation.GridXLength);
    }

    public void SetListeners()
    {
        GridXLengthSlider.onValueChanged.AddListener(value =>
        {
            TerrainManagerInformation.GridXLength = (int)value;
            SetTextBoxValue(GridXLengthSlider.transform.parent.Find("SliderValue").GetComponent<TextMeshProUGUI>(), value);
        });
        
    }
}
