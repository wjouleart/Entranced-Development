// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using UnityEngine;
using UnityEngine.AI;

public class PlayerBehaviour : MonoBehaviour
{
    NavMeshAgent agent;
    Transform cursor;

    bool isCompleted;

    public GameObject theEnd;

    [HideInInspector]
    public bool isDead;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        cursor = GameObject.FindWithTag("Cursor").transform;
        isDead = false;
        isCompleted = false;
    }

    void Update()
    {
        if (!isDead)
        {
            agent.SetDestination(cursor.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DivinePlatform")
        {
            if (!isCompleted)
            {
                GameObject.FindWithTag("Dialogue").GetComponent<DialogueManager>().StartDialogue("DivinePlatform");
            }
            isCompleted = true;
        }
        else if (other.gameObject.tag == "Finish")
        {
            if (PathManager.index == 6)
            {
                theEnd.SetActive(true);
            }
            else
            {
                GameObject.FindWithTag("Dialogue").GetComponent<DialogueManager>().StartDialogue("Path_Not_Finished");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "DivinePlatform")
        {
            other.GetComponent<DivinePlatformBehaviour>().EndObservation();
        }
    }
}
