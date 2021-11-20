// Cart-cart collision behavior
//
// Intended to be attached to the player cart and any stacked carts. Handles a collision between
// the current (cart) object and a free cart object. On collision, destroys the free cart object
// and creates a new stacked cart in its place, extending the player's cart chain.

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpringJoint2D))]
public class CartStacking : MonoBehaviour
{
    [Tooltip("Cart GameObject behind this cart (if any)")]
    public GameObject backCart;

    [Tooltip("Cart GameObject in front of this cart (if any)")]
    public GameObject forwardCart;

    [Tooltip("Audio source for cart sliding sound")]
    [SerializeField]
    private AudioSource _slidingSoundSource;

    [Tooltip("Scale factor for cart rotation")]
    [SerializeField]
    private float _rotationScale = 0.25F;

    // Required components
    private SpringJoint2D _joint;
    private Rigidbody2D _rb2d;

    // Count for number of stacked carts that have been created
    private static int _stackCount = 0;
    
    // Duration a cart has been continuously colliding with this object
    private float _collisionTime = 0.0F;

    void Awake()
    {
        _joint = GetComponent<SpringJoint2D>();
        _rb2d = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        // Check for trigger between front stacked cart and free cart 
        bool isFrontCart = (GameData.FrontCart == gameObject);
        if (isFrontCart && (other.gameObject.CompareTag(Tags.FreeCart.ToString()))) {
            // Use free cart's previous vertical position, but use a fixed offset from the
            // current cart's horizontal position so that the stack is consistently spaced
            var freeCart = other.gameObject;
            float cartY = freeCart.transform.position.y;
            float cartX = transform.position.x + 0.5F;

            // Get new cart type from free cart's object container
            GameObject stackedCartObject = freeCart.GetComponent<ObjectContainer>()?.Object;
            if (!stackedCartObject) {
                Utils.ExitGame("Collided free cart holds no ObjectContainer (or object is unset)");
            }

            // Destroy free cart and instantiate stacked cart in its place
            Destroy(freeCart);
            forwardCart = Instantiate(stackedCartObject,
                                      new Vector2(cartX, cartY),
                                      stackedCartObject.transform.rotation);

            // Attach spring joint to the new cart
            _joint.enabled = true;
            _joint.connectedBody = forwardCart.GetComponent<Rigidbody2D>();

            // Initialize various variables for the new stacked cart
            forwardCart.name = stackedCartObject.name + (_stackCount++).ToString();
            forwardCart.GetComponent<SpriteRenderer>().sortingOrder = _stackCount;
            forwardCart.GetComponent<CartStacking>().backCart = gameObject;

            // Update relevant game data
            GameData.FrontCart = forwardCart;
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
        if (collision.gameObject.CompareTag(Tags.FreeCart.ToString())) {
            _collisionTime += Time.fixedDeltaTime;
            if (_collisionTime > 0.20F) {
                DisableColliders(collision.gameObject);
            }

            if (_collisionTime > 0.05F) {
                // Play cart sliding sound
                if (!_slidingSoundSource.isPlaying) {
                    _slidingSoundSource.Play();
                }

                // Occasionally, a bad collision results in the collided cart being roated
                // ~45 degrees, pulling the player cart(s) up/down out of control.
                if (Mathf.Abs(collision.transform.eulerAngles.z) > 35.0F) {
                    DisableColliders(collision.gameObject);
                }
            }

        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Reset collision timer on exit
        if (collision.gameObject.CompareTag(Tags.FreeCart.ToString())) {
            _collisionTime = 0.0F;
        }
    }

    void FixedUpdate()
    {
        // Adjust cart rotation according to position relative to back cart
        if (backCart) {
            var deltaX = transform.position.x - backCart.transform.position.x;
            var deltaY = transform.position.y - backCart.transform.position.y;
            var angle_rad = Mathf.Atan2(deltaY, deltaX) * _rotationScale;
            transform.eulerAngles = new Vector3(0.0F, 0.0F, (Mathf.Rad2Deg * angle_rad));
        }
    }
}
