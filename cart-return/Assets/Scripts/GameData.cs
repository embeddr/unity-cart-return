// Global game data
//
// Simple static class to provide convenient global access to game data.

using UnityEngine.InputSystem;

public static class GameData
{
    public delegate void StateChangeHandler(GameState newState);
    public static event StateChangeHandler OnStateChange;

    public static GameState State {
        get { return _gameState; }
        set {
            // Fire event on state change
            OnStateChange?.Invoke(value);
            _gameState = value;
        }
    }
    private static GameState _gameState;

    public static float ScrollSpeed { get; set; }

    // Initialize the game data
    public static void init(GameConfig config)
    {
        State = config.state;
        ScrollSpeed = config.scrollSpeed;
    }
}
