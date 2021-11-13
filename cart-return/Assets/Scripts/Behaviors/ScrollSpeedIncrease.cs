using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollSpeedIncrease : MonoBehaviour
{

    [Tooltip("Whether or not speed should increase")]
    public bool enableIncrease = true;

    [Tooltip("The interval at which to increase scroll speed, in seconds")]
    [SerializeField]
    private float _speedIncreaseInterval = 5.0F;

    [Tooltip("The amount to increase scroll speed at each interval")]
    [SerializeField]
    private float _speedIncreaseAmount = 1.0F;

    private float _timer = 0.0F;

    void OnEnable()
    {
        CartObstacleCollision.OnCollision += PauseIncrease;
    }

    void OnDisable()
    {
        CartObstacleCollision.OnCollision += PauseIncrease;
    }

    void PauseIncrease()
    {
        enableIncrease = false;
    }

    void Update()
    {
        _timer += Time.deltaTime;    
        if (_timer > _speedIncreaseInterval) {
            _timer = 0.0F;
            if (enableIncrease) {
                GameData.ScrollSpeed += _speedIncreaseAmount;
            }
        }
    }
}
