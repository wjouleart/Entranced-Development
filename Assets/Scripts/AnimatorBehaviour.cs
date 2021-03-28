// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using UnityEngine;
using UnityEngine.AI;

public class AnimatorBehaviour : MonoBehaviour
{
    public enum Source { Other, Player, Evil, Grandpa };
    public Source source;

    Animator animator;
    Animator respawn_scene;
    NavMeshAgent agent;

    // Evil variables
    EvilBehaviour evil_script;



    void Awake()
    {
        animator = GetComponent<Animator>();
        respawn_scene = GameObject.FindWithTag("Respawn").GetComponent<Animator>();
        SourceConstructor();
    }

    void Update()
    {
        if (source == Source.Player)
        {
            animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
        }
        else if (source == Source.Evil)
        {
            animator.SetFloat("MoveSpeed", agent.velocity.magnitude);

            if (evil_script.caught_player)
            {
                animator.SetTrigger("Caught");
                evil_script.enabled = false; // 'OnTriggerEnter' will not turn detected_player to on
                evil_script.caught_player = false; // To trigger this function only once
                respawn_scene.SetTrigger("Respawn");
            }
            else // if ()
            {

            }
        }
    }




    void SourceConstructor()
    {
        if (source == Source.Other)
        {
            agent = null;
        }
        else if (source == Source.Player)
        {
            agent = GameObject.FindWithTag("Player").GetComponent<NavMeshAgent>();
            evil_script = null;
        }
        else if (source == Source.Evil)
        {
            agent = GetComponent<NavMeshAgent>();
            evil_script = GetComponent<EvilBehaviour>();
        }
        else if (source == Source.Grandpa)
        {
            agent = null;
        }
    }


    #region Animator Triggers

    public void AnimateString()
    {
        animator.SetTrigger("AnimateString");
    }

    #endregion
}
