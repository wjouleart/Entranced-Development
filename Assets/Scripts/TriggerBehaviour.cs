// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using System.Collections;
using UnityEngine;

public class TriggerBehaviour : MonoBehaviour
{
    [HideInInspector]
    public int index;

    bool isBeingPushed = false;
    bool isBeingPushedBefore = false;

    TriggerManager manager_script;
    Vector3 offset_position;
    Rigidbody body;
    Camera arCamera;
    Transform player;
    Transform cursor;
    Material material;
    Color emissionColor;

    void Awake()
    {
        arCamera = Camera.main;
        body = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").transform;
        cursor = GameObject.FindWithTag("Cursor").transform;
        manager_script = GetComponentInParent<TriggerManager>();
        material = GetComponent<Renderer>().material;
        emissionColor = material.GetColor("_EmissionColor");
    }

    void OnDisable()
    {
        material.SetColor("_EmissionColor", emissionColor * 0f);
    }

    void Update()
    {
        UpdateMaterial();

        if (isBeingPushed)
        {
            body.AddForce(manager_script.thePusher.forward);
            cursor.position = transform.position - offset_position;
        }
    }

    void UpdateMaterial()
    {
        // Glowing Material
        float glow = (Mathf.Cos(Time.time * 2.0f) + 1.5f) * 0.5f;
        material.SetColor("_EmissionColor", emissionColor * glow);
        // Masking Material
        material.SetVector("_Masking", MaskingBehaviour.maskVolume);
    }

    public void StartPushing()
    {
        offset_position = transform.position - player.position;
        player.LookAt(new Vector3(transform.position.x, player.position.y, transform.position.z));
        manager_script.Initialize_ThePusher(player);
        isBeingPushed = true;
    }

    public void StopPushing()
    {
        isBeingPushed = false;
        isBeingPushedBefore = true;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            manager_script.current_index = index;
            manager_script.ShowInteractionButton(transform);
            manager_script.SetDestinationIndicator();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isBeingPushed)
        {
            if (isBeingPushedBefore)
            {
                isBeingPushedBefore = false;
            }
            else
            {
                manager_script.HideInteractionButton();
            }
        }
    }
}
