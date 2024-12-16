using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeLockCameraControl : MonoBehaviour
{
    public float sensitivity
    { get; private set; } = 3.0f; 

    public float maxVerticalAngle
    { get; private set; } = 85.0f;

    public float rotationX
    { get; private set; } = 0.0f;

    public float ForwardMovement
    { get; private set; }

    public float SideMovement
    { get; private set; }

    private float verticalMovement;
    public float VerticalMovement
    { 
        get { return verticalMovement; }
        set
        {
            verticalMovement = Mathf.Clamp(value, -1.0f, 1.0f);
        }
    }

    [field: SerializeField]
    public float Speed
    { get; private set; }

    [field: SerializeField]
    public float MovementSpeedMultiplyer
    { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) && GetComponent<Camera>().isActiveAndEnabled) 
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

        float movementBoost;
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            movementBoost = 1;
        }
        else
        {
            movementBoost = MovementSpeedMultiplyer;
        }

        ForwardMovement = Input.GetAxisRaw("Horizontal");
        SideMovement = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.E))
        {
            VerticalMovement = 1;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            VerticalMovement = -1;
        }
        else
        {
            VerticalMovement = 0;
        }
        

        transform.Translate(Time.deltaTime * Speed * movementBoost * new Vector3(ForwardMovement, VerticalMovement, SideMovement).normalized);
    }
}
