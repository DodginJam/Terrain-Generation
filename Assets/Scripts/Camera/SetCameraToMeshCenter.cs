using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraToMeshCenter : MonoBehaviour
{
    public GameObject CenterMesh
    { get; private set; }

    public Vector3 CameraOffset
    { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCameraToMesh(MeshRenderer mesh)
    {
        Vector3 terrainMeshCoords = mesh.bounds.center;

        gameObject.transform.position = terrainMeshCoords + CameraOffset;
    }
}
