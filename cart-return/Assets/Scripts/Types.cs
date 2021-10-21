// General types (that don't fit in a more specific location)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum for compile-time checking of tag usage
// Must be manually maintained to match tags defined in Unity
public enum Tags 
{
    MainCamera,
    Player,
    Obstacle,
}

// Game state
// Should be manually maintained to match action maps defined in Unity
public enum GameState {
    // TODO: Add Warmup, ReturnCart?
    InGame,
    Paused,
    GameOver,
}
