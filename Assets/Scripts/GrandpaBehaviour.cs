// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using System.Collections;
using UnityEngine;

public class GrandpaBehaviour : MonoBehaviour
{
    public GameObject transporter;
    public RespawnBehaviour respawn_script;
    public GameObject skip_cheat;
    int skip_amount = 0;

    void Awake()
    {
        transporter.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transporter.SetActive(true);
        }
    }

    public void Button_ToFirstCorner()
    {
        RespawnBehaviour.current_spawn_point = 1;
        StartCoroutine("SettingPosition");
    }

    public void Button_ToSecondCorner()
    {
        RespawnBehaviour.current_spawn_point = 2;
        StartCoroutine("SettingPosition");
    }

    public void Button_ToThirdCorner()
    {
        RespawnBehaviour.current_spawn_point = 3;
        StartCoroutine("SettingPosition");
    }

    IEnumerator SettingPosition()
    {
        transporter.SetActive(false);
        respawn_script.RespawningPlayer();
        yield return new WaitForSeconds(0.1f);

        respawn_script.RecoverEvent();
    }

    public void SkipCheat()
    {
        skip_amount += 1;

        if (skip_amount == 17)
        {
            skip_cheat.SetActive(true);
            transporter.SetActive(false);
        }
    }
    
}
