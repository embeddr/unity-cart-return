// Game data initializer behavior
//
// This script initializes the static game data with data from the scene.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameDataInit : MonoBehaviour
{
    [Tooltip("Initial scroll speed")]
    [SerializeField]
    private float _initScrollSpeed = 10.0F;

    [Tooltip("Initial game state")]
    [SerializeField]
    private GameState _initGameState = GameState.InGame;

    void Awake()
    {
        var init_data = new GameData.InitData();

        init_data.PlayerInput = GetComponent<PlayerInput>();
        init_data.ScrollSpeed = _initScrollSpeed;
        init_data.GameState = _initGameState;

        GameData.init(init_data);
    }

}
