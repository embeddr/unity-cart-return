using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [Tooltip("Multiplier for point accumulation")]
    public float multiplier = 1.0F;

    [Tooltip("Score is running")]
    public bool scoreEnabled = true;

    [SerializeField]
    [Tooltip("Baseline points per second")]
    private float _pointsPerSecond = 10.0F;

    private Text _text;
    private float _points = 0.0F;

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void OnEnable()
    {
       PlayerObstacleCollision.OnCollision += PauseScore; 
    }

    void OnDisable()
    {
       PlayerObstacleCollision.OnCollision -= PauseScore; 
    }

    void PauseScore()
    {
        scoreEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreEnabled) {
            _points += (Time.deltaTime * _pointsPerSecond * multiplier);
            _text.text = _points.ToString("0");
        }
    }
}
