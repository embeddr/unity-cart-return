// Cart-obstacle collision behavior
//
// Intended to be attached to the player cart and any stacked carts. Handles a collision between
// the current cart object and an obstacle object. On collision, sets the game state to game
// over. Additionally, exposes and fires an event, to which other components can subscribe to
// appropriately respond to a cart-obstacle collision.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CartObstacleCollision : MonoBehaviour
{
    public delegate void CollisionHandler();
    public static event CollisionHandler OnCollision;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameData.State == GameState.InGame) {
            // Check for collision with obstacle
            if (collision.gameObject.CompareTag(Tags.Obstacle.ToString())) {
                Debug.Log("Cart-obstacle collision!");

                // Move state to game over
                GameData.State = GameState.GameOver;

                // Fire a collision event for other behavior updates
                OnCollision?.Invoke();
            }
        }
    }
}
