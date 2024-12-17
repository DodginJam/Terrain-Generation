using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IKManagerUI : UIManager
{
    [field: SerializeField]
    public SpiderModifiableValues SpiderModifiableValues
    { get; private set; }

    [field: SerializeField]
    public Slider StrideSlider
    { get; private set; }

    [field: SerializeField]
    public Slider LegMoveSlider
    { get; private set; }

    [field: SerializeField]
    public Slider BodyOffsetSlider
    { get; private set; }

    [field: SerializeField]
    public Slider MovementSlider
    { get; private set; }

    [field: SerializeField]
    public Slider RotationSlider
    { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        InitUIValues();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void InitUIValues()
    {
        base.InitUIValues();

        // Leg stride length
        StrideSlider.onValueChanged.AddListener((value) =>
        {
            SpiderModifiableValues.StrideLength = value;
            SetTextBoxValue(StrideSlider.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>(), SpiderModifiableValues.StrideLength);
        });

        SetSliderValues(StrideSlider, 0.1f, 2.0f, SpiderModifiableValues.StrideLength);
        SetTextBoxValue(StrideSlider.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>(), Mathf.Round(SpiderModifiableValues.StrideLength * 10.0f) * 0.1f);

        // Time to move leg.
        LegMoveSlider.onValueChanged.AddListener((value) =>
        {
            SpiderModifiableValues.TimeToMoveLeg = value;
            SetTextBoxValue(LegMoveSlider.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>(), SpiderModifiableValues.TimeToMoveLeg);
        });

        SetSliderValues(LegMoveSlider, 0.01f, 0.6f, SpiderModifiableValues.TimeToMoveLeg);
        SetTextBoxValue(LegMoveSlider.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>(), Mathf.Round(SpiderModifiableValues.TimeToMoveLeg * 10.0f) * 0.1f );

        // Offset body from ground.
        BodyOffsetSlider.onValueChanged.AddListener((value) =>
        {
            SpiderModifiableValues.BodyOffset = new Vector3(SpiderModifiableValues.BodyOffset.x, value, SpiderModifiableValues.BodyOffset.z);
            SetTextBoxValue(BodyOffsetSlider.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>(), SpiderModifiableValues.BodyOffset.y);
        });

        SetSliderValues(BodyOffsetSlider, 0.0f, 1.75f, SpiderModifiableValues.BodyOffset.y);
        SetTextBoxValue(BodyOffsetSlider.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>(), Mathf.Round(SpiderModifiableValues.BodyOffset.y * 10.0f) * 0.1f);

        // Movement Speed.
        MovementSlider.onValueChanged.AddListener((value) =>
        {
            SpiderModifiableValues.MovementSpeed = value;
            SetTextBoxValue(MovementSlider.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>(), SpiderModifiableValues.MovementSpeed);
        });

        SetSliderValues(MovementSlider, 0.0f, 10.0f, SpiderModifiableValues.MovementSpeed);
        SetTextBoxValue(MovementSlider.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>(), Mathf.Round(SpiderModifiableValues.MovementSpeed * 10.0f) * 0.1f);

        // Movement Speed.
        RotationSlider.onValueChanged.AddListener((value) =>
        {
            SpiderModifiableValues.RotationSpeed = value;
            SetTextBoxValue(RotationSlider.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>(), SpiderModifiableValues.RotationSpeed);
        });

        SetSliderValues(RotationSlider, 0.0f, 100.0f, SpiderModifiableValues.RotationSpeed);
        SetTextBoxValue(RotationSlider.transform.parent.Find("Value").GetComponent<TextMeshProUGUI>(), Mathf.Round(SpiderModifiableValues.RotationSpeed * 10.0f) * 0.1f);
    }
}
