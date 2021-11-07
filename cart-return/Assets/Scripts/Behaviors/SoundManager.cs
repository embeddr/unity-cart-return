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
    }

    void OnDisable()
    {
        GameData.OnGameStateChange -= UpdateStateSounds;
        GameData.OnScrollSpeedChange -= UpdateSpeedSounds;
        GameData.OnStackSizeChange -= UpdateStackSounds;
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
                ((newScrollSpeed - 10.0F) / 50.0F);
    }

    void UpdateStackSounds(uint newStackSize)
    {
        _stackedRollingSoundSource.volume = GameData.StackSize * 0.02F;
    }
}
