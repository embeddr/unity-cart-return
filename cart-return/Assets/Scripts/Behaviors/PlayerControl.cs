// Player control behavior
//
// Contains simple logic for controlling the player cart based on input actions.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    // Force to apply when moving the object 
    [SerializeField]
    private float _moveForce = 50.0F;

    private Rigidbody2D _rb2d;

    // Player actions
    private InputAction _upDownAction;
    private InputAction _pauseAction;
    private InputAction _unpauseAction;
    private InputAction _restartAction;

    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();

        _upDownAction = GameData.PlayerInput.actions["InGame/MoveUpDown"];
        _pauseAction = GameData.PlayerInput.actions["InGame/Pause"];
        _unpauseAction = GameData.PlayerInput.actions["Paused/Unpause"];
        _restartAction = GameData.PlayerInput.actions["GameOver/Restart"];
    }

    void Update()
    {
        // Handling pausing/unpausing
        // TODO: show/hide paused text, darken screen a bit
        if (_pauseAction.triggered) {
            GameData.State = GameState.Paused;
            Time.timeScale = 0;
        } else if (_unpauseAction.triggered) {
            GameData.State = GameState.InGame;
            Time.timeScale = 1;
        }

        // Handle restarting
        if (_restartAction.triggered) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    void FixedUpdate()
    {
        // Handle player up/down control
        var force = new Vector3(0, _moveForce * _upDownAction.ReadValue<float>(), 0);
        _rb2d.AddForce(force);
    }
}
