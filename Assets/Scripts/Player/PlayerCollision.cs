using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftMarks : MonoBehaviour
{
    PlayerController playerController;
    TrailRenderer trailRenderer;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();

        trailRenderer = GetComponent<TrailRenderer>();

        trailRenderer.emitting = false;
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        if (playerController.IsTireScreeching(out float lateralVelocity, out bool isDrifting))
            trailRenderer.emitting = true;
        else
            trailRenderer.emitting = false;
    }
}
