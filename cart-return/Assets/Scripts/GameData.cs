using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GameData
{
    public static PlayerInput PlayerInput {
        get { return _playerInput; }
    }
    private static PlayerInput _playerInput;

    public static GameState State {
        get { return _gameState; }
        set {
            // Require initialization before setting game state
            if (!_playerInput) {
                Utils.ExitGame("Attempted to set game state before initializing");
            }

            _gameState = value;

            // Update player input action map according to state
            // TODO: define this mapping in a better place
            switch (value) {
                case GameState.InGame:
                    _playerInput.SwitchCurrentActionMap(GameState.InGame.ToString());
                    break;
                case GameState.Paused:
                    _playerInput.SwitchCurrentActionMap(GameState.Paused.ToString());
                    break;
                case GameState.GameOver:
                    _playerInput.SwitchCurrentActionMap(GameState.GameOver.ToString());
                    break;
                default:
                    Utils.ExitGame("Invalid game state");
                    break;

            }
        }
    }
    private static GameState _gameState;

    public static float ScrollSpeed { get; set; }

    // Initialization data
    public struct InitData
    {
        public PlayerInput PlayerInput { get; set; }
        public GameState GameState { get; set; }
        public float ScrollSpeed { get; set; }
    }

    // Initialize the game data
    // Note: This must be called before attempting to set the game state propery
    public static void init (InitData init_data)
    {
        _playerInput = init_data.PlayerInput;
        _gameState = init_data.GameState;
        ScrollSpeed = init_data.ScrollSpeed;
    }
}
