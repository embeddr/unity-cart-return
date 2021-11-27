// Simple text fader behavior

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextFader : MonoBehaviour
{
    [Tooltip("Delay before beginning fade in seconds")]
    [SerializeField]
    private float _fadeDelay = 1.0F;

    [Tooltip("Duration of fade in seconds")]
    [SerializeField]
    private float _fadeDuration = 1.0F;

    [Tooltip("Rise rate")]
    [SerializeField]
    private float _riseRate = 3.0F;

    private Text _text;
    private float _delayTime = 0.0F;
 
    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void Update()
    {
        // Raise position up at specified rate
        transform.position += Vector3.up * _riseRate * Time.deltaTime;

        // Fade out over specified duration after specified delay
        if ((_delayTime += Time.deltaTime) > _fadeDelay) {
            var newColor = _text.color;
            newColor.a -= 1.0F * (Time.deltaTime / _fadeDuration);
            if (newColor.a <= 0.0F) {
                Destroy(gameObject);
            } else {
                _text.color = newColor;
            }
        }
    }
}
