// Cart return behavior
//
// Provides functionality for returning a cart of a given type to the cart return corral.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                Debug.Log("Player cart is now the front!");
                GetComponent<CartCartCollision>().frontCart = true;
            } else {
                Debug.Log("Returning " + _cartType.ToString() + " cart!");

                switch (_cartType) {
                    case CartType.Normal:
                        // No special behavior
                        break;
                    case CartType.Red:
                        // TODO
                        break;
                    case CartType.Blue:
                        GameData.ScrollSpeed -= 1.0F;
                        break;
                    case CartType.Green:
                        // TODO
                        break;
                    default:
                        Utils.ExitGame("Returned invalid cart type: " + ((int)_cartType).ToString());
                        break;
                }

                Destroy(gameObject);
            }
        }
    }
}
