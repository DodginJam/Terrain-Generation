using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.UI;
using UnityEngine.UI;

public class TerrainManagerUI : MonoBehaviour
{
    public TerrainManager TerrainManagerScript
    {  get; private set; }

    [field: SerializeField]
    public TerrainInformation TerrainManagerInformation
    { get; private set; }

    [field: SerializeField]
    public Slider GridXLengthSlider
    { get; private set; }

    private void Awake()
    {
        TerrainManagerScript = GameObject.FindFirstObjectByType<TerrainManager>();
        TerrainManagerInformation = TerrainManagerScript.GlobalTerrainInformation;
    }

    // Start is called before the first frame update
    void Start()
    {
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

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SetObjectInactive(GameObject uiObject)
    {
        bool isActive = uiObject.activeInHierarchy;

        uiObject.SetActive(!isActive);
    }

    public void SetSliderValues(Slider slider, int min, int max, float newValue)
    {
        slider.minValue = min;
        slider.maxValue = max;
        slider.value = newValue;
    }

    public void InitUIValues()
    {
        SetSliderValues(GridXLengthSlider, 0, 2000, (int)GridXLengthSlider.value);
    }
}
