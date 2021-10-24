// Global game data
//
// Simple static class to provide convenient global access to game data.

using UnityEngine.InputSystem;

public static class GameData
{
    public static PlayerInput PlayerInput { get; private set; }

    public static GameState State {
        get { return _gameState; }
        set {
            PlayerInput.SwitchCurrentActionMap(value.ToString());
            _gameState = value;
        }
    }
    private static GameState _gameState;

    public static float ScrollSpeed { get; set; }

    // Initialize the game data
    public static void init(GameConfig config, PlayerInput playerInput)
    {
        PlayerInput = playerInput;

        State = config.state;
        ScrollSpeed = config.scrollSpeed;
    }
}
