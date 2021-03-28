using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CurrenciesManager : MonoBehaviour
{
    int inventory_currency = 0;
    Transform[] patterns;
    ParticleSystem particleIndicator;
    ParticleSystem particleCollector;
    Transform player;

    public Text currency_text;


    void Awake()
    {
        InitializePatterns();
        InitializeParticles();
        InitializePlayer();
    }

    void InitializePlayer()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void InitializeParticles()
    {
        particleIndicator = transform.GetChild(0).GetComponent<ParticleSystem>();
        particleCollector = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    void InitializePatterns()
    {
        patterns = new Transform[transform.childCount - 2];

        for (int i = 0; i < transform.childCount - 2; i++)
            patterns[i] = transform.GetChild(i + 2);
    }

    bool CheckDistance(int i, float dist)
    {
        return Vector3.Distance(patterns[i].position, player.position) < dist;
    }

    void Start()
    {
        StartCoroutine("Base");
    }

    IEnumerator Base()
    {
        while (true)
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if (CheckDistance(i, 0.4f))
                {
                    particleCollector.transform.position = patterns[i].position;
                    particleCollector.Play();
                    yield return new WaitForSeconds(0.5f);

                    inventory_currency += 1;
                    currency_text.text = inventory_currency.ToString();
                    patterns[i].position = new Vector3(0, 0, 0);
                    Dialogue();
                }
                else if (CheckDistance(i, 4.0f))
                {
                    particleIndicator.transform.position = patterns[i].position;
                    particleIndicator.Emit(1);
                }

                yield return null;
            }

            yield return new WaitForSeconds(2 / patterns.Length);
        }
    }

    void Dialogue()
    {
        if (inventory_currency == 1)
        {
            GameObject.FindWithTag("Dialogue").GetComponent<DialogueManager>().StartDialogue("Explain_Currencies");
        }
    }
}
