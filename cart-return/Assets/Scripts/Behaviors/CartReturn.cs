// Cart return behavior
//
// Provides functionality for returning a cart of a given type to the cart return corral.

using UnityEngine;

[RequireComponent(typeof(CartStacking))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CartReturn : MonoBehaviour
{
    [Tooltip("The cart type/color, which determines its return bonus")]
    [SerializeField]
    private CartType _cartType = CartType.Normal;

    // Required components
    private CartStacking _cartStacking;

    void Awake()
    {
        _cartStacking = GetComponent<CartStacking>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.ReturnZone.ToString())) {
            if (tag != Tags.Player.ToString()) {
                // Handle type-specific behavior
                switch (_cartType) {
                    case CartType.Normal:
                        // No special behavior
                        GameData.ReturnCountNormal++;
                        break;
                    case CartType.Red:
                        // Red carts provide magnetism time
                        GameData.MagnetismTime += 2.0F;
                        GameData.ReturnCountRed++;
                        break;
                    case CartType.Blue:
                        // Blue carts reduce scroll speed
                        GameData.ScrollSpeed -= 1.0F;
                        GameData.ReturnCountBlue++;
                        break;
                    case CartType.Green:
                        // Green carts provide nudges
                        GameData.Dashes++;
                        GameData.ReturnCountGreen++;
                        break;
                    default:
                        Utils.ExitGame("Returned invalid cart type: " + _cartType.ToString());
                        break;
                }

                // Cart behind this one is now the front
                GameData.FrontCart = _cartStacking.backCart.gameObject;
                _cartStacking.backCart.GetComponent<CartStacking>().forwardCart = null;
                var cartBackSpring = _cartStacking.backCart.GetComponent<SpringJoint2D>();
                cartBackSpring.connectedBody = null;
                cartBackSpring.enabled = false;

                // Destroy this cart and any carts in front
                var numCarts = deleteForwardCarts(_cartStacking);
                GameData.StackSize -= numCarts;
            }
        }
    }

    int deleteForwardCarts(CartStacking cart)
    {
        Destroy(cart.gameObject);

        if (cart.forwardCart) {
            return deleteForwardCarts(cart.forwardCart) + 1;
        } else {
            return 1;
        }
    }
}
