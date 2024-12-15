using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public virtual void Awake()
    {

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
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public virtual void InitUIValues()
    {

    }

    public void SetSliderValues(Slider slider, float min, float max, float newValue)
    {
        slider.minValue = min;
        slider.maxValue = max;
        slider.value = newValue;
    }

    public void SetSliderValues(Slider slider, int min, int max, int newValue)
    {
        slider.minValue = min;
        slider.maxValue = max;
        slider.value = newValue;
    }

    public void SetTextBoxValue(TextMeshProUGUI textDisplay, float value)
    {
        textDisplay.text = value.ToString();
    }

    public void SetTextBoxValue(TextMeshProUGUI textDisplay, string value)
    {
        textDisplay.text = value.ToString();
    }
}
