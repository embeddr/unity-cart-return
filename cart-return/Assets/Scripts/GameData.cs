// Global game data
//
// Simple static class to provide convenient global access to game data.

using UnityEngine;
using UnityEngine.InputSystem;

public static class GameData
{
    ///////////////////////////////////////////////////////////////////////////
    // Events to be invoked on game data property update
    ///////////////////////////////////////////////////////////////////////////

    public delegate void GameStateChangeHandler(GameState newGameState);
    public static event GameStateChangeHandler OnGameStateChange;

    public delegate void FrontCartChangeHandler(GameObject newFrontCart);
    public static event FrontCartChangeHandler OnFrontCartChange;

    public delegate void BackCartChangeHandler(GameObject newBackCart);
    public static event BackCartChangeHandler OnBackCartChange;

    public delegate void ScrollSpeedChangeHandler(float newScrollSpeed);
    public static event ScrollSpeedChangeHandler OnScrollSpeedChange;

    public delegate void StackSizeChangeHandler(uint newStackSize);
    public static event StackSizeChangeHandler OnStackSizeChange;

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

    // Number of dashes available to the player
    public static uint Dashes { get; set; }

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

    // Back cart object
    public static GameObject BackCart {
        get { return _backCart; }
        set {
            OnBackCartChange?.Invoke(value);
            _backCart = value;
        }
    }
    private static GameObject _backCart;

    // Cart stack size (not including the player)
    public static uint StackSize {
        get { return _stackSize; }
        set {
            OnStackSizeChange?.Invoke(value);
            _stackSize = value;
        }
    }
    private static uint _stackSize;

    ///////////////////////////////////////////////////////////////////////////
    // Helper functions
    ///////////////////////////////////////////////////////////////////////////

    // Initialize the game data
    public static void init(GameConfig config,
                            GameObject frontCart,
                            GameObject backCart,
                            uint stackSize)
    {
        State = config.state;
        ScrollSpeed = config.scrollSpeed;
        Dashes = config.dashes;
        MagnetismTime = config.magnetismTime;

        FrontCart = frontCart;
        BackCart = backCart;
        StackSize = stackSize;
    }
}
