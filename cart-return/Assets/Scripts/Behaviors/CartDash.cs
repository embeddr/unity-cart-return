// Cart dash behavior
//
// Provides functionality for nudging the player's cart stack up/down based on player input.

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CartDash : MonoBehaviour
{
    [Tooltip("The dash duration in seconds")]
    [SerializeField]
    private float _dashDuration = 0.2F;

    [Tooltip("The dash distance")]
    [SerializeField]
    private float _dashDistance = 6.0F;

    private InputAction _nudgeAction;
    private InputAction _upDownAction;

    private bool _debounceDone = true;
    private float _debounceTime;

    void Awake()
    {
        // Find player input component
        var gameController = GameObject.FindWithTag("GameController");
        PlayerInput playerInput = gameController?.GetComponent<PlayerInput>();
        if (!playerInput) {
            Utils.ExitGame("Failed to find GameController with PlayerInput component");
        }

        _nudgeAction = playerInput.actions["InGame/Nudge"];
        _upDownAction = playerInput.actions["InGame/MoveUpDown"];
    }

    void OnEnable()
    {
        _nudgeAction.started += RequestDash;
    }

    void OnDisable()
    {
        _nudgeAction.started -= RequestDash;
    }

    IEnumerator Dash(GameObject cart, float targetY)
    {
        // Cache rigidbody for duration of coroutine
        var rb2d = cart.GetComponent<Rigidbody2D>();
        var newPos = new Vector3();

        // Yield for fixed update
        yield return new WaitForFixedUpdate();

        float coroutineTime = 0.0F;
        while (coroutineTime < _dashDuration) {
            // Calculate costine shape factor
            float shapeFactor = (Mathf.PI / 2.0F) * 
                    Mathf.Cos((coroutineTime / _dashDuration) * Mathf.PI / 2.0F);

            // Calculate and apply vertical delta
            float deltaY = (targetY - cart.transform.position.y) * 
                    (Time.fixedDeltaTime / _dashDuration) * shapeFactor;
            newPos = cart.transform.position;
            newPos.y += deltaY;
            rb2d.MovePosition(newPos);

            // Increment time and yield to continue in next FixedUpdate()
            coroutineTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        // Terminate coroutine
        yield break;
    }

    void RequestDash(InputAction.CallbackContext context)
    {
        if ((GameData.Dashes > 0) && _debounceDone) {
            // Apply fixed impulse to object to move it up or down based on the player's
            // up/down input. If no up/down input is provided, then the nudge is ignored.
            float direction = _upDownAction.ReadValue<float>();
            float sign = Math.Sign(direction); // note: Math, not Mathf (zero behavior)

            if (sign != 0) {
                // Determine target position from back cart position
                var targetY = GameData.BackCart.transform.position.y + (sign * _dashDistance);

                // Start coroutine for each cart in stack
                CartStacking cartIter = GameData.BackCart.GetComponent<CartStacking>();
                while (cartIter) {
                    StartCoroutine(Dash(cartIter.gameObject, targetY));
                    cartIter = cartIter.forwardCart;
                }

                GameData.Dashes--;
            }

            // Init debouncing
            _debounceDone = false;
            _debounceTime = 0.1F;
        }
    }

    void Update()
    {
        if (_debounceTime > 0.0F) {
            _debounceTime -= Time.deltaTime;
            if (_debounceTime <= 0.0F) {
                _debounceDone = true;
            }
        }
    }
}
