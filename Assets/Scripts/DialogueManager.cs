// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    string[] sentences;
    Queue<string> display_sentences;

    bool isReadyToNext;
    bool isInDialogue;
    Text dialogue;
    Animator animator, banner_animator;

    bool isGrandpaTalking;


    void Awake()
    {
        display_sentences = new Queue<string>();
        dialogue = transform.GetChild(4).GetComponent<Text>();
        animator = GetComponent<Animator>();
        banner_animator = GameObject.FindWithTag("Banner").GetComponent<Animator>();
        dialogue.text = "";
        isReadyToNext = true;
        isInDialogue = false;
        isGrandpaTalking = false;
    }

    public void StartDialogue(string source)
    {
        animator.ResetTrigger("EndDialogue");
        banner_animator.ResetTrigger("HideBanner");
        StartCoroutine("Starting", source);
    }

    IEnumerator Starting(string source)
    {
        isInDialogue = true;
        // Set the current sentences
        SentencesManager(source);
        DialogueBoxManager(source);
        // Clear the current queue
        display_sentences.Clear();
        // Set the current queue
        foreach (string sentence in sentences)
            display_sentences.Enqueue(sentence);

        yield return null;

        DisplayNextSentence();
        yield return null;

        banner_animator.SetTrigger("ShowBanner");
    }

    public void DisplayNextSentence()
    {
        if (isReadyToNext)
        {
            if (display_sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
            else if (isGrandpaTalking)
            {
                AudioManager.PlaySound("grandpa_talk");
            }

            string current_sentence = display_sentences.Dequeue();
            StartCoroutine(Typing(current_sentence));
            isReadyToNext = false;
        }

    }

    IEnumerator Typing(string current_sentence)
    {
        dialogue.text = "";
        int letterCount = 0;

        foreach (char letter in current_sentence.ToCharArray())
        {
            letterCount++;
            if (Input.GetMouseButton(0) || Input.touchCount == 1)
            {
                if (letterCount > 0)
                {
                    dialogue.text += current_sentence.Substring(letterCount - 1);
                }
                else
                {
                    dialogue.text += current_sentence.Substring(letterCount);
                }
                yield return null;
                break;
            }
            else
            {
                dialogue.text += letter;
                yield return null;
            }

        }

        yield return new WaitForSeconds(0.5f);
        isReadyToNext = true;
    }

    void EndDialogue()
    {
        isInDialogue = false;
        isGrandpaTalking = false;
        animator.SetTrigger("EndDialogue");
        banner_animator.SetTrigger("HideBanner");
    }

    public bool CheckIsInDialogue()
    {
        return isInDialogue;
    }


    void DialogueBoxManager(string source)
    {
        switch (source)
        {
            case "Explain_Currencies":
            case "Path_Not_Finished":
            case "StonePushing":
                animator.SetTrigger("GrandpaDialogue");
                AudioManager.PlaySound("grandpa_cough");
                isGrandpaTalking = true;
                break;

            case "DivinePlatform":
                animator.SetTrigger("GrandpaDialogue");
                AudioManager.PlaySound("grandpa_cough");
                isGrandpaTalking = true;
                break;

            case "Respawn":
                animator.SetTrigger("EvilDialogue");
                AudioManager.PlaySound("evil_dialogue");
                break;
        }
    }

    void SentencesManager(string source)
    {
        if (source == "StonePushing")
        {
            sentences = new string[]
            {
                "Oh, here comes another newcomer.",
                "Welcome to the Island of Spiritual Wood, newcomer. I'm here to guide you through your adventures.",
                "It is believed that the stones here manifest mystical powers.",
                "When placed at their rightful spots, they will unleash their potential power.",
                "But beware of the evil spirits lurking around. They are the guardian of the Tree of Life and its treasures.",
                "The stones are not in their rightful spots due to their interference.",
                "Figure out the stones' rightful spots and reach the Tree of Life.",
                "You can tap the Sape's string on your left to unleash its powers."
            };
        }
        else if (source == "DivinePlatform")
        {
            sentences = new string[]
            {
                "This platform is ancient and sacred. It widens the view of your surroundings.",
                "You can monitor the evil spirits' movements and set your strategies here."
            };
        }
        else if (source == "Respawn")
        {
            sentences = new string[]
            {
                "A girl with sape? That is absolutely not allowed.",
                "YOU CAN'T BE HERE, LEAVE NOW !!"
            };
        }
        else if (source == "Path_Not_Finished")
        {
            sentences = new string[]
            {
                "Oh.. It seems that you have arrived early.",
                "You cannot enter the Tree of Life until you have constructed all the paths."
            };
        }
        else if (source == "Explain_Currencies")
        {
            sentences = new string[]
            {
                "Oh.. You just found a fragment of Sape's pattern",
                "This is a legacy from people who have been here before.",
                "I saw with my own eyes those evil spirits deliberately breaking their Sape while driving them away.",
                "Those people will never come back anyway. You can use it to upgrade your Sape's abilities."
            };
        }
    }
}
