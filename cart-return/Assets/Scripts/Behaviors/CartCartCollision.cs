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
    public GameObject stackedCartObject;

    public bool frontCart = true;

    // Count for number of stacked carts that have been created
    private static int _stackCount = 0;
    
    // Duration a cart has been continuously colliding with this object
    // Note: Assuming at most one cart is colliding with this object
    private float _collisionTime = 0.0F;

    void OnTriggerEnter2D(Collider2D other) 
    {
        // Check for collision between front stacked cart and free cart 
        var oldCart = other.gameObject;
        if (frontCart && (oldCart.tag == Tags.FreeCart.ToString())) {
            Debug.Log("Cart-cart collision!");

            // Cache free cart's position and rotation
            float cartY = oldCart.transform.position.y;
            float cartX = transform.position.x + 0.5F;

            // Destroy free cart and instantiate stacked cart in its place (child)
            Destroy(oldCart);
            var newCart = Instantiate(stackedCartObject,
                                      new Vector2(cartX, cartY),
                                      stackedCartObject.transform.rotation);

            // Attach spring joint of new stacked cart to this cart
            newCart.GetComponent<SpringJoint2D>().connectedBody = 
                    gameObject.GetComponent<Rigidbody2D>();
            newCart.name = stackedCartObject.name + (_stackCount++).ToString();

            // Point new stacked cart to appropriate prefabs/objects
            newCart.GetComponent<CartCartCollision>().stackedCartObject = stackedCartObject;
            newCart.GetComponent<CartObstacleCollision>().playerControl =
                    GetComponent<CartObstacleCollision>().playerControl;

            // This cart is no longer the front!
            frontCart = false;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        // If too much time spent in a collision with a free cart, disable cart's colliders
        if (collision.gameObject.tag == Tags.FreeCart.ToString()) {
            _collisionTime += Time.fixedDeltaTime;
            if (_collisionTime > 0.25F) {
                PolygonCollider2D[] colliders = 
                    collision.gameObject.GetComponents<PolygonCollider2D>();
                foreach (PolygonCollider2D collider in colliders) {
                    collider.enabled = false;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Reset collision timer on exit
        if (collision.gameObject.tag == Tags.FreeCart.ToString()) {
            _collisionTime = 0.0F;
        }
    }
}
