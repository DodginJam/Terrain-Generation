using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetColourDisplay : MonoBehaviour
{
    [field: SerializeField]
    public Color CurrentColour
    {  get; private set; }

    [field: SerializeField]
    public float CurrentTime
    { get; private set; }

    [field: SerializeField]
    public RawImage ColourDisplay
    { get; private set; }

    private float redValue;
    public float RedValue
    { 
        get { return redValue; }
        private set
        { 
            redValue = Mathf.Clamp(value, 0.0f, 1.0f);
        } 
    }

    private float greenValue;
    public float GreenValue
    {
        get { return greenValue; }
        private set
        {
            greenValue = Mathf.Clamp(value, 0.0f, 1.0f);
        }
    }

    private float blueValue;
    public float BlueValue
    {
        get { return blueValue; }
        private set
        {
            blueValue = Mathf.Clamp(value, 0.0f, 1.0f);
        }
    }

    [field: SerializeField, Header("Colour Sliders")]
    public Slider RedSlider
    { get; private set; }

    [field: SerializeField]
    public Slider Greenlider
    { get; private set; }

    [field: SerializeField]
    public Slider BlueSlider
    { get; private set; }

    [field: SerializeField]
    public Slider TimeSlider
    { get; private set; }

    private void Awake()
    {
        RedSlider.onValueChanged.AddListener(value =>
        {
            RedValue = value;
        });

        Greenlider.onValueChanged.AddListener(value =>
        {
            GreenValue = value;
        });

        BlueSlider.onValueChanged.AddListener(value =>
        {
            BlueValue = value;
        });

        TimeSlider.onValueChanged.AddListener(value =>
        {
            CurrentTime = value;
            UpdateTime();
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        ColourDisplay.color = CurrentColour;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Updates the CurrentColour and DisplayColour variables of this script based on the current Red, Green and Blue values (the RGB values are updated via slider listeners).
    /// Then calls method from the parent object script to update the relevent colour array and then terrain gradient.
    /// </summary>
    public void UpdateColour()
    {
        Color newColor = new Color(RedValue, GreenValue, BlueValue);

        ColourDisplay.color = newColor;

        CurrentColour = newColor;

        gameObject.transform.parent.GetComponent<ToggleOtherColorMenus>().GrabColourReferences();
        gameObject.transform.parent.GetComponent<ToggleOtherColorMenus>().SetGradientColoursAndTime();
    }

    /// <summary>
    /// Calls method from the parent object script to update the relevent Time array and then terrain gradient.
    /// </summary>
    public void UpdateTime()
    {
        gameObject.transform.parent.GetComponent<ToggleOtherColorMenus>().GrabTimeReference();
        gameObject.transform.parent.GetComponent<ToggleOtherColorMenus>().SetGradientColoursAndTime();
    }

    // Currently trying to set the slider values for the colours will cause and out of index exception error.
    public void SetColour(Color newColor)
    {
        ColourDisplay.color = newColor;

        CurrentColour = newColor;

        RedValue = newColor.r;
        GreenValue = newColor.g;
        BlueValue = newColor.b;
    }

    /// <summary>
    /// Called from parent script for setting the time values to the master value on start.
    /// </summary>
    /// <param name="newTime"></param>
    public void SetTime(float newTime)
    {
        CurrentTime = newTime;

        TimeSlider.value = newTime;
    }
}
