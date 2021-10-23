using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    [Tooltip("The interval at which to increase scroll speed, in seconds")]
    [SerializeField]
    private float _scrollSpeedIncreaseInterval = 10.0F;

    [Tooltip("The amount to increase scroll speed at each interval")]
    [SerializeField]
    private float _scrollSpeedIncreaseAmount = 1.0F;

    private float _timer = 0.0F;

    void Update()
    {
        _timer += Time.deltaTime;    
        if (_timer > _scrollSpeedIncreaseInterval) {
            _timer = 0.0F;
            GameData.ScrollSpeed += _scrollSpeedIncreaseAmount;
        }
    }
}
