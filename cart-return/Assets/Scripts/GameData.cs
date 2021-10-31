// Global game data
//
// Simple static class to provide convenient global access to game data.

using UnityEngine.InputSystem;

public static class GameData
{
    // Event to be invoked on game state changes
    public delegate void StateChangeHandler(GameState newState);
    public static event StateChangeHandler OnStateChange;

    // Main game state
    public static GameState State {
        get { return _gameState; }
        set {
            // Fire event on state change
            OnStateChange?.Invoke(value);
            _gameState = value;
        }
    }
    private static GameState _gameState;

    // Horizontal world scroll speed
    public static float ScrollSpeed { get; set; }

    // Number of nudges available to the player
    public static uint Nudges { get; set; }

    // Magnetism time available in seconds
    public static float MagnetismTime { get; set; }

    // Current cart stack size (not including the player)
    public static uint StackSize { get; set; }

    // Initialize the game data
    public static void init(GameConfig config, uint stackSize)
    {
        State = config.state;
        ScrollSpeed = config.scrollSpeed;
        Nudges = config.nudges;
        MagnetismTime = config.magnetismTime;

        StackSize = stackSize;
    }
}
