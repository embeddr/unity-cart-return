// Cart-cart collision behavior
//
// Intended to be attached to the player cart and any stacked carts. Handles a collision between
// the current (cart) object and a free cart object. On collision, destroys the free cart object
// and creates a new stacked cart in its place, extending the player's cart chain.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartCartCollision : MonoBehaviour
{
    [Tooltip("Stacked cart object/prefab to spawn on collision")]
    public GameObject stackedObject;
    private static int _count = 0;

    // Note: Free carts are triggers
    void OnTriggerEnter2D(Collider2D collider) {
        // Check for collision with free cart 
        if (collider.gameObject.tag == Tags.FreeCart.ToString()) {
            Debug.Log("Cart-cart collision!");

            // Cache free cart's position and rotation
            Vector2 cart_position = collider.gameObject.transform.position;
            Quaternion cart_rotation = collider.gameObject.transform.rotation;

            // Destroy free cart and instantiate stacked cart in its place (child)
            Destroy(collider.gameObject);
            var new_cart = Instantiate(stackedObject, cart_position, cart_rotation);

            // Attach spring joint of new cart to this cart
            new_cart.GetComponent<SpringJoint2D>().connectedBody = 
                    gameObject.GetComponent<Rigidbody2D>();
            new_cart.name = stackedObject.name + (_count++).ToString();

            // Point new cart to appropriate prefabs/objects
            new_cart.GetComponent<CartCartCollision>().stackedObject = stackedObject;
            new_cart.GetComponent<CartObstacleCollision>().playerControl =
                    GetComponent<CartObstacleCollision>().playerControl;
        }
    }
}
