using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpiderControl : MonoBehaviour
{
    public float sensitivity
    { get; private set; } = 3.0f;

    public float maxVerticalAngle
    { get; private set; } = 85.0f;

    public float rotationX
    { get; private set; } = 0.0f;

    public bool MouseControlOn
    { get; private set; } = true;

    [field: SerializeField]
    public GameObject SpiderControlsUI
    { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (MouseControlOn)
        {
            // Get mouse movement
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Rotate the camera horizontally
            transform.Rotate(Vector3.up, mouseX * sensitivity, Space.World);

            // Rotate the camera vertically
            rotationX -= mouseY * sensitivity;
            rotationX = Mathf.Clamp(rotationX, -maxVerticalAngle, maxVerticalAngle);

            // Apply the vertical rotation
            Vector3 currentRotation = transform.localEulerAngles;
            currentRotation.x = rotationX;
            transform.localEulerAngles = currentRotation;
        }

        if (Input.GetKeyDown(KeyCode.Z) && MouseControlOn == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            MouseControlOn = false;

            if (SpiderControlsUI != null)
            {
                SpiderControlsUI.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z) && MouseControlOn == false)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            MouseControlOn = true;

            if (SpiderControlsUI != null)
            {
                SpiderControlsUI.SetActive(false);
            }
        }
    }
}
