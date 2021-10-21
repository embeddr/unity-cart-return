// Player-obstacle collision behavior
//
// This is intended to be attached to the player object. Relieves player control at the time
// of collision. More generally, exposes and fires an obstacle collision event, to which other
// components can subscribe to appropriately respond to a player-obstacle collision.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObstacleCollision : MonoBehaviour
{
    public delegate void CollisionHandler();
    public static event CollisionHandler OnCollision;

    void OnCollisionEnter2D(Collision2D collision) {
        // Check for obstacle collision
        if (collision.gameObject.tag == Tags.Obstacle.ToString()) {
            Debug.Log("Obstacle collision!");

            // Move state to game over
            GetComponent<PlayerControl>().State = PlayerControl.GameState.GameOver;

            // Disable player rigidbody constraints
            var rb2d = GetComponent<Rigidbody2D>();
            rb2d.constraints = RigidbodyConstraints2D.None;

            // Fire a collision event for other behavior updates
            OnCollision?.Invoke();
        }
    }
}
