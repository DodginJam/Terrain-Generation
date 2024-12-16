using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOtherColorMenus : MonoBehaviour
{
    [field: SerializeField]
    public GameObject[] ColourSliderDisplays
    {  get; private set; }

    [field: SerializeField]
    public Gradient TerrainGradient
    { get; private set; }

    [field: SerializeField]
    public Color[] TerrainColours
    { get; private set; }

    [field: SerializeField]
    public float[] TerrainTimeValues
    { get; private set; }

    void Awake()
    {
        TerrainInformation GlobalTerrainInfo = GameObject.Find("TerrainManager").GetComponent<TerrainManager>().GlobalTerrainInformation;

        TerrainGradient = GlobalTerrainInfo.TerrainGradient;

        TerrainGradient = GameObject.Find("TerrainManager").GetComponent<TerrainManager>().GlobalTerrainInformation.TerrainGradient;
    }

    // Start is called before the first frame update
    void Start()
    {
        GrabColourReferences();
        GrabTimeReference();
        SetColoursAndTimeToGradient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideDisplays(GameObject currentObject)
    {
        foreach(GameObject colourDisplay in ColourSliderDisplays)
        {
            colourDisplay.transform.Find("ColourRGB").gameObject.SetActive(false);
        }

        currentObject.SetActive(true);
    }

    public void GrabColourReferences()
    {
        SetColourDisplay[] coloursArray = GetComponentsInChildren<SetColourDisplay>();

        TerrainColours = new Color[coloursArray.Length];

        int counter = 0;
        foreach (SetColourDisplay script in coloursArray)
        {
            TerrainColours[counter] = script.CurrentColour;
            counter++;
        }

    }

    public void GrabTimeReference()
    {
        SetColourDisplay[] coloursArray = GetComponentsInChildren<SetColourDisplay>();

        TerrainTimeValues = new float[coloursArray.Length];

        int counter = 0;
        foreach (SetColourDisplay script in coloursArray)
        {
            TerrainTimeValues[counter] = script.CurrentTime;
            counter++;
        }
    }

    public void SetColoursAndTimeToGradient()
    {
        GradientColorKey[] newColourKeys = new GradientColorKey[TerrainColours.Length];
        GradientAlphaKey[] newAlphaKeys = new GradientAlphaKey[TerrainColours.Length];

        for (int i = 0; i < TerrainColours.Length; i++)
        {
            newColourKeys[i] = new GradientColorKey(TerrainColours[i], TerrainTimeValues[i]);

            newAlphaKeys[i] = new GradientAlphaKey(1, (float)i / TerrainColours.Length);
        }

        TerrainGradient.SetKeys(newColourKeys, newAlphaKeys);
    }
}
