using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<ScrollLeft>()) {
            Debug.Log("Collision!");

            // TODO: Instead of doing everything below, consider firing an event!

            // Disable player moveability
            GetComponent<MoveUpDown>()._moveable = false;

            // Disable player rigidbody constraints
            var rb2d = GetComponent<Rigidbody2D>();
            rb2d.constraints = RigidbodyConstraints2D.None;

            // Disable all scrolling
            GameObject[] scrolling_objects;
            scrolling_objects = GameObject.FindGameObjectsWithTag("Scrolling");
            foreach (GameObject scrolling_object in scrolling_objects) {
                scrolling_object.GetComponent<ScrollLeft>()._scrolling = false;
            }

            // Pan camera left
            // TODO: get scroll velocity from global constant somehow?
            var camera_vel = new Vector2(-5.0F, 0);
            Camera.main.GetComponent<Rigidbody2D>().velocity = camera_vel;

            // Disable spawning
            GameObject[] spawning_objects;
            spawning_objects = GameObject.FindGameObjectsWithTag("Spawner");
            foreach (GameObject spawning_object in spawning_objects) {
                spawning_object.GetComponent<SpawnParkedCar>().enabled = false;
            }
        }
    }
}
