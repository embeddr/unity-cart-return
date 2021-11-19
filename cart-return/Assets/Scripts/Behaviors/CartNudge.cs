// Cart nudge behavior
//
// Provides functionality for nudging any cart up/down based on player input. This should be
// attached to all stacked cart objects.

using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CartNudge : MonoBehaviour
{
    [Tooltip("The amount of force to apply as an impulse when nudging")]
    [SerializeField]
    private float _impulseForce = 15.0F;

    private Rigidbody2D _rb2d;

    private InputAction _nudgeAction;
    private InputAction _upDownAction;

    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();

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
        _nudgeAction.started += Nudge;
    }

    void OnDisable()
    {
        _nudgeAction.started -= Nudge;
    }

    void Nudge(InputAction.CallbackContext context)
    {
        if (GameData.Nudges > 0) {
            // Apply fixed impulse to object to nudge it up or down based on the player's
            // up/down input. If no up/down input is provided, then the nudge is ignored.
            float direction = _upDownAction.ReadValue<float>();
            float sign = Math.Sign(direction); // note: Math, not Mathf!
            if (sign != 0) {
                // TODO: scale force according to mass?
                var force = Vector3.up * sign * _impulseForce;
                _rb2d.AddForce(force, ForceMode2D.Impulse);
                GameData.Nudges--;
            }
        }
    }
}
