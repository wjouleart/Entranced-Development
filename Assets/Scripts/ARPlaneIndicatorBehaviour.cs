// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaneIndicatorBehaviour : MonoBehaviour
{
    ARRaycastManager arRaycastManager;
    MeshRenderer meshRenderer;

    
    void Awake()
    {
        // Assigns value for private variables
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    void Update()
    {
        // Casts a ray from the centre of the screen
        List<ARRaycastHit> hitsAR = new List<ARRaycastHit>();
        arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hitsAR, TrackableType.PlaneWithinBounds);
        
        // If it hits a surface, show visual and update its transformation
        if (hitsAR.Count > 0)
        {
            transform.position = hitsAR[0].pose.position;
            transform.rotation = hitsAR[0].pose.rotation;

            if (!meshRenderer.enabled)
                meshRenderer.enabled = true;
        }
        else
        {
            if (meshRenderer.enabled)
                meshRenderer.enabled = false;
        }
    }
}
