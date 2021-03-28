// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using UnityEngine;

public class DivinePlatformBehaviour : MonoBehaviour
{
    MaskingBehaviour arMask_script;
    ParticleSystem vfx;


    void Awake()
    {
        arMask_script = Camera.main.transform.root.GetChild(1).GetComponent<MaskingBehaviour>();
        vfx = GetComponentInChildren<ParticleSystem>();
    }


    public void StartObservation()
    {
        PlayVFX(true);
        arMask_script.Activate_DivinePlatform();
        AudioManager.PlaySound("divine_platform");
    }

    public void EndObservation()
    {
        PlayVFX(false);
        arMask_script.Deactivate_DivinePlatform();
    }

    void PlayVFX(bool active)
    {
        var particle_main = vfx.main;

        if (active)
        {
            particle_main.loop = true;
            vfx.Play();
        }
        else
        {
            particle_main.loop = false;
        }
    }
}
