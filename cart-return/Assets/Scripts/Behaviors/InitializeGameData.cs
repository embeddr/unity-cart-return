// Game data initialization behavior
//
// This script initializes the static game data with data from the provided game config
// asset, as well as any other data required from the scene itself.

using UnityEngine;

public class InitializeGameData : MonoBehaviour
{
    [Tooltip("Game configuration asset to load")]
    [SerializeField]
    private GameConfig _gameConfig;

    void Awake()
    {
        // For now, assume only one player cart in stack
        GameObject playerCart = GameObject.FindGameObjectWithTag("Player");
        var stackSize = 1;
        GameData.init(_gameConfig,
                      playerCart,
                      playerCart,
                      (uint)stackSize);
    }
}
