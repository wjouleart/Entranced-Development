// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using System.Collections;
using UnityEngine;

public class SoundwaveBehaviour : MonoBehaviour
{
    [HideInInspector]
    public bool isRecharged;

    Transform visual;
    Animator visual_animator;
    Transform player;
    EvilBehaviour evil_script;

    public DestinationManager destinationManager_script;


    void Awake()
    {
        visual = transform.GetChild(0);
        visual_animator = visual.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        evil_script = null;
    }

    void Start()
    {
        isRecharged = true;
    }

    public void EmitSoundwave()
    {
        if (isRecharged)
        {
            StartCoroutine("Emitting");
            AudioManager.PlaySound("sape_strum");
        }
    }

    IEnumerator Emitting()
    {
        isRecharged = false;
        transform.position = player.position;
        transform.rotation = player.rotation;
        yield return null;

        visual_animator.SetTrigger("VFX");
        yield return new WaitForSeconds(4.0f);
        transform.position -= new Vector3(0, 20, 0);

        yield return new WaitForSeconds(5.0f);
        isRecharged = true;
    }


    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Evil":
                if (evil_script != other.GetComponent<EvilBehaviour>())
                    evil_script = other.GetComponent<EvilBehaviour>();
                evil_script.Stunned();
                break;

            case "Trigger":
                if (other.GetComponentInParent<TriggerManager>().current_index >= 0)
                    destinationManager_script.StartCoroutine("ShowDestinationIndicator");
                break;

            case "DivinePlatform":
                DivinePlatformBehaviour divinePlat_script = other.GetComponent<DivinePlatformBehaviour>();
                divinePlat_script.enabled = true;
                divinePlat_script.StartObservation();
                break;
        }
    }

}
