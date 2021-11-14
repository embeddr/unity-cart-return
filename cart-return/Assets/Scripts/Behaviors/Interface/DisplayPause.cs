// Pause display behavior

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayPause : MonoBehaviour
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
        if (newGameState == GameState.Paused) {
            _text.enabled = true;
        } else {
            _text.enabled = false;
        }
    }
}
