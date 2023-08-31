using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform Player;
    [Tooltip("How Much the player Can be below the camera (Y Axe)")]
    public float Distance;

    void Update()
    {
        if (!PlayerInteractions.Dead)
        {
            if(transform.position.y - Player.transform.position.y > Distance)
            {
                transform.position = new Vector3(transform.position.x, Player.transform.position.y + Distance
                                                             ,transform.position.z);
            }
        }
    }
}
