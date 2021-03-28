// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using UnityEngine;
using UnityEngine.AI;

public class RespawnBehaviour : MonoBehaviour
{
    // Respawn evil spirits
    EvilBehaviour[] evil_scripts;
    Animator[] evil_animators;
    DialogueManager dialogue_script;

    // Respawn player 
    Transform player;
    NavMeshAgent player_agent;
    Transform cursor;
    public Transform location_a;
    public Transform location_b;
    public Transform location_c;
    public Transform pointLv2;

    public static int current_spawn_point = 1;


    void Awake()
    {
        GameObject[] evils = GameObject.FindGameObjectsWithTag("Evil");
        evil_scripts = new EvilBehaviour[evils.Length];
        evil_animators = new Animator[evils.Length];

        for (int i = 0; i < evils.Length; i++)
        {
            evil_scripts[i] = evils[i].GetComponent<EvilBehaviour>();
            evil_animators[i] = evils[i].GetComponent<Animator>();
        }

        player = GameObject.FindWithTag("Player").transform;
        player_agent = player.gameObject.GetComponent<NavMeshAgent>();
        cursor = GameObject.FindWithTag("Cursor").transform;
        dialogue_script = GameObject.FindWithTag("Dialogue").GetComponent<DialogueManager>();

        // For debugging
        Debug.Log("Evil Object Length: " + evils.Length.ToString());
        Debug.Log("Evil Script Length: " + evil_scripts.Length.ToString());
        Debug.Log("Evil Animator Length: " + evil_animators.Length.ToString());
    }

    public void RespawnEvent()
    {
        RespawningEvil();
        RespawningPlayer();
    }

    public void RecoverEvent()
    {
        player_agent.enabled = true;
        player.gameObject.GetComponent<PlayerBehaviour>().isDead = false;
    }

    void Event_StartDialogue()
    {
        dialogue_script.StartDialogue("Respawn");
    }

    void RespawningEvil()
    {
        for (int i = 0; i < evil_scripts.Length; i++)
        {
            // Reset the evil animation & Re-enable its script
            if (evil_animators[i].GetCurrentAnimatorStateInfo(0).IsName("catch"))
            {
                evil_animators[i].SetTrigger("Respawn");
                evil_scripts[i].enabled = true;
            }
        }
    }

    public void RespawningPlayer()
    {
        player_agent.enabled = false;
        player.GetComponentInChildren<Animator>().SetTrigger("Respawn");
        
        Vector3 final_respawn_position = CheckPosition();
        cursor.position = final_respawn_position;
        player.position = final_respawn_position;
    }

    Vector3 CheckPosition()
    {
        if (current_spawn_point == 1)
        {
            return location_a.position;
        }
        else if (current_spawn_point == 2)
        {
            return location_b.position;
        }
        else if (current_spawn_point == 4)
        {
            return pointLv2.position;
        }
        else
        {
            return location_c.position;
        }
    }
}
