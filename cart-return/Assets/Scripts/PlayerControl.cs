using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    // Sets whether object should currently be moveable
    // TODO: De-duplicate with ActionMap enum
    // TODO: Move to general game state file
    public enum GameState {
        // TODO: Add Warmup, ReturnCart?
        InGame,
        Paused,
        GameOver,
    }
    public GameState State {
        get { return _state; }
        set {
            // Configure action maps according to new state
            _state = value;
            switch(value)
            {
                case GameState.InGame:
                    _playerInput.SwitchCurrentActionMap(ActionMaps.InGame.ToString());
                    break;
                case GameState.Paused:
                    _playerInput.SwitchCurrentActionMap(ActionMaps.Paused.ToString());
                    break;
                case GameState.GameOver:
                    _playerInput.SwitchCurrentActionMap(ActionMaps.GameOver.ToString());
                    break;
                default:
                    Utils.ExitGame("Invalid game state");
                    break;
            }
        }
    }
    private GameState _state = GameState.InGame;

    // Force to apply when moving the object 
    [SerializeField]
    private float _moveForce = 50.0F;

    private Rigidbody2D _rb2d;

    // Player input and actions
    private PlayerInput _playerInput;
    private InputAction _upDownAction;
    private InputAction _pauseAction;
    private InputAction _unpauseAction;
    private InputAction _restartAction;

    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();

        _upDownAction = _playerInput.actions["InGame/MoveUpDown"];
        _pauseAction = _playerInput.actions["InGame/Pause"];
        _unpauseAction = _playerInput.actions["Paused/Unpause"];
        _restartAction = _playerInput.actions["GameOver/Restart"];
    }

    void Update()
    {
        // Handling pausing/unpausing
        // TODO: show/hide paused text, darken screen a bit
        if (_pauseAction.triggered) {
            State = GameState.Paused;
            Time.timeScale = 0;
        } else if (_unpauseAction.triggered) {
            State = GameState.InGame;
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
        var force = new Vector2(0, _moveForce * _upDownAction.ReadValue<float>());
        _rb2d.AddForce(force);
    }
}
