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
    [Tooltip("CartStacking component for cart behind this cart (if any)")]
    public CartStacking backCart;

    [Tooltip("CartStacking component for cart in front of this cart (if any)")]
    public CartStacking forwardCart;

    [Tooltip("Scale factor for cart rotation")]
    [SerializeField]
    private float _rotationScale = 0.25F;

    // Large angle threshold [deg]
    private const float _badAngle = 35.0F;

    // Large-angle collision time after which to disable colliders [sec]
    private const float _badAngleColliderDisableTime = 0.20F;

    // Any-angle collision time after which to disable colliders [sec]
    private const float _colliderDisableTime = 0.05F;

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
            var newCart = Instantiate(stackedCartObject,
                                      new Vector2(cartX, cartY),
                                      stackedCartObject.transform.rotation);
            forwardCart = newCart.GetComponent<CartStacking>();

            // Attach spring joint to the new cart
            _joint.enabled = true;
            _joint.connectedBody = newCart.GetComponent<Rigidbody2D>();

            // Initialize various variables for the new stacked cart
            newCart.name = stackedCartObject.name + (_stackCount++).ToString();
            newCart.GetComponent<SpriteRenderer>().sortingOrder = _stackCount;
            newCart.GetComponent<CartStacking>().backCart = this;

            // Update relevant game data
            GameData.FrontCart = forwardCart.gameObject;
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
        if (collision.gameObject.CompareTag(Tags.FreeCart.ToString())) {
            _collisionTime += Time.fixedDeltaTime;
            // If too much time spent in a collision with a free cart, disable its colliders
            // Note: This simple approach assumes at most one cart is colliding with this object
            if (_collisionTime > _colliderDisableTime) {
                DisableColliders(collision.gameObject);
            }

            // Occasionally, a bad collision results in the collided cart being heavily rotated
            // and pulling the player up/down. Disable colliders sooner if this happens.
            if (_collisionTime > _badAngleColliderDisableTime) {
                if (Mathf.Abs(collision.transform.eulerAngles.z) > _badAngle) {
                    DisableColliders(collision.gameObject);
                }
            }

        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.FreeCart.ToString())) {
            var audioSource = collision.gameObject.GetComponent<AudioSource>();
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }

            // Reset collision timer on exit
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

