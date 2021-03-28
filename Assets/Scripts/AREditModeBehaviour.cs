// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AREditModeBehaviour : MonoBehaviour
{
    [SerializeField]
    ARSession arSession;

    public GraphicRaycaster raycaster;
    public Transform homePivot;

    Transform arPlaneIndicator;
    ARPlaneManager arPlaneManager;
    ARSessionOrigin arSessionOrigin;
    MaskingBehaviour arMask_script;
    Transform player;

    // For AR Camera scale settings
    float minScaleValue = 1.0f;
    float maxScaleValue = 20.0f;
    float scaleSpeed = 0.01f;
    float scaleValue;

    bool isResetReady;
    bool isStartGame;
    public static bool isAREditMode = true;



    void Awake()
    {
        arPlaneManager = Camera.main.transform.GetComponentInParent<ARPlaneManager>();
        arSessionOrigin = Camera.main.transform.GetComponentInParent<ARSessionOrigin>();
        arPlaneIndicator = transform.GetChild(0);
        player = GameObject.FindWithTag("Player").transform;
        arMask_script = Camera.main.transform.root.GetChild(1).GetComponent<MaskingBehaviour>();
        scaleValue = 10.0f;
        isStartGame = false;
    }

    void Start()
    {
        isResetReady = true;
        VisualizingPlanes(true);
        arMask_script.EmptyArea();
        arSessionOrigin.MakeContentAppearAt(homePivot, arPlaneIndicator.position * 50);
    }

    void Update()
    {
        if (isAREditMode && !IsClickOverUI())
        {
            AnchorInputDetector();
            ARCameraScaler();
        }
    }

    void AnchorInputDetector()
    {
        // if (Input.GetButtonDown("Fire1"))
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (isStartGame)
            {
                // Makes 'content' appear to be placed at 'position'
                arSessionOrigin.MakeContentAppearAt(player, arPlaneIndicator.position, arPlaneIndicator.rotation);
            }
            else
            {
                arSessionOrigin.MakeContentAppearAt(homePivot, arPlaneIndicator.position, arPlaneIndicator.rotation);
            }

            arMask_script.EmptyArea();
        }
    }

    public void StartGame(bool active)
    {
        isStartGame = active;
    }

    bool IsClickOverUI()
    {
        PointerEventData data = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(data, results);

        return results.Count > 0;
    }

    void ARCameraScaler()
    {
        if (Input.touchCount == 2)
        {
            // Get the current touch positions
            Touch zero = Input.GetTouch(0);
            Touch one = Input.GetTouch(1);
            // Get the previous touch positions
            Vector2 zeroPrevious = zero.position - zero.deltaPosition;
            Vector2 onePrevious = one.position - one.deltaPosition;
            // Calculate both touch distances
            float currentTouchDistance = Vector2.Distance(zero.position, one.position);
            float previousTouchDistance = Vector2.Distance(zeroPrevious, onePrevious);
            // Get the offset distance value
            float offsetDistance = currentTouchDistance - previousTouchDistance;
            // AR Camera scaling function
            scaleValue -= offsetDistance * scaleSpeed;
            scaleValue = Mathf.Clamp(scaleValue, minScaleValue, maxScaleValue);
            arSessionOrigin.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        }
    }

    public void VisualizingPlanes(bool active)
    {
        foreach (ARPlane plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(active);
        }

        arPlaneManager.enabled = active;
        GetComponentInChildren<MeshRenderer>().enabled = active;
        GetComponentInChildren<ARPlaneIndicatorBehaviour>().enabled = active;
    }

    public void Button_ResetAREdit()
    {
        if (isResetReady)
        {
            StartCoroutine("Resetting");
            AudioManager.PlaySound("button_click");
        }
    } 

    IEnumerator Resetting()
    {
        isResetReady = false;
        VisualizingPlanes(false);
        arSessionOrigin.MakeContentAppearAt(player, arPlaneIndicator.position * 50);
        yield return null;

        arSession.Reset();
        yield return null;

        VisualizingPlanes(true);
        yield return null;

        isResetReady = true;
    }
}
