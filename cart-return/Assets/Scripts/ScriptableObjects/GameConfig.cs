using UnityEngine;

// Game configuration data
[CreateAssetMenu(fileName = "GameConfig",
                 menuName = "ScriptableObjects/GameConfig",
                 order = 1)]
public class GameConfig : ScriptableObject
{
    [Tooltip("Initial game state")]
    public GameState state;

    [Tooltip("Initial scroll speed")]
    public float scrollSpeed;

    [Tooltip("Initial nudges available to player")]
    public uint nudges;
}
