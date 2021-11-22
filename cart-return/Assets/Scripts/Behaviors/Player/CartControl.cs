// Cart up/down control behavior
//
// Provides functionality for moving the player cart up/down based on player input. This should
// only be attached to the player cart.

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CartControl : MonoBehaviour
{
    [Tooltip("PlayerInput component to provide input data")]
    [SerializeField]
    private PlayerInput _playerInput;

    [Tooltip("Force to apply when moving the cart")]
    [SerializeField]
    private float _moveForce = 50.0F;

    private InputAction _moveUpDownAction;

    private Rigidbody2D _rb2d;
    private Vector3 _force;

    void Awake()
    {
        _moveUpDownAction = _playerInput.actions["InGame/MoveUpDown"];
        _rb2d = GetComponent<Rigidbody2D>();
        _force = new Vector3();
    }

    void FixedUpdate()
    {
        var force_y = _moveForce * _moveUpDownAction.ReadValue<float>();
        _force.y = force_y;
        _rb2d.AddForce(_force);
    }
}
