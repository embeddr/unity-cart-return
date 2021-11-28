// Magnetism time display behavior

using UnityEngine;
public class DisplayMagnetism : MonoBehaviour
{
    // Max magnetism time
    const float _timeMax = 18.0F;

    // Max bar scale
    const float _scaleMax = 2.0F;

    // Current scale of the magnetism indicator bar
    float _scale;

    void Awake()
    {
        _scale = calcScale();
    }

    void Update()
    {
        // Set transform x-axis scale according to available magnetism time
        var targetScale = calcScale();
        transform.localScale = new Vector3(targetScale, 1.0F, 1.0F);
    }

    float calcScale()
    {
        var timeRatio = Mathf.Clamp((GameData.MagnetismTime / _timeMax), 0.0F, 1.0F);
        return Mathf.Lerp(0.0F, _scaleMax, timeRatio);
    }
}
