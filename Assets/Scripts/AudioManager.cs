// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioClip button_click, divine_platform, evil_detect, evil_dialogue,
    sape_strum, stone_pushing, grandpa_cough, grandpa_talk1, grandpa_talk2, supernova;

    static AudioSource audioSource;
    public static bool isMuted;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        isMuted = false;
        InitializeSource();
    }

    void InitializeSource()
    {
        button_click = Resources.Load<AudioClip>("Audios/button_click");
        divine_platform = Resources.Load<AudioClip>("Audios/divine_platform");
        evil_detect = Resources.Load<AudioClip>("Audios/evil_detect");
        evil_dialogue = Resources.Load<AudioClip>("Audios/evil_dialogue");
        sape_strum = Resources.Load<AudioClip>("Audios/sape_strum");
        // stone_pushing = Resources.Load<AudioClip>("Audios/stone_pushing");
        grandpa_cough = Resources.Load<AudioClip>("Audios/grandpa_cough");
        grandpa_talk1 = Resources.Load<AudioClip>("Audios/grandpa_talk1");
        grandpa_talk2 = Resources.Load<AudioClip>("Audios/grandpa_talk2");
        supernova = Resources.Load<AudioClip>("Audios/supernova_effect");
    }

    public static void PlaySound(string clip)
    {
        if (isMuted)
        {
            return;
        }

        switch (clip)
        {
            case "button_click":
                audioSource.PlayOneShot(button_click);
                break;
            case "divine_platform":
                audioSource.PlayOneShot(divine_platform);
                break;
            case "evil_detect":
                audioSource.PlayOneShot(evil_detect);
                break;
            case "evil_dialogue":
                audioSource.PlayOneShot(evil_dialogue);
                break;
            case "sape_strum":
                audioSource.PlayOneShot(sape_strum);
                break;
            case "stone_pushing":
                audioSource.Play();
                break;
            case "grandpa_cough":
                audioSource.PlayOneShot(grandpa_cough);
                break;
            case "grandpa_talk":
                int index = Random.Range(1, 3);
                GrandpaTalking(index);
                break;
            case "supernova":
                audioSource.PlayOneShot(supernova);
                break;
        }
    }

    static void GrandpaTalking(int index)
    {
        switch (index)
        {
            case 1:
                audioSource.PlayOneShot(grandpa_talk1);
                break;
            case 2:
                audioSource.PlayOneShot(grandpa_talk2);
                break;
        }
    }

    public static void StopPushingSound()
    {
        audioSource.Stop();
    }
}
