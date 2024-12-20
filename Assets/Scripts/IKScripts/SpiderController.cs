using DitzelGames.FastIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    /// <summary>
    /// Takes in the noramlised input from the vertical axis.
    /// </summary>
    public float VerticalInput
    { get; private set; }

    /// <summary>
    /// Takes in the noramlised input from the horizontal axis.
    /// </summary>
    public float HorizontalInput
    { get; private set; }

    /// <summary>
    /// The limb end points for purpose of body height calculation.
    /// </summary>
    [field: SerializeField]
    public List<GameObject> LimbsEndPoints
    { get; private set; }

    // Backing field for the property.
    [SerializeField]
    private bool toggleIKMeshVisual; 
    // Property to handle changes.
    public bool ToggleIKMeshVisual
    {
        get { return toggleIKMeshVisual; }
        set
        {
            toggleIKMeshVisual = value;
            MeshVisableSet(value);
        }
    }

    [field: SerializeField]
    public SpiderModifiableValues SpiderModifiableValues
    { get; private set; }

    private void Awake()
    {
        // Ensure visuals of the IK transform components are hidden.
        ToggleIKMeshVisual = false;

        // Grabbing reference to all limb end points via the FastIKFabric script attached.
        FastIKFabric[] limbList = transform.GetComponentsInChildren<DitzelGames.FastIK.FastIKFabric>();

        foreach(FastIKFabric script in limbList)
        {
            GameObject currentObject = script.gameObject;
            LimbsEndPoints.Add(currentObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update loop containing player input.
        if (gameObject.transform.parent.CompareTag("Player"))
        {
            VerticalInput = Input.GetAxisRaw("Vertical");
            HorizontalInput = Input.GetAxisRaw("Horizontal");

            // Forward and back movement.
            gameObject.transform.localPosition += Time.deltaTime * SpiderModifiableValues.MovementSpeed * VerticalInput * gameObject.transform.forward;

            // Rotational movement.
            gameObject.transform.localRotation *= Quaternion.Euler(Time.deltaTime * SpiderModifiableValues.RotationSpeed * HorizontalInput * gameObject.transform.up);

            // Set the body to the average of the leg positions plus offset.
            transform.position = new Vector3(transform.position.x, CalulateAveragePosition(LimbsEndPoints).y + SpiderModifiableValues.BodyOffset.y, transform.position.z);
        }
    }

    /// <summary>
    /// Takes in multiple gameobjects and return the average position.
    /// </summary>
    /// <param name="limbPositions"></param>
    /// <returns></returns>
    public Vector3 CalulateAveragePosition(List<GameObject> limbPositions)
    {
        Vector3 totalPositions = new Vector3(0, 0, 0);

        foreach(GameObject limb in limbPositions)
        {
            totalPositions += limb.transform.position;
        }

        return totalPositions / limbPositions.Count;
    }

    /// <summary>
    /// Toggles the visability of the meshes of objects attached to transform components relating to calculation of limb movement in world space.
    /// </summary>
    /// <param name="setToVisable"></param>
    private void MeshVisableSet(bool setToVisable)
    {
        Debug.Log($"Mesh visibility set to: {setToVisable}");
        GameObject[] ikHelpers = GameObject.FindGameObjectsWithTag("IK_Helper");

        foreach (GameObject ikHelper in ikHelpers)
        {
            ikHelper.GetComponent<MeshRenderer>().enabled = setToVisable;
        }
    }
}
