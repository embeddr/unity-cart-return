using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Tooltip("Audio source for rolling cart sound effect")]
    [SerializeField]
    private AudioSource _rollingSoundSource;

    [Tooltip("Audio source for collision sound effect")]
    [SerializeField]
    private AudioSource _collisionSoundSource;

    private float _initialPitch;

    void Awake()
    {
        _initialPitch = _rollingSoundSource.pitch;
    }

    void OnEnable()
    {
        GameData.OnGameStateChange += UpdateStateSounds;
        GameData.OnScrollSpeedChange += UpdateSpeedSounds;
    }

    void OnDisable()
    {
        GameData.OnGameStateChange -= UpdateStateSounds;
        GameData.OnScrollSpeedChange -= UpdateSpeedSounds;
    }

    void UpdateStateSounds(GameState newGameState)
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

    void UpdateSpeedSounds(float newScrollSpeed)
    {
        _rollingSoundSource.pitch = _initialPitch +
                ((newScrollSpeed - 10.0F) / 50.0F);
    }
}
