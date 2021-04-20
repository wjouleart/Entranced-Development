// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class EvilBehaviour : MonoBehaviour
{
    GameObject player;
    NavMeshAgent player_agent;
    NavMeshAgent agent;
    public static GameObject eye_indicator;
    public static bool detected_player = false;
    bool isBeingRestrained;

    [HideInInspector]
    public bool caught_player;

    Transform location_a;
    Transform location_b;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        player_agent = player.GetComponent<NavMeshAgent>();
        location_a = transform.parent.GetChild(1);
        location_b = transform.parent.GetChild(2);
        eye_indicator = transform.GetChild(transform.childCount - 1).gameObject;
    }

    void OnEnable()
    {
        // Reset the detector functions
        detected_player = false;
        isBeingRestrained = false;
        caught_player = false;
        eye_indicator.SetActive(false);
        // Reset the agent radius
        agent.radius = 0.4f;
        // Reset this evil position to default
        transform.position = location_a.position;
        StartCoroutine("Patrolling");
    }

    void Update()
    {
        if (VisionCone.detected_player && !isBeingRestrained)
        {
            agent.SetDestination(player.transform.position);

            if (Vector3.Distance(transform.position, player.transform.position) < 0.65f)
            {
                CaughtPlayer();
            }
        }
    }

    public void RestrainEvilMovement()
    {
        isBeingRestrained = true;
        agent.isStopped = true;
    }

    public void EnableEvilMovement()
    {
        isBeingRestrained = false;
        agent.isStopped = false;
    }


    void CaughtPlayer()
    {
        TriggerManager triggerManager = GetTriggerManager();
        if (CheckIfPlayerIsPushing(triggerManager))
            triggerManager.StartCoroutine("StopPushingSelection", true);

        StopAllCoroutines();
        PlayerBeingCaughtAnimation();
        player_agent.radius = 0.1f;
        player_agent.isStopped = true;
        player.GetComponent<PlayerBehaviour>().isDead = true;
        caught_player = true;
    }

    void PlayerBeingCaughtAnimation()
    {
        GameObject.FindWithTag("Player").GetComponentInChildren<Animator>().SetTrigger("BeingCaught");
    }

    TriggerManager GetTriggerManager()
    {
        return GameObject.FindWithTag("Trigger").GetComponentInParent<TriggerManager>();
    }

    bool CheckIfPlayerIsPushing(TriggerManager script)
    {
        return script.isPlayerPushing;
    }

    IEnumerator Patrolling()
    {
        while (true)
        {
            agent.SetDestination(location_a.position);
            yield return null;

            yield return new WaitUntil(() => agent.remainingDistance < 0.5f);
            yield return new WaitForSeconds(6.0f);

            agent.SetDestination(location_b.position);
            yield return null;

            yield return new WaitUntil(() => agent.remainingDistance < 0.5f);
            yield return new WaitForSeconds(6.0f);
        }
    }


    public void Stunned()
    {
        StartCoroutine("Stunning");
    }

    IEnumerator Stunning()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(1.5f);
        agent.isStopped = false;
    }


    /*void OnTriggerEnter(Collider other)
    {
        if (!detected_player && other.gameObject.tag == "Player")
        {
            detected_player = true;
            eye_indicator.SetActive(true);
            AudioManager.PlaySound("evil_detect");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (detected_player && other.gameObject.tag == "Player")
        {
            detected_player = false;
            eye_indicator.SetActive(false);
        }
    }*/
}
