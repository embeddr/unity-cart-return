// Global game data
//
// Simple static class to provide convenient global access to game data.

using UnityEngine;
using UnityEngine.InputSystem;

public static class GameData
{
    ///////////////////////////////////////////////////////////////////////////
    // Events to be invoked on game data property value change
    ///////////////////////////////////////////////////////////////////////////

    public delegate void GameStateChangeHandler(GameState newGameState);
    public static event GameStateChangeHandler OnGameStateChange;

    public delegate void FrontCartChangeHandler(GameObject newFrontCart);
    public static event FrontCartChangeHandler OnFrontCartChange;

    public delegate void ScrollSpeedChangeHandler(float newScrollSpeed);
    public static event ScrollSpeedChangeHandler OnScrollSpeedChange;

    ///////////////////////////////////////////////////////////////////////////
    // Game data properties
    ///////////////////////////////////////////////////////////////////////////

    // Main game state
    public static GameState State {
        get { return _gameState; }
        set {
            OnGameStateChange?.Invoke(value);
            _gameState = value;
        }
    }
    private static GameState _gameState;

    // Horizontal world scroll speed
    public static float ScrollSpeed {
        get { return _scrollSpeed; } 
        set {
            OnScrollSpeedChange?.Invoke(value);
            _scrollSpeed = value;
        } 
    }
    private static float _scrollSpeed;

    // Number of nudges available to the player
    public static uint Nudges { get; set; }

    // Magnetism time available in seconds
    public static float MagnetismTime { get; set; }

    // Front cart object
    public static GameObject FrontCart {
        get { return _frontCart; }
        set {
            OnFrontCartChange?.Invoke(value);
            _frontCart = value;
        }
    }
    private static GameObject _frontCart;

    // Cart stack size (not including the player)
    public static uint StackSize { get; set; }

    ///////////////////////////////////////////////////////////////////////////
    // Helper functions
    ///////////////////////////////////////////////////////////////////////////

    // Initialize the game data
    public static void init(GameConfig config, GameObject frontCart, uint stackSize)
    {
        State = config.state;
        ScrollSpeed = config.scrollSpeed;
        Nudges = config.nudges;
        MagnetismTime = config.magnetismTime;

        FrontCart = frontCart;
        StackSize = stackSize;
    }
}
