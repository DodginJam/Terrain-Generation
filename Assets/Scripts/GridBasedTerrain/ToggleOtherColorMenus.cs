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
        // Ensure the TerrainGradient property of this script points to the same Gradient as the global TerrainManager script.
        TerrainInformation GlobalTerrainInfo = GameObject.Find("TerrainManager").GetComponent<TerrainManager>().GlobalTerrainInformation;
        TerrainGradient = GlobalTerrainInfo.TerrainGradient;

        // Set the colours, alpha and times floats into the relevent objects in the colour UI game objects.
        SetColourAndAlphaReferences(TerrainGradient.colorKeys, TerrainGradient.alphaKeys);
    }

    // Start is called before the first frame update
    void Start()
    {


        
        // Grab the reference to the colour UI game objects and set them to the gradient attached.
        GrabColourReferences();
        GrabTimeReference();
        
        SetGradientColoursAndTime(TerrainColours, TerrainTimeValues);
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

    /// <summary>
    /// Fill the colour array in the script to the colours set in a childs script.
    /// </summary>
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

    /// <summary>
    /// Fill the time array in the script to the colours set in a childs script.
    /// </summary>
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

    /// <summary>
    /// Set the Gradient values in this script to the Colour and Time arrays already in the script.
    /// </summary>
    public void SetGradientColoursAndTime()
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

    /// <summary>
    /// Set the gradient values in this scipt to the Colour and Time arrays passed as arguments.
    /// </summary>
    /// <param name="newColorArray"></param>
    /// <param name="newTimeValues"></param>
    public void SetGradientColoursAndTime(Color[] newColorArray, float[] newTimeValues)
    {
        GradientColorKey[] newColourKeys = new GradientColorKey[newColorArray.Length];
        GradientAlphaKey[] newAlphaKeys = new GradientAlphaKey[newColorArray.Length];

        for (int i = 0; i < newColorArray.Length; i++)
        {
            newColourKeys[i] = new GradientColorKey(newColorArray[i], newTimeValues[i]);

            newAlphaKeys[i] = new GradientAlphaKey(1, (float)i / newColorArray.Length);
        }

        TerrainGradient.SetKeys(newColourKeys, newAlphaKeys);
    }

    public void SetColourAndAlphaReferences(GradientColorKey[] colourKeys, GradientAlphaKey[] alphaKeys)
    {
        SetColourDisplay[] coloursArray = GetComponentsInChildren<SetColourDisplay>();

        for (int i = 0; i < colourKeys.Length; i++)
        {
            coloursArray[i].SetColour(colourKeys[i].color);
            coloursArray[i].SetTime(colourKeys[i].time);
        }
    }
}
