using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public static bool detected_player = EvilBehaviour.detected_player;

    void Awake()
    {
        Debug.Log("detected_player = " + detected_player);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!detected_player && other.gameObject.tag == "Player")
        {
            //transform.parent.GetComponent<EvilBehaviour>().CollisionDetected(this);
       
            detected_player = true;
            EvilBehaviour.eye_indicator.SetActive(true);
            AudioManager.PlaySound("evil_detect");

            Debug.Log("HERE COMES JOHHNNYY!!!!");
            Debug.Log("detected_player = " + detected_player);
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (detected_player && other.gameObject.tag == "Player")
        {
            detected_player = false;
            EvilBehaviour.eye_indicator.SetActive(false);
        }
    }
}
