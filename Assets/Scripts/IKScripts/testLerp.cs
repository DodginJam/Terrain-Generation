using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testLerp : MonoBehaviour
{
    Vector3 StartPosition;

    Vector3 EndPosition;

    Vector3 DistanceToTransition = new Vector3(5.0f, 0.0f, 0);

    float TimeToTransition = 1.0f;

    float currentTimer = 0.0f;

    [field: SerializeField]
    public AnimationCurve LerpCurve
    { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        StartPosition = transform.position;
        EndPosition = transform.position + DistanceToTransition;
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer += Time.deltaTime;

        if (currentTimer >= TimeToTransition)
        {
            currentTimer = 0.0f;
        }
        else
        {

            Vector3 newPosition = Vector3.Lerp(StartPosition, EndPosition, currentTimer);

            newPosition = new Vector3(newPosition.x, LerpCurve.Evaluate(currentTimer * newPosition.y), newPosition.z);

            transform.position = newPosition;
        }
    }
}
