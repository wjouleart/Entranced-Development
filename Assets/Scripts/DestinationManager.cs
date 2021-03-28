// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using System.Collections;
using UnityEngine;

public class DestinationManager : MonoBehaviour
{
    bool isVisible;
    Transform[] destinations;
    EvilBehaviour[] evil_scripts;

    Transform indicator;
    Animator visual_animator;
    Vector3 rotate_rate;
    Animator banner_animator;

    TriggerManager triggerManager_script;
    ARCameraFollowBehaviour arCamera_script;
    PathManager pathManager_script;

    void Awake()
    {
        isVisible = false;
        rotate_rate = new Vector3(0.0f, 60.0f, 0.0f);
        indicator = transform.GetChild(transform.childCount - 1);
        visual_animator = indicator.GetChild(0).GetComponent<Animator>();
        triggerManager_script = transform.parent.GetChild(0).GetComponent<TriggerManager>();
        arCamera_script = Camera.main.GetComponentInParent<ARCameraFollowBehaviour>();
        pathManager_script = transform.parent.GetChild(2).GetComponent<PathManager>();
        banner_animator = GameObject.FindWithTag("Banner").GetComponent<Animator>();

        destinations = new Transform[transform.childCount - 1];
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            destinations[i] = transform.GetChild(i);
        }

        GameObject[] evils = GameObject.FindGameObjectsWithTag("Evil");
        evil_scripts = new EvilBehaviour[evils.Length];
        for (int i = 0; i < evils.Length; i++)
        {
            evil_scripts[i] = evils[i].GetComponent<EvilBehaviour>();
        }
    }

    void Update()
    {
        if (isVisible)
            indicator.Rotate(rotate_rate * Time.deltaTime);
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Trigger")
        {
            StartCoroutine("ReachedDestination");
        }
    }

    IEnumerator ReachedDestination()
    {
        banner_animator.ResetTrigger("HideBanner");
        triggerManager_script.StartCoroutine("StopPushingSelection", false);
        yield return null;

        RestrainAllEvilMovement();
        triggerManager_script.DisableTheSelection();
        yield return new WaitForSeconds(0.85f);

        triggerManager_script.RestrainPlayerMovement();
        yield return new WaitForSeconds(1.0f);

        banner_animator.SetTrigger("ShowBanner");
        arCamera_script.CutSceneToRoad();
        yield return new WaitForSeconds(4.0f);

        PathManager.PathBuilder();
        yield return new WaitForSeconds(5.3f);

        arCamera_script.CutSceneToPlayer();
        yield return new WaitForSeconds(6.0f);

        arCamera_script.StopCutScene();
        banner_animator.SetTrigger("HideBanner");
        EnableAllEvilMovement();
        triggerManager_script.EnablePlayerMovement();
    }

    public void SetDestinationIndicator(int current_index)
    {
        indicator.position = destinations[current_index].position;
    }

    public IEnumerator ShowDestinationIndicator()
    {
        isVisible = true;
        visual_animator.ResetTrigger("HideIndicator");
        visual_animator.SetTrigger("ShowIndicator");
        yield return new WaitForSeconds(3.5f);

        visual_animator.SetTrigger("HideIndicator");
        yield return new WaitForSeconds(0.7f);
        isVisible = false;
    }

    void RestrainAllEvilMovement()
    {
        foreach (EvilBehaviour evil_script in evil_scripts)
            evil_script.RestrainEvilMovement();
    }

    void EnableAllEvilMovement()
    {
        foreach (EvilBehaviour evil_script in evil_scripts)
            evil_script.EnableEvilMovement();
    }
}
