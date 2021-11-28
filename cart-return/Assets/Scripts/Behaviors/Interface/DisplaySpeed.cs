// Scroll speed display behavior
//
// Manipulates the angle of this object's transform to present a speedometer needle effect.

using UnityEngine;

public class DisplaySpeed : MonoBehaviour
{
    // Min/max needle angles [deg]
    const float _angleMin = 120.0F;
    const float _angleMax = -120.0F;

    // Min/max speed thresholds
    const float _speedMin = 8.0F;
    const float _speedMax = 18.0F;

    // Angle rate limit [deg/s]
    const float _angleRateMax = 40.0F;

    // Current needle angle (used for rate limiting)
    float _angle;

    void Awake()
    {
        _angle = calcAngle();
    }

    void Update()
    {
        // Set rotation according to current speed, with rate limiting
        var angleTarget = calcAngle();
        var maxDelta = _angleRateMax * Time.deltaTime;
        _angle = Mathf.Clamp(angleTarget, _angle - maxDelta, _angle + maxDelta);
        transform.eulerAngles = new Vector3(0.0F, 0.0F, _angle);
    }

    float calcAngle() {
        var speedRatio = (GameData.ScrollSpeed - _speedMin) / (_speedMax - _speedMin);
        return Mathf.Lerp(_angleMin, _angleMax, speedRatio);
    }
}
