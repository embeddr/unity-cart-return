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

    [Tooltip("Audio source for magnetism sound effect")]
    [SerializeField]
    private AudioSource _magnetismSoundSource;

    [Tooltip("Required magnetism spin-down time in seconds")]
    [SerializeField]
    private float _magnetismSpindownTimeRequired = 0.25F;

    [Tooltip("Base sound effect start/stop pitch")]
    [SerializeField]
    private float _magnetismPitchStart = 0.1F;

    // Whether or not magnetism has been requested by the player
    private bool _magnetismRequested = false;
    
    // Magnitism state
    enum MagnetismState {
        Inactive,
        Active,
        Spindown,
    }
    private MagnetismState _magnetismState = MagnetismState.Inactive;

    private Collider2D[] _colliders = new Collider2D[10];

    private InputAction _magnetismAction;

    private float _magnetismPitchTarget;

    private float _magnetismSpindownTime;

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
        _magnetismPitchTarget = _magnetismSoundSource.pitch;
    }

    void OnEnable()
    {
        _magnetismAction.started += ToggleMagnetismRequest;
        CartObstacleCollision.OnCollision += DisableMagnetism;
    }

    void OnDisable()
    {
        _magnetismAction.started -= ToggleMagnetismRequest;
        CartObstacleCollision.OnCollision -= DisableMagnetism;
    }

    void ToggleMagnetismRequest(InputAction.CallbackContext context)
    {
        _magnetismRequested = !_magnetismRequested;
    }

    void DisableMagnetism()
    {
        _magnetismRequested = false;
    }

    void FixedUpdate()
    {
        // Decrement magnetism time if active
        if (_magnetismState != MagnetismState.Inactive) {
            GameData.MagnetismTime -= Time.fixedDeltaTime;
            // Clamp at zero
            if (GameData.MagnetismTime < 0.0F) {
                GameData.MagnetismTime = 0.0F;
            }
        }

        // Update magnetism state machine
        switch (_magnetismState) {
            case MagnetismState.Inactive:
                // Transition to active if requested and time is available
                if (_magnetismRequested && GameData.MagnetismTime > 0.0F) {
                    _magnetismState = MagnetismState.Active;
                }
                break;
            case MagnetismState.Active:
                // Transition to spindown if no longer requested
                if (!_magnetismRequested) {
                    _magnetismSpindownTime = _magnetismSpindownTimeRequired;
                    _magnetismState = MagnetismState.Spindown;
                }

                // Transition to inactive if no magnetism time remaining
                if (GameData.MagnetismTime <= 0.0F) {
                    GameData.MagnetismTime = 0.0F;
                    _magnetismRequested = false;
                    _magnetismState = MagnetismState.Inactive;
                }
                break;
            case MagnetismState.Spindown:
                // Transition to inactive if spin-down time or magnetism time reach zero
                _magnetismSpindownTime -= Time.fixedDeltaTime;
                if ((_magnetismSpindownTime <= 0.0F) || (GameData.MagnetismTime <= 0.0F)) {
                    _magnetismRequested = false;
                    _magnetismState = MagnetismState.Inactive;
                } else if (_magnetismRequested) {
                    // Otherwise, transition back to active if requested
                    _magnetismState = MagnetismState.Active;
                }
                break;
        }

        // Apply magnetism effect
        if (_magnetismState != MagnetismState.Inactive) {
            // Play base sound while magnetism is enabled
            if (!_magnetismSoundSource.isPlaying) {
                _magnetismSoundSource.pitch = _magnetismPitchStart;
                _magnetismSoundSource.Play();
            }

            // Pitch up when first enabled
            if (_magnetismSoundSource.pitch < _magnetismPitchTarget) {
                _magnetismSoundSource.pitch += 0.1F;
            }

            // Project circle ahead of front cart, find all overlapping colliders
            Vector2 pos = new Vector2(GameData.FrontCart.transform.position.x + _radius,
                                      GameData.FrontCart.transform.position.y);
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask("Obstacle"));
            int numColliders = Physics2D.OverlapCircle(pos,
                                                       _radius,
                                                       filter,
                                                       _colliders);

            // Apply magnetism effect to vertically align free carts with front cart in stack
            for (int i = 0; i < numColliders; i++) {
                if (_colliders[i].CompareTag("FreeCart")) {
                    // TODO: check for clear LOS from object to target y coord?
                    Rigidbody2D rb2d = _colliders[i].gameObject.GetComponent<Rigidbody2D>();
                    // TODO: This is a great place for a PID!
                    float error = transform.position.y - _colliders[i].transform.position.y;
                    float force = error * 100.0F; // TODO: tune coefficient
                    rb2d.AddForce(Vector2.up * force);
                }
            }

            // Spin down sound
            if (_magnetismState == MagnetismState.Spindown) {
                // Reduce pitch so that we reach the starting pitch at the end of spin-down
                float timeRatio = (Time.fixedDeltaTime / _magnetismSpindownTime);
                _magnetismSoundSource.pitch -= timeRatio * (_magnetismSoundSource.pitch - _magnetismPitchStart);

                // Clamp at start pitch
                if (_magnetismSoundSource.pitch < _magnetismPitchStart) {
                    _magnetismSoundSource.pitch = _magnetismPitchStart;
                }
            }
        } else {
            // Stop base sound
            _magnetismSoundSource.Stop();
        }
    }
}
