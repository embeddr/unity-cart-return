// Score keeping and display behavior

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayScore : MonoBehaviour
{
    [Tooltip("Multiplier for point accumulation")]
    public float multiplier = 1.0F;

    [Tooltip("Score is running")]
    private bool scoreEnabled = true;

    [SerializeField]
    [Tooltip("Baseline points per second")]
    private float _pointsPerSecond = 10.0F;

    private Text _text;
    private double _points = 0.0F;

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void OnEnable()
    {
       CartObstacleCollision.OnCollision += PauseScore; 
    }

    void OnDisable()
    {
       CartObstacleCollision.OnCollision -= PauseScore; 
    }

    void PauseScore()
    {
        scoreEnabled = false;
    }

    void Update()
    {
        if (scoreEnabled) {
            _points += (Time.deltaTime * _pointsPerSecond * multiplier);
            _text.text = _points.ToString("0");
        }
    }
}
