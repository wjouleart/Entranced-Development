// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using UnityEngine;
using UnityEngine.UI;

public class SettingsBehaviour : MonoBehaviour
{
    // int current_scene;
    Animator animator;
    AREditModeBehaviour arEdit_script;
    DialogueManager dialogue_script;
    Toggle mute_toggle;

    public AudioSource bgm_audio, wave_audio;


    void Awake()
    {
        animator = GetComponent<Animator>();
        arEdit_script = GameObject.FindWithTag("AREditMode").GetComponent<AREditModeBehaviour>();
        dialogue_script = GameObject.FindWithTag("Dialogue").GetComponent<DialogueManager>();
        mute_toggle = transform.GetChild(3).GetChild(3).GetComponent<Toggle>();
        animator.SetTrigger("First");
        Invoke("ResetFirst", 0.1f);
    }

    void ResetFirst()
    {
        animator.ResetTrigger("First");
    }


    public void ToSettingsScene()
    {
        animator.SetTrigger("ToSettings");
        AudioManager.PlaySound("button_click");
    }

    // public void ToSkillScene()
    // {
    //     // current_scene = !IsInHomeScene() ? 2 : 99;
    // }

    public void ToHomeScene()
    {
        if (!dialogue_script.CheckIsInDialogue())
        {
            animator.SetTrigger("ToHome");
            AudioManager.PlaySound("button_click");
            ARCameraFollowBehaviour arCamera_script = Camera.main.transform.root.GetComponent<ARCameraFollowBehaviour>();
            arCamera_script.TheOnlyWayToGame(false);
            arCamera_script.TestTestTest(false);
        }
    }

    public void ToAREditScene()
    {
        if (!dialogue_script.CheckIsInDialogue())
        {
            animator.SetTrigger("ToAREdit");
            AudioManager.PlaySound("button_click");
            Invoke("DelayedVisualizingPlane", 1.0f);
            AREditModeBehaviour.isAREditMode = true;
        }
    }

    void DelayedVisualizingPlane()
    {
        arEdit_script.VisualizingPlanes(true);
    }

    public void ExitAREditScene()
    {
        animator.SetTrigger("ExitAREdit");
        AudioManager.PlaySound("button_click");
        arEdit_script.VisualizingPlanes(false);
        AREditModeBehaviour.isAREditMode = false;
    }

    public void ToBackScene()
    {
        animator.SetTrigger("GoBack");
        AudioManager.PlaySound("button_click");
    }

    public void Toggle_MuteSound()
    {
        AudioManager.isMuted = mute_toggle.isOn;
        bgm_audio.mute = mute_toggle.isOn;
        wave_audio.mute = mute_toggle.isOn;

        AudioManager.PlaySound("button_click");
    }

    public void ToGameStage()
    {
        animator.SetTrigger("ToGame");
    }

}
