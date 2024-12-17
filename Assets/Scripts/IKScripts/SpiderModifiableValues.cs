using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderModifiableValues : MonoBehaviour
{
    /// <summary>
    /// The distance required between the leg end effector and predicted target positions before the feet move.
    /// </summary>
    [field: SerializeField]
    public float StrideLength
    { get; set; } = 1.0f;

    /// <summary>
    /// Time taken to lerp legs position to new position.
    /// </summary>
    [field: SerializeField]
    public float TimeToMoveLeg
    { get; set; } = 0.2f;

    /// <summary>
    /// The offset of the body the average leg position.
    /// </summary>
    [field: SerializeField]
    public Vector3 BodyOffset
    { get; set; } = new Vector3(0, 1.0f, 1);

    /// <summary>
    /// Speed of the translate of the spider body.
    /// </summary>
    [field: SerializeField]
    public float MovementSpeed
    { get; set; } = 5.0f;

    /// <summary>
    /// Speed of the rotation of the spider body.
    /// </summary>
    [field: SerializeField]
    public float RotationSpeed
    { get; set; } = 25.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
