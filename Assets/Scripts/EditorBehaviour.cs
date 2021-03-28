// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EditorBehaviour : MonoBehaviour
{
    public EvilBehaviour evil_script;
    GameObject locator;

    GameObject player;
    NavMeshAgent player_agent;
    PlayerBehaviour player_script;

    Transform cursor;



    void Awake()
    {   
        if (Application.isEditor)
        {
            locator = transform.GetChild(0).gameObject;
            player = GameObject.FindWithTag("Player");
            player_agent = player.GetComponent<NavMeshAgent>();
            player_script = player.GetComponent<PlayerBehaviour>();
            cursor = GameObject.FindWithTag("Cursor").transform;
        }
    }

    void Start()
    {
        if (Application.isEditor)
        {
            Application.targetFrameRate = 30;
            // StartCoroutine("Initialize");
            // arCamera_pos.position 
        }
        else
        {
            locator.SetActive(false);
        }
    }


    IEnumerator Initialize()
    {
        player_script.enabled = false;
        player_agent.enabled = false;
        yield return null;

        player.transform.position = locator.transform.position;
        cursor.position = locator.transform.position;
        yield return null;

        player_agent.enabled = true;
        player_script.enabled = true;
    }


    void OnGUI()
    {
        GUIStyle font = new GUIStyle(GUI.skin.GetStyle("label"));
        font.fontSize = 20;

        // GUI.Label(new Rect(10, 30, 500, 30), "Detected Player? " + evil_script.debuggerlah(), font);
    }

}