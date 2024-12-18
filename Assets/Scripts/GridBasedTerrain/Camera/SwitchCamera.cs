using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCamera : MonoBehaviour
{
    [field: SerializeField]
    public Button SwitchCameraButton
    {  get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI CameraTypeText
    { get; private set; }

    [field: SerializeField]
    public Camera CameraMain
    { private set; get; }

    [field: SerializeField]
    public Camera CameraFreeLook
    { get; private set; }

    [field: SerializeField]
    public Canvas UICanvas
    { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        SwitchCameraButton.onClick.AddListener(() => SwitchCameraEvent());
        CameraTypeText.text = "Fixed Overhead";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCameraEvent();
        }
    }

    public void SwitchCameraEvent()
    {
        if (CameraMain.isActiveAndEnabled)
        {
            CameraMain.enabled = false;
            CameraFreeLook.enabled = true;
            UICanvas.worldCamera = CameraFreeLook;
            CameraTypeText.text = "Freelook";
        }
        else if (CameraFreeLook.isActiveAndEnabled)
        {
            CameraFreeLook.enabled = false;
            CameraMain.enabled = true;
            UICanvas.worldCamera = CameraMain;
            CameraTypeText.text = "Fixed Overhead";
        }
    }
}
