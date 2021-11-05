// Cart-cart collision behavior
//
// Intended to be attached to the player cart and any stacked carts. Handles a collision between
// the current (cart) object and a free cart object. On collision, destroys the free cart object
// and creates a new stacked cart in its place, extending the player's cart chain.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CartStacking : MonoBehaviour
{
    // Count for number of stacked carts that have been created
    private static int _stackCount = 0;
    
    // Duration a cart has been continuously colliding with this object
    private float _collisionTime = 0.0F;

    // Whether or not this cart is the front cart in the player cart stack
    private bool _isFrontCart;

    void OnEnable()
    {
        GameData.OnFrontCartChange += CheckFrontCart;
    }

    void OnDisable()
    {
        GameData.OnFrontCartChange -= CheckFrontCart;
        _isFrontCart = false;
    }

    void CheckFrontCart(GameObject newFrontCart)
    {
        _isFrontCart = (newFrontCart == gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        // Check for trigger between front stacked cart and free cart 
        if (_isFrontCart && (other.gameObject.tag == Tags.FreeCart.ToString())) {
            // Use free cart's previous vertical position, but use a fixed offset from the
            // current cart's horizontal position so that the stack is consistently spaced.
            var freeCart = other.gameObject;
            float cartY = freeCart.transform.position.y;
            float cartX = transform.position.x + 0.5F;

            GameObject stackedCartObject = freeCart.GetComponent<ObjectContainer>()?.Object;
            if (!stackedCartObject) {
                Utils.ExitGame("Collided free cart holds no ObjectContainer (or object is unset)");
            }

            // Destroy free cart and instantiate stacked cart in its place
            Destroy(freeCart);
            var stackedCart = Instantiate(stackedCartObject,
                                          new Vector2(cartX, cartY),
                                          stackedCartObject.transform.rotation,
                                          transform);

            // Attach spring joint of new stacked cart to this cart
            SpringJoint2D joint = stackedCart.GetComponent<SpringJoint2D>();
            if (!joint) {
                Utils.ExitGame("Attempting to stack a GameObject with no spring joint component");
            }
            joint.connectedBody = GetComponent<Rigidbody2D>();
            stackedCart.name = stackedCartObject.name + (_stackCount++).ToString();

            // Set new stacked cart's sorting order to appear on top 
            stackedCart.GetComponent<SpriteRenderer>().sortingOrder = _stackCount;

            // Update relevant game data
            GameData.FrontCart = stackedCart;
            GameData.StackSize++;
        }
    }

    void DisableColliders(GameObject obj)
    {
        // Disable all colliders for the given object
        Collider2D[] colliders =
            obj.GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders) {
            collider.enabled = false;
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // If too much time spent in a collision with a free cart, disable its colliders
        // Note: This simple approach assumes at most one cart is colliding with this object
        if (collision.gameObject.tag == Tags.FreeCart.ToString()) {
            _collisionTime += Time.fixedDeltaTime;
            if (_collisionTime > 0.20F) {
                DisableColliders(collision.gameObject);
            }

            // Occasionally, a bad collision results in the collided cart being roated ~45
            // degrees, pulling the player cart(s) up/down out of control. Disable colliders
            // sooner if this occurs.
            if (Mathf.Abs(collision.transform.eulerAngles.z) > 35.0F) {
                if (_collisionTime > 0.05F) {
                    DisableColliders(collision.gameObject);
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
