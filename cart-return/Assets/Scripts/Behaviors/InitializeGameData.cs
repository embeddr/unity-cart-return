// Game data initialization behavior
//
// This script initializes the static game data with data from the provided game config
// asset, as well as any other data required from the scene itself.

using UnityEngine;
using UnityEngine.InputSystem;

public class InitializeGameData : MonoBehaviour
{
    [Tooltip("PlayerInput component to provide input data")]
    [SerializeField]
    private PlayerInput _playerInput;

    [Tooltip("Game configuration asset to load")]
    [SerializeField]
    private GameConfig _gameConfig;

    void Awake()
    {
        GameData.init(_gameConfig, _playerInput);
    }

}
