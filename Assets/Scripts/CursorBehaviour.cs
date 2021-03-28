// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorBehaviour : MonoBehaviour
{
    Ray ray;
    int layer_ground, layer_button, layer_home;
    Camera arCamera;

    Transform player;
    PlayerBehaviour player_script;
    Vector3 cursor_planar;

    public GraphicRaycaster raycaster;
    public TriggerManager trigger_manager;
    public SettingsBehaviour settings_script;

    void Awake()
    {
        arCamera = Camera.main;
        player = GameObject.FindWithTag("Player").transform;
        player_script = player.GetComponent<PlayerBehaviour>();
        // Raycast layermasks
        layer_ground = LayerMask.NameToLayer("Ground");
        layer_button = LayerMask.NameToLayer("Button");
        layer_home = LayerMask.NameToLayer("Home");
    }

    void Update()
    {
        if (!IsClickOverUI())
        {
            TouchInput_DragEnabled();
            // TouchInput_NoDrag();
        }
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

    void TouchInput_NoDrag()
    {
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            ray = arCamera.ScreenPointToRay(touch.position);

            if (Physics.Raycast(ray, out RaycastHit rayHit))
            {
                if (rayHit.transform.gameObject.layer == layer_button)
                {
                    InteractButton();
                    AudioManager.PlaySound("button_click");
                }
                else if (rayHit.transform.gameObject.layer == layer_home)
                {
                    InteractHomeButton(rayHit);
                    AudioManager.PlaySound("button_click");
                }
                else if (rayHit.transform.gameObject.layer == layer_ground)
                {
                    PlaceCursor(rayHit);
                }
            }
        }
    }


    void TouchInput_DragEnabled()
    {
        if (Input.touchSupported)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                ray = arCamera.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit rayHit))
                {
                    if (rayHit.transform.gameObject.layer == layer_button)
                    {
                        InteractButton();
                        AudioManager.PlaySound("button_click");
                    }
                    else if (rayHit.transform.gameObject.layer == layer_home)
                    {
                        InteractHomeButton(rayHit);
                        AudioManager.PlaySound("button_click");
                    }
                    else if (rayHit.transform.gameObject.layer == layer_ground)
                    {
                        PlaceCursor(rayHit);
                    }
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ray = arCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit rayHit))
                {
                    if (rayHit.transform.gameObject.layer == layer_button)
                    {
                        InteractButton();
                        AudioManager.PlaySound("button_click");
                    }
                    else if (rayHit.transform.gameObject.layer == layer_home)
                    {
                        InteractHomeButton(rayHit);
                        AudioManager.PlaySound("button_click");
                    }
                    else if (rayHit.transform.gameObject.layer == layer_ground)
                    {
                        PlaceCursor(rayHit);
                    }
                }
            }
        }
    }

    void InteractHomeButton(RaycastHit rayHit)
    {
        if (AREditModeBehaviour.isAREditMode)
        {
            return;
        }

        switch (rayHit.transform.gameObject.name)
        {
            case "Play Button":
                StartCoroutine("PlayButtonTest");
                settings_script.ToGameStage();
                break;

            case "Settings Button":
                break;

            case "Exit Button":
                Application.Quit();
                break;
        }
    }

    IEnumerator PlayButtonTest()
    {
        ARCameraFollowBehaviour arCamera_script = arCamera.transform.root.GetComponent<ARCameraFollowBehaviour>();
        arCamera_script.TheOnlyWayToGame(true);
        yield return new WaitForSeconds(3.0f);
        arCamera_script.TestTestTest(true);
    }


    void InteractButton()
    {
        if (!trigger_manager.isPlayerPushing)
        {
            trigger_manager.StartPushingSelection();
        }
        else
        {
            trigger_manager.StartCoroutine("StopPushingSelection", false);
        }
    }

    void PlaceCursor(RaycastHit rayHit)
    {
        cursor_planar = new Vector3(rayHit.point.x, player.position.y, rayHit.point.z);

        if (IsInDistance(4.0f))
        {
            transform.position = rayHit.point;

            if (trigger_manager.isPlayerPushing && !IsInDistance(0.3f))
            {
                trigger_manager.thePusher.LookAt(cursor_planar);
            }
        }
    }

    bool IsInDistance(float dist)
    {
        return Vector3.Distance(player.position, cursor_planar) < dist;
    }
}
