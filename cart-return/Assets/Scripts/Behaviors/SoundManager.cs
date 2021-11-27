using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Tooltip("Audio source for rolling cart sound effect")]
    [SerializeField]
    private AudioSource _rollingSoundSource;

    [Tooltip("Audio source for stacked rolling cart sound effect")]
    [SerializeField]
    private AudioSource _stackedRollingSoundSource;

    [Tooltip("Audio source for collision sound effect")]
    [SerializeField]
    private AudioSource _collisionSoundSource;

    [Tooltip("Audio source for cart return sound effect")]
    [SerializeField]
    private AudioSource _cartReturnSoundSource;

    private float _initialPitch;

    void Awake()
    {
        _initialPitch = _rollingSoundSource.pitch;
    }

    void OnEnable()
    {
        GameData.OnGameStateChange += UpdateStateSounds;
        GameData.OnScrollSpeedChange += UpdateSpeedSounds;
        GameData.OnStackSizeChange += UpdateStackSounds;
        GameData.OnReturnCountChange += PlayCartReturnSound;
    }

    void OnDisable()
    {
        GameData.OnGameStateChange -= UpdateStateSounds;
        GameData.OnScrollSpeedChange -= UpdateSpeedSounds;
        GameData.OnStackSizeChange -= UpdateStackSounds;
        GameData.OnReturnCountChange -= PlayCartReturnSound;
    }

    void UpdateStateSounds(GameState newGameState)
    {
        switch (newGameState) {
            case GameState.Paused:
                _rollingSoundSource.Stop();
                _stackedRollingSoundSource.Stop();
                break;
            case GameState.InGame:
                _rollingSoundSource.Play();
                _stackedRollingSoundSource.Play();
                _collisionSoundSource.Stop();
                break;
            case GameState.GameOver:
                _rollingSoundSource.Stop();
                _stackedRollingSoundSource.Stop();
                _collisionSoundSource.Play();
                break;
        }
    }

    void UpdateSpeedSounds(float newScrollSpeed)
    {
        // Simple approach to increase pitch according to speed
        _rollingSoundSource.pitch = _initialPitch +
                ((newScrollSpeed - 10.0F) / 25.0F);
        _stackedRollingSoundSource.pitch = _initialPitch +
                ((newScrollSpeed - 10.0F) / 25.0F);
    }

    void UpdateStackSounds(uint newStackSize)
    {
        // Incrtacked rolling cart sound as stack size grows
        _stackedRollingSoundSource.volume = newStackSize * 0.04F;
    }

    void PlayCartReturnSound(uint newTotalCount)
    {
        // Play on increment
        if (newTotalCount > GameData.ReturnCountTotal) {
            _cartReturnSoundSource.Play();
        }
    }
}
