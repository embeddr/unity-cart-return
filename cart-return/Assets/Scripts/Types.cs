using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum for compile-time checking of tag usage
// Must be manually maintained to match tags defined in Unity
enum Tags 
{
    MainCamera,
    Player,
    Obstacle,
}

// Enum for compile-time checking of action maps
// Must be manually maintained to match action maps defined in Unity
enum ActionMaps
{
    InGame,
    Paused,
    GameOver,
}
