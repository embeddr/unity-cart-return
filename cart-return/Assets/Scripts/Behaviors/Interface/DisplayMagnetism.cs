// Magnetism time display behavior

using UnityEngine;
public class DisplayMagnetism : MonoBehaviour
{
    // Max magnetism time
    const float _timeMax = 18.0F;

    // Max bar x-axis scale
    const float _scaleXMax = 113.0F;

    // Current scale of the magnetism indicator bar
    float _scale;

    void Awake()
    {
        _scale = calcScale();
    }

    void Update()
    {
        // Set transform x-axis scale according to available magnetism time
        var targetScaleX = calcScale();
        transform.localScale = new Vector3(targetScaleX,
                                           transform.localScale.y,
                                           transform.localScale.z);
    }

    float calcScale()
    {
        var timeRatio = Mathf.Clamp((GameData.MagnetismTime / _timeMax), 0.0F, 1.0F);
        return Mathf.Lerp(0.0F, _scaleXMax, timeRatio);
    }
}
