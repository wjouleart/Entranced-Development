
using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.AI;

public class Supernova : MonoBehaviour
{
    ParticleSystem boom_particle;
    VideoPlayer ending_video;
    Animator animator;

    public Text ending_text;
    public AudioSource bgm, wave;
    public Transform target;
    public Transform grandpa;
    public MaskingBehaviour mask_script;
    public Transform second_pivot;
    public GameObject string2;


    void Awake()
    {
        boom_particle = transform.Find("During Supernova").GetComponent<ParticleSystem>();
        ending_video = GetComponentInChildren<VideoPlayer>();
        animator = GetComponent<Animator>();
        animator.ResetTrigger("LetsGoEnding");
        Invoke("FinishEndVideo", 20.0f);
    }

    IEnumerator AudioFadingOut()
    {
        for (int i = 1; i <= 20; i++)
        {
            bgm.volume -= 0.05f;
            wave.volume -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator AudioFadingIn()
    {
        for (int i = 1; i <= 20; i++)
        {
            bgm.volume += 0.03f;
            wave.volume += 0.03f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator PlayerEnding()
    {
        yield return new WaitForSeconds(4.0f);

        Transform player = GameObject.FindWithTag("Player").transform;
        player.GetComponent<PlayerBehaviour>().isDead = true;
        yield return null;

        player.GetComponent<NavMeshAgent>().enabled = false;
        yield return null;

        player.position = target.position;
        player.LookAt(new Vector3(grandpa.position.x, player.position.y, grandpa.position.z));
    }

    public void StartEnding()
    {
        StartCoroutine("AudioFadingOut");
        StartCoroutine("PlayerEnding");
    }

    void FinishEndVideo()
    {
        animator.SetTrigger("LetsGoEnding");
        StartCoroutine("AudioFadingIn");
        mask_script.EndGame();
        AudioManager.PlaySound("supernova");
    }

    public void PlayEndVideo()
    {
        ending_video.Play();
    }

    public void EmitBoom()
    {
        boom_particle.Play();
    }

    public void ChangeText()
    {
        ending_text.text = "Thanks for playing!";
    }

    public void ToLevelTwo()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        player.position = second_pivot.position;
        player.GetComponent<PlayerBehaviour>().isDead = false;
        player.GetComponent<NavMeshAgent>().enabled = true;
        string2.SetActive(true);
        RespawnBehaviour.current_spawn_point = 4;
    }
}
