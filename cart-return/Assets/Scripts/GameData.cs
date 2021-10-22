using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GameData
{
    private static PlayerInput _playerInput;
    public static GameState State {
        get { return _state; }
        set {
            // Require initialization before setting game state
            if (!_playerInput) {
                Utils.ExitGame("Attempted to set game state before initializing");
            }
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
    private static GameState _state = GameState.InGame;

    // Initialize the game data
    // Note: This must be called before attempting to set the game state propery
    public static void init (PlayerInput playerInput)
    {
        _playerInput = playerInput;
    }

}
