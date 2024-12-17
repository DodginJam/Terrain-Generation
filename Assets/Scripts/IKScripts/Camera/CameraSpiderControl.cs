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

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
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

        if (Input.GetKey(KeyCode.Z))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
