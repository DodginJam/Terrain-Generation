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
    /// The offset of the body the average leg position.
    /// </summary>
    [field: SerializeField]
    public Vector3 BodyOffset
    { get; private set; }

    /// <summary>
    /// Speed of the translate of the spider body.
    /// </summary>
    [field: SerializeField]
    public float MovementSpeed
    { get; private set; }

    /// <summary>
    /// Speed of the rotation of the spider body.
    /// </summary>
    [field: SerializeField]
    public float RotationSpeed
    { get; private set; }

    /// <summary>
    /// The limb end points for purpose of body height calculation.
    /// </summary>
    [field: SerializeField]
    public List<GameObject> LimbsEndPoints
    { get; private set; }

    private void Awake()
    {
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
        VerticalInput = Input.GetAxisRaw("Vertical");
        HorizontalInput = Input.GetAxisRaw("Horizontal");

        // Forward and back movement.
        gameObject.transform.position += Time.deltaTime * MovementSpeed * VerticalInput * gameObject.transform.forward;

        // Rotational movement.
        gameObject.transform.rotation *= Quaternion.Euler(Time.deltaTime * RotationSpeed * HorizontalInput * gameObject.transform.up);

        // Set the body to the average of the leg positions plus offset.
        transform.position = new Vector3(transform.position.x, CalulateAveragePosition(LimbsEndPoints).y + BodyOffset.y, transform.position.z);
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
}
