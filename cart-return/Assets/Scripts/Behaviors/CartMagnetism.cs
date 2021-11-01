// Cart magnetism behavior
//
// Implements a simple magnetism behavior that attracts any free carts within the specified
// radius toward this objects y coordinate. Does not impact horizontal position or model any
// sort of realistic magnetic effect.

using UnityEngine;
using UnityEngine.InputSystem;

public class CartMagnetism : MonoBehaviour
{
    private CartStacking _cartStacking;

    [Tooltip("Radius of magnetic effect")]
    private float _radius = 3.0F;

    private bool _magnetismEnabled = false;

    private Collider2D[] _colliders = new Collider2D[10];

    private InputAction _magnetismAction;

    void Awake()
    {
        _cartStacking = GetComponent<CartStacking>();

        // Find player input component
        var gameController = GameObject.FindWithTag("GameController");
        PlayerInput playerInput = gameController?.GetComponent<PlayerInput>();
        if (!playerInput) {
            Utils.ExitGame("Failed to find GameController with PlayerInput component");
        }

        _magnetismAction = playerInput.actions["InGame/Magnetism"];
    }

    void OnEnable()
    {
        _magnetismAction.started += ToggleMagnetism;
        CartObstacleCollision.OnCollision += DisableMagnetism;
    }

    void OnDisable()
    {
        _magnetismAction.started -= ToggleMagnetism;
        CartObstacleCollision.OnCollision -= DisableMagnetism;
    }

    void ToggleMagnetism(InputAction.CallbackContext context)
    {
        _magnetismEnabled = !_magnetismEnabled;
    }

    void DisableMagnetism()
    {
        _magnetismEnabled = false;
    }

    void FixedUpdate()
    {
        if (_magnetismEnabled && (GameData.MagnetismTime > 0.0F)) {
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask("Obstacle"));

            // Project circle ahead of front cart, find all overlapping colliders
            Vector2 pos = new Vector2(GameData.FrontCart.transform.position.x + _radius,
                                      GameData.FrontCart.transform.position.y);
            int numColliders = Physics2D.OverlapCircle(pos,
                                                       _radius,
                                                       filter,
                                                       _colliders);

            // Apply magnetism effect to vertically align free carts with front cart in stack
            for (int i = 0; i < numColliders; i++) {
                if (_colliders[i].tag == "FreeCart") {
                    // TODO: check for clear LOS from object to target y coord?
                    Rigidbody2D rb2d = _colliders[i].gameObject.GetComponent<Rigidbody2D>();
                    // TODO: This is a great place for a PID!
                    float error = transform.position.y - _colliders[i].transform.position.y;
                    float force = error * 100.0F; // TODO: tune coefficient
                    rb2d.AddForce(Vector2.up * force);
                }
            }

            // Decrement available magnetism time
            GameData.MagnetismTime -= Time.fixedDeltaTime;
            if (GameData.MagnetismTime < 0.0F) {
                GameData.MagnetismTime = 0.0F;
            }
        }
    }
}
