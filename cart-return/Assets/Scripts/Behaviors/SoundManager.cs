using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Tooltip("Audio source for rolling cart sound effect")]
    [SerializeField]
    private AudioSource _rollingSoundSource;

    [Tooltip("Audio source for collision sound effect")]
    [SerializeField]
    private AudioSource _collisionSoundSource;

    void OnEnable()
    {
        GameData.OnGameStateChange += UpdateSounds;
    }

    void OnDisable()
    {
        GameData.OnGameStateChange -= UpdateSounds;
    }

    void UpdateSounds(GameState newGameState)
    {
        switch (newGameState) {
            case GameState.Paused:
                _rollingSoundSource.Stop();
                break;
            case GameState.InGame:
                _rollingSoundSource.Play();
                _collisionSoundSource.Stop();
                break;
            case GameState.GameOver:
                _rollingSoundSource.Stop();
                _collisionSoundSource.Play();
                break;
        }
    }
}
