using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveUpDown : MonoBehaviour
{
    // Sets whether object should currently be moveable
    public bool moveable = true;

    // Force to apply when moving the object 
    [SerializeField]
    private float _moveForce = 50.0F;

    private PlayerInput _playerInput;
    private InputAction _upDownAction;
    private Rigidbody2D _rb2d;

    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _upDownAction = _playerInput.actions["InGame/MoveUpDown"];
    }

    void FixedUpdate()
    {
        if (moveable) {
            var force = new Vector2(0, _moveForce * _upDownAction.ReadValue<float>());
            _rb2d.AddForce(force);
        }
    }
}
