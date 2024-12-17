using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SetPositionToRaycast : MonoBehaviour
{
    [field: SerializeField]
    public float LegStrideLengthModifier
    { get; private set; } = 1.0f;

    [field: SerializeField]
    public float TimeToMoveLegModifier
    { get; private set; } = 1.0f;

    /// <summary>
    /// The transform component for the predicted target gameobject for the current limb.
    /// </summary>
    [field: SerializeField]
    public Transform TargetPredicted
    { get; private set; }

    /// <summary>
    /// The transform component for the end effector of the current limb.
    /// </summary>
    [field: SerializeField]
    public Transform EndEffectorTarget
    { get; private set; }

    /// <summary>
    /// Tracks if the current limb is being lerp to new position - prevents multiple interpolitions coroutines being ran.
    /// </summary>
    [field: SerializeField]
    public bool LegMoving 
    { get; private set; } = false;

    /// <summary>
    /// The lerp curve to provide upwards Y movement to steps - currently disabled due to glitch with uneven terrain.
    /// </summary>
    [field: SerializeField]
    public AnimationCurve LerpCurve
    { get; private set; }

    /// <summary>
    /// Reference to leg positional raycast script on opposite leg - enables check if it is moving.
    /// </summary>
    [field: SerializeField]
    public SetPositionToRaycast PairedLeg
    { get; private set; }

    [field: SerializeField]
    public GameObject RayCastEmitter
    { get; private set; }

    [field: SerializeField]
    public SpiderModifiableValues SpiderModifiableValues
    { get; private set; }

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        SetTargetToPredictedTarget();

        EndEffectorTarget.position = TargetPredicted.position;
    }

    // Update is called once per frame
    void Update()
    {
        SetTargetToPredictedTarget();

        float targetToPredictedStepDistance = Vector3.Distance(EndEffectorTarget.position, TargetPredicted.position);

        // Only start moving the leg if it is not moving, it's paired leg is not moving, and the distance from current leg position to predicted position is greater then given value.
        if (targetToPredictedStepDistance > (SpiderModifiableValues.StrideLength * LegStrideLengthModifier) && LegMoving == false && PairedLeg.LegMoving == false)
        {
            // Start the movement lerp of the current end effector target to moves position towards the target predicted position.
            StartCoroutine(TransitionLegs(SpiderModifiableValues.TimeToMoveLeg * TimeToMoveLegModifier));
        }
    }

    /// <summary>
    /// Lerps between the start
    /// </summary>
    /// <returns></returns>
    public IEnumerator TransitionLegs(float timeToMoveLeg)
    {
        LegMoving = true;

        Vector3 startPosition = EndEffectorTarget.position;
        Vector3 endPosition = TargetPredicted.position;

        float timeElasped = 0.0f;

        while (timeElasped < timeToMoveLeg)
        {
            // This line will keep the end position moving to the current position of the preditcted target position - moving goal post
            endPosition = TargetPredicted.position;

            float currentTime = timeElasped / timeToMoveLeg;

            Vector3 currentValue = Vector3.Lerp(startPosition, endPosition, currentTime);

            //currentValue = new Vector3(currentValue.x, LerpCurve.Evaluate(currentTime * currentValue.y * 500), currentValue.z);

            EndEffectorTarget.position = currentValue;

            timeElasped += Time.deltaTime;

            if (currentTime > 0.99f)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        LegMoving = false;
    }

    /// <summary>
    /// Set's the target predicted position to the new ray hit position.
    /// </summary>
    public void SetTargetToPredictedTarget()
    {
        // This first raycaset is looking stright down from the gameobject emitter.
        RaycastHit hit;

        bool hitFound = Physics.Raycast(RayCastEmitter.transform.position, -transform.up, out hit);
        Debug.DrawRay(RayCastEmitter.transform.position, -transform.up);


        // This second raycaset is looking forward and down from the gameobject emitter, a diagonal line.
        RaycastHit hit2;

        bool hitFound2 = Physics.Raycast(RayCastEmitter.transform.position, -RayCastEmitter.transform.up + RayCastEmitter.transform.forward, out hit2, 1.5f);
        Debug.DrawRay(RayCastEmitter.transform.position, -RayCastEmitter.transform.up + RayCastEmitter.transform.forward * 1.5f, Color.red);

        // If the shorter length, second raycast hit is successful, allow that information to pass to predicted target position, else the downwards raycast is passed.
        if (hitFound2)
        {
            TargetPredicted.position = new Vector3(hit2.point.x, hit2.point.y, hit2.point.z);
        }
        else
        {
            TargetPredicted.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
    }
}
