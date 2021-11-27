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

    public delegate void StackSizeChangeHandler(int newStackSize);
    public static event StackSizeChangeHandler OnStackSizeChange;

    public delegate void ReturnCountChangeHandler(int newReturnCount,
                                                  CartType cartType);
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
    public static int Dashes { get; set; }

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
    public static int StackSize {
        get { return _stackSize; }
        set {
            OnStackSizeChange?.Invoke(value);
            _stackSize = value;
        }
    }
    private static int _stackSize;

    // Normal carts returned
    public static int ReturnCountNormal {
        get { return _returnCountNormal; }
        set {
            OnReturnCountChange?.Invoke(value, CartType.Normal);
            _returnCountNormal = value;
        }
    }
    private static int _returnCountNormal;

    // Red carts returned
    public static int ReturnCountRed {
        get { return _returnCountRed; }
        set {
            OnReturnCountChange?.Invoke(value, CartType.Red);
            _returnCountRed = value;
        }
    }
    private static int _returnCountRed;

    // Blue carts returned
    public static int ReturnCountBlue {
        get { return _returnCountBlue; }
        set {
            OnReturnCountChange?.Invoke(value, CartType.Blue);
            _returnCountBlue = value;
        }
    }
    private static int _returnCountBlue;

    // Green carts returned
    public static int ReturnCountGreen {
        get { return _returnCountGreen; }
        set {
            OnReturnCountChange?.Invoke(value, CartType.Green);
            _returnCountGreen = value;
        }
    }
    private static int _returnCountGreen;

    ///////////////////////////////////////////////////////////////////////////
    // Helper functions
    ///////////////////////////////////////////////////////////////////////////

    // Initialize the game data
    public static void init(GameConfig config,
                            GameObject frontCart,
                            GameObject backCart,
                            int stackSize)
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

    // TODO: Just send the delta as the event arg, instead of the new count
    public static int getCartDelta(int newCount, CartType cartType)
    {
        var prevCount = 0;
        switch (cartType) {
            case CartType.Normal:
                prevCount = ReturnCountNormal;
                break;
            case CartType.Red:
                prevCount = ReturnCountRed;
                break;
            case CartType.Blue:
                prevCount = ReturnCountBlue;
                break;
            case CartType.Green:
                prevCount = ReturnCountGreen;
                break;
        }

        return (newCount - prevCount);
    }
}
