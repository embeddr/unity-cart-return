using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveUpDown : MonoBehaviour
{
    private PlayerInput playerInput_;
    private InputAction upDownAction_;
    private Rigidbody2D rb2d_;

    void Awake()
    {
        rb2d_ = GetComponent<Rigidbody2D>();
        playerInput_ = GetComponent<PlayerInput>();
        upDownAction_ = playerInput_.actions["InGame/MoveUpDown"];
    }

    void FixedUpdate()
    {
        var force = new Vector2(0, 50 * upDownAction_.ReadValue<float>());
        rb2d_.AddForce(force);
    }

    public void SetUpDownInput(InputAction.CallbackContext context) {
        Debug.Log("UpDown:" + context.ReadValue<float>());
    }
}
