// Player-obstacle collision behavior
//
// This is intended to be attached to the player object. Relieves player control at the time
// of collision. More generally, exposes and fires an obstacle collision event, to which other
// components can subscribe to appropriately respond to a player-obstacle collision.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartObstacleCollision : MonoBehaviour
{
    public delegate void CollisionHandler();
    public static event CollisionHandler OnCollision;

    [Tooltip("PlayerControl component to update")]
    [SerializeField]
    private PlayerControl _playerControl;

    void OnCollisionEnter2D(Collision2D collision) {
        // Check for collision with obstacle
        if (collision.gameObject.tag == Tags.Obstacle.ToString()) {
            Debug.Log("Cart-obstacle collision!");

            // Move state to game over
            _playerControl.State = GameState.GameOver;

            // Fire a collision event for other behavior updates
            OnCollision?.Invoke();
        }
    }
}
