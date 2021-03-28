// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using UnityEngine;

[ExecuteAlways]
public class MaskingBehaviour : MonoBehaviour
{
    public Material[] materials = new Material[13];
    ARCameraFollowBehaviour arCamera_script;
    Transform negativeMask;
    Transform positiveMask;
    Vector3 smoothVelocity = Vector3.zero;

    bool isActivated_DPlatform;
    bool isStartGame;
    bool isEndGame;

    Vector3 default_area = new Vector3(1.0f, 1.0f, 1.0f);
    Vector3 editor_area = new Vector3(10.0f, 1.0f, 10.0f);
    Vector3 enlarged_area = new Vector3(7.0f, 1.0f, 7.0f);
    Vector3 emptied_area = new Vector3(0.1f, 0.1f, 0.1f);
    Vector3 final_area = new Vector3(1.4f, 1.4f, 1.4f);

    public static Vector4 maskVolume;

    void Awake()
    {
        arCamera_script = Camera.main.transform.root.GetComponent<ARCameraFollowBehaviour>();
        negativeMask = transform.GetChild(0);
        positiveMask = transform.GetChild(1);
        isActivated_DPlatform = false;
        isStartGame = false;
        isEndGame = false;
    }


    // Update is called once per frame
    void Update()
    {
        UpdateArea();
        UpdateMasking();
    }


    void UpdateArea()
    {
        if (isStartGame)
        {
            if (isEndGame)
            {
                if (transform.localScale != final_area)
                    transform.localScale = Vector3.SmoothDamp(transform.localScale, final_area, ref smoothVelocity, 2.0f);
            }
            else if (!isActivated_DPlatform)
            {
                if (transform.localScale != default_area)
                    transform.localScale = Vector3.SmoothDamp(transform.localScale, default_area, ref smoothVelocity, 1.0f);
            }
            else
            {
                if (transform.localScale != enlarged_area)
                    transform.localScale = Vector3.SmoothDamp(transform.localScale, enlarged_area, ref smoothVelocity, 1.0f);
            }
        }
        else
        {
            if (!Application.isEditor)
            {
                if (transform.localScale != default_area)
                    transform.localScale = Vector3.SmoothDamp(transform.localScale, default_area, ref smoothVelocity, 0.3f);
            }
            else
            {
                if (transform.localScale != editor_area)
                    transform.localScale = Vector3.SmoothDamp(transform.localScale, editor_area, ref smoothVelocity, 1.0f);
            }
        }
    }

    void UpdateMasking()
    {
        maskVolume = new Vector4(negativeMask.position.x, negativeMask.position.z, positiveMask.position.x, positiveMask.position.z);

        foreach (Material mat in materials)
        {
            if (mat != null)
                mat.SetVector("_Masking", maskVolume);
        }
    }

    public void Activate_DivinePlatform()
    {
        isActivated_DPlatform = true;
    }

    public void Deactivate_DivinePlatform()
    {
        isActivated_DPlatform = false;
    }

    public void EmptyArea()
    {
        transform.localScale = emptied_area;
    }

    public void StartGame(bool active)
    {
        isStartGame = active;
    }

    public void EndGame()
    {
        isEndGame = true;
    }
}
