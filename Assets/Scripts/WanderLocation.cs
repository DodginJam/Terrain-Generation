using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderLocation : MonoBehaviour
{
    [field: SerializeField]
    public SpiderModifiableValues SpiderModifiableValues
    { get; private set; }

        // For use of NPC varient.
    [field: SerializeField,Header("Wander Settings")]
    public Vector3 Center
    { get; private set; } = Vector3.zero;

    [field: SerializeField]
    public Vector3 AreaSize
    { get; private set; } = new Vector3(10, 0, 10); // The size of the area (X and Z dimensions)

    [field: SerializeField]
    public float ChangeDirectionTime
    { get; private set; } = 3f;

    [field: SerializeField]
    public Vector3 TargetPosition
    { get; private set; }

    [field: SerializeField]
    public float ChangeDirectionTimer 
    { get; private set; } // Timer to track when to change direction

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.parent.CompareTag("NPC"))
        {
            // Move towards the target position
            transform.position = Vector3.MoveTowards(gameObject.transform.position, TargetPosition, SpiderModifiableValues.MovementSpeed * Time.deltaTime);

            // Check if reached the target position
            if (Vector3.Distance(transform.position, TargetPosition) < 0.1f || ChangeDirectionTimer <= 0)
            {
                // Get a random position within the defined area
                float randomX = Random.Range(Center.x - AreaSize.x / 2, Center.x + AreaSize.x / 2);
                float randomZ = Random.Range(Center.z - AreaSize.z / 2, Center.z + AreaSize.z / 2);

                TargetPosition = new Vector3(randomX, transform.position.y, randomZ);
                ChangeDirectionTimer = ChangeDirectionTimer; // Reset the timer
            }

            ChangeDirectionTimer -= Time.deltaTime;
        }
    }
}
