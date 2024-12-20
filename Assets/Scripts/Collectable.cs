using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    public bool BeenCollected
    { get; private set; } = false;

    public float CountdownTimer
    { get; private set; } = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !BeenCollected)
        {
            BeenCollected = true;
            Destroy(gameObject, CountdownTimer);
        }
    }
}
