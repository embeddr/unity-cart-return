// Camera control behavior

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    void OnEnable()
    {
       CartObstacleCollision.OnCollision += FocusOnCollision; 
    }

    void OnDisable()
    {
       CartObstacleCollision.OnCollision -= FocusOnCollision; 
    }

    void FocusOnCollision()
    {
        // Pan camera left to draw attention to the collision
        var camera_vel = new Vector2(-5.0F, 0);
        Camera.main.GetComponent<Rigidbody2D>().velocity = camera_vel;
    }
}
