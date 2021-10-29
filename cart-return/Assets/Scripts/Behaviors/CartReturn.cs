// Cart return behavior
//
// Provides functionality for returning a cart of a given type to the cart return corral.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CartReturn : MonoBehaviour
{
    public enum CartType {
        Normal,
        Red,
        Blue,
        Green,
    }

    [Tooltip("The cart type/color, which determines its return bonus")]
    [SerializeField]
    private CartType _cartType = CartType.Normal;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Tags.ReturnZone.ToString()) {
            if (tag == Tags.Player.ToString()) {
                // TODO: assign parent cart as front instead?
                Debug.Log("Player cart is now the front!");
                GetComponent<CartStacking>().isFrontCart = true;
            } else {
                Debug.Log("Returning " + _cartType.ToString() + " cart!");

                // Handle type-specific behavior
                switch (_cartType) {
                    case CartType.Normal:
                        // No special behavior
                        break;
                    case CartType.Red:
                        // TODO
                        break;
                    case CartType.Blue:
                        // Blue carts reduce scroll speed
                        GameData.ScrollSpeed -= 1.0F;
                        break;
                    case CartType.Green:
                        // Green carts provide nudges
                        GameData.Nudges++;
                        break;
                    default:
                        Utils.ExitGame("Returned invalid cart type: " + ((int)_cartType).ToString());
                        break;
                }

                // Decrement stack size and destroy object
                GameData.StackSize--;
                Destroy(gameObject);
            }
        }
    }
}
