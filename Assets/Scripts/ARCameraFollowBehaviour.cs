// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using UnityEngine;

public class ARCameraFollowBehaviour : MonoBehaviour
{
    bool isStartGame;
    bool isCutScene;
    Transform player;
    AREditModeBehaviour arEdit_script;

    Vector3 target_position;
    Vector3 smoothVelocity = Vector3.zero;

    public Transform home_scene;

    public GameObject teststring;
    int testAmount;


    void Awake()
    {
        isStartGame = false;
        isCutScene = false;
        player = GameObject.FindWithTag("Player").transform;
        arEdit_script = GameObject.FindWithTag("AREditMode").GetComponent<AREditModeBehaviour>();


        teststring.SetActive(false);
        testAmount = 0;
    }

    void Update()
    {
        if (isStartGame)
        {
            if (!isCutScene)
            {
                if (transform.position != player.position)
                    transform.position = Vector3.SmoothDamp(transform.position, player.position, ref smoothVelocity, 1.0f);
            }
            else
            {
                if (transform.position != target_position)
                    transform.position = Vector3.SmoothDamp(transform.position, target_position, ref smoothVelocity, 1.2f);
            }
        }
        else
        {
            if (transform.position != home_scene.position)
                transform.position = Vector3.SmoothDamp(transform.position, home_scene.position, ref smoothVelocity, 1.0f);
        }
    }

    public void TheOnlyWayToGame(bool active)
    {
        isStartGame = active;
        arEdit_script.StartGame(active);
        Camera.main.transform.root.GetChild(1).GetComponent<MaskingBehaviour>().StartGame(active);
    }

    public void CutSceneToRoad()
    {
        target_position = new Vector3(-6.3f, 1.1f, -5.5f);
        isCutScene = true;
    }

    public void CutSceneToPlayer()
    {
        target_position = player.position;
    }

    public void StopCutScene()
    {
        isCutScene = false;
    }

    public void TestTestTest(bool active)
    {
        testAmount += 1;
        home_scene.gameObject.SetActive(!active);
        teststring.SetActive(active);

        if (testAmount == 1)
            GameObject.FindWithTag("Dialogue").GetComponent<DialogueManager>().StartDialogue("StonePushing");
    }
}
