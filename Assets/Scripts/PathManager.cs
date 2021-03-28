// This file is part of Entranced.
// All program codes are completely written by Wong Ze Hao, 101209329.

using UnityEngine;

public class PathManager : MonoBehaviour
{
    static Transform[] paths;
    static Vector3 target_position;
    static bool isCutScene = false;

    public static int index = -1;

    Vector3 smoothVelocity = Vector3.zero;


    void Awake()
    {
        paths = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            paths[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        if (isCutScene)
        {
            if (paths[index].position != target_position)
                paths[index].position = Vector3.SmoothDamp(paths[index].position, target_position, ref smoothVelocity, 1.0f);
        }
    }

    public static void PathBuilder()
    {
        index += 1;
        Vector3 path = paths[index].position;
        target_position = new Vector3(path.x, path.y + 5.0f, path.z);
        isCutScene = true;
    }

}
