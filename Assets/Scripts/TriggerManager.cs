// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TriggerManager : MonoBehaviour
{
    [HideInInspector]
    public int current_index;
    [HideInInspector]
    public bool isPlayerPushing;

    public Transform thePusher;
    Transform interactButton;
    Animator interactButton_animator;
    DestinationManager destinationManager_script;

    CursorBehaviour cursor_script;
    TriggerBehaviour[] trigger_scripts;
    Animator player_animator;
    NavMeshAgent player_agent;


    void Awake()
    {
        cursor_script = GameObject.FindWithTag("Cursor").GetComponent<CursorBehaviour>();
        player_agent = GameObject.FindWithTag("Player").GetComponent<NavMeshAgent>();
        player_animator = GameObject.FindWithTag("Player").GetComponentInChildren<Animator>();
        interactButton = transform.GetChild(transform.childCount - 1);
        interactButton_animator = interactButton.GetComponent<Animator>();
        destinationManager_script = transform.parent.GetChild(1).GetComponent<DestinationManager>();

        trigger_scripts = new TriggerBehaviour[transform.childCount - 1];
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            trigger_scripts[i] = transform.GetChild(i).GetComponent<TriggerBehaviour>();
        }
    }

    void Start()
    {
        current_index = -1;
        isPlayerPushing = false;
        // Assign index for each trigger in the first island
        for (int i = 0; i < trigger_scripts.Length; i++)
        {
            trigger_scripts[i].index = i;
        }
    }

    void Update()
    {
        if (isPlayerPushing)
        {
            Vector3 rawPosition = trigger_scripts[current_index].transform.position;
            Vector3 adjustedPosition = new Vector3(rawPosition.x, rawPosition.y + 1.5f, rawPosition.z + 0.12f);
            interactButton.position = adjustedPosition;
        }
    }

    public void ShowInteractionButton(Transform trigger)
    {
        Vector3 adjustedPosition = new Vector3(trigger.position.x, trigger.position.y + 1.5f, trigger.position.z + 0.12f);
        interactButton.position = adjustedPosition;
        interactButton_animator.ResetTrigger("CancelButton");
        interactButton_animator.SetTrigger("ShowButton");
    }

    public void HideInteractionButton()
    {
        interactButton_animator.SetTrigger("CancelButton");
    }

    public void SetDestinationIndicator()
    {
        destinationManager_script.SetDestinationIndicator(current_index);
    }

    public void StartPushingSelection()
    {
        player_agent.speed = 1.8f;
        player_agent.updateRotation = false;
        isPlayerPushing = true;
        player_animator.SetTrigger("Push");
        interactButton_animator.SetTrigger("CrossButton");
        trigger_scripts[current_index].StartPushing();
        AudioManager.PlaySound("stone_pushing");
    }

    public IEnumerator StopPushingSelection(bool isCaughtByEvil)
    {
        if (!isCaughtByEvil)
            player_animator.SetTrigger("StopPush");

        isPlayerPushing = false;
        interactButton_animator.SetTrigger("ExitButton");
        trigger_scripts[current_index].StopPushing();
        AudioManager.StopPushingSound();
        yield return new WaitForSeconds(0.8f);

        player_agent.updateRotation = true;
        player_agent.speed = 1.0f;
    }


    public void Initialize_ThePusher(Transform player)
    {
        thePusher.position = player.position;
        thePusher.rotation = player.rotation;
    }

    public void DisableTheSelection()
    {
        TriggerBehaviour selected_script = trigger_scripts[current_index];

        selected_script.enabled = false;
        selected_script.GetComponent<Collider>().enabled = false;
    }

    public void RestrainPlayerMovement()
    {
        ResetCursorPosition();
        player_agent.speed = 0;
    }

    public void EnablePlayerMovement()
    {
        ResetCursorPosition();
        player_agent.speed = 1.0f;
    }

    void ResetCursorPosition()
    {
        Transform cursor = cursor_script.transform;
        Transform player = player_agent.transform;
        cursor.position = player.position;
    }
}
