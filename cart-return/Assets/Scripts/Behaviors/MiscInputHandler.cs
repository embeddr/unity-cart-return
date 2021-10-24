// Misc player input handling behavior
//
// Contains logic for handling player input that isn't associated with a specific object.

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MiscInputHandler : MonoBehaviour
{
    [Tooltip("PlayerInput component to provide input data")]
    [SerializeField]
    private PlayerInput _playerInput;

    private InputAction _pauseAction;
    private InputAction _unpauseAction;
    private InputAction _restartAction;

    void Awake()
    {
        _pauseAction = _playerInput.actions["InGame/Pause"];
        _unpauseAction = _playerInput.actions["Paused/Unpause"];
        _restartAction = _playerInput.actions["GameOver/Restart"];
    }

    void Update()
    {
        // Handling pausing/unpausing by adjusting time scale
        if (_pauseAction.triggered) {
            GameData.State = GameState.Paused;
            Time.timeScale = 0;
        } else if (_unpauseAction.triggered) {
            GameData.State = GameState.InGame;
            Time.timeScale = 1;
        }

        // Handle restarting by reloading the scene entirely
        if (_restartAction.triggered) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
