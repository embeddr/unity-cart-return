// Game Over display behavior

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayGameOver : MonoBehaviour
{
    private Text _text;

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void OnEnable()
    {
        GameData.OnGameStateChange += UpdateDisplay;
    }

    void OnDisable()
    {
       GameData.OnGameStateChange -= UpdateDisplay;
    }

    void UpdateDisplay(GameState newGameState)
    {
        if (newGameState == GameState.GameOver) {
            _text.enabled = true;
        } else {
            _text.enabled = false;
        }
    }
}
