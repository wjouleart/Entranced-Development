// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenVideo : MonoBehaviour
{
    void Start()
    {
        Invoke("ToMainScene", 40.0f);
    }

    void ToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
