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

    public delegate void ReturnCountChangeHandler(uint newTotalCount);
    public static event ReturnCountChangeHandler OnReturnCountChange;

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

    // Total carts returned
    public static uint ReturnCountTotal {
        get; private set; // Updated by color-specific setters
    }

    // Normal carts returned
    public static uint ReturnCountNormal {
        get { return _returnCountNormal; }
        set {
            var newTotal = value + 
                           ReturnCountRed + 
                           ReturnCountBlue + 
                           ReturnCountGreen;
            OnReturnCountChange?.Invoke(newTotal);
            _returnCountNormal = value;
            ReturnCountTotal = newTotal;
        }
    }
    private static uint _returnCountNormal;

    // Red carts returned
    public static uint ReturnCountRed {
        get { return _returnCountRed; }
        set {
            var newTotal = ReturnCountNormal + 
                           value + 
                           ReturnCountBlue + 
                           ReturnCountGreen;
            OnReturnCountChange?.Invoke(newTotal);
            _returnCountRed = value;
            ReturnCountTotal = newTotal;
        }
    }
    private static uint _returnCountRed;

    // Blue carts returned
    public static uint ReturnCountBlue {
        get { return _returnCountBlue; }
        set {
            var newTotal = ReturnCountNormal + 
                           ReturnCountRed +
                           value + 
                           ReturnCountGreen;
            OnReturnCountChange?.Invoke(newTotal);
            _returnCountBlue = value;
            ReturnCountTotal = newTotal;
        }
    }
    private static uint _returnCountBlue;

    // Green carts returned
    public static uint ReturnCountGreen {
        get { return _returnCountGreen; }
        set {
            var newTotal = ReturnCountNormal + 
                           ReturnCountRed +
                           ReturnCountBlue +
                           value;
            OnReturnCountChange?.Invoke(newTotal);
            _returnCountGreen = value;
            ReturnCountTotal = newTotal;
        }
    }
    private static uint _returnCountGreen;

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

        ReturnCountNormal = 0;
        ReturnCountRed = 0;
        ReturnCountBlue = 0;
        ReturnCountGreen = 0;
    }
}
