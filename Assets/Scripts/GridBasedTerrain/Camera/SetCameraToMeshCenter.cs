using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraToMeshCenter : MonoBehaviour
{
    public GameObject CenterMesh
    { get; private set; }

    [field: SerializeField]
    public Vector3 CameraOffset
    { get; private set; }

    [field: SerializeField]
    public Vector3 StartPosition
    { get; private set; }

    [field: SerializeField]
    public Quaternion StartRotation
    { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CenterMesh = GameObject.Find("Terrain0");

        MeshRenderer mesh = CenterMesh.GetComponent<MeshRenderer>();

        CameraOffset = new Vector3(0, mesh.bounds.size.x, mesh.bounds.size.z / 2);

        SetCameraToMesh(mesh);

        StartPosition = transform.position;
        StartRotation = transform.rotation;
    }

    public void SetCameraToMesh(MeshRenderer mesh)
    {
        Vector3 terrainMeshCoords = mesh.bounds.center;

        transform.position = terrainMeshCoords + CameraOffset;
        transform.LookAt(terrainMeshCoords);
    }
}
