// Time keeping and display behavior

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayTime : MonoBehaviour
{
    [Tooltip("Time is running")]
    private bool timeEnabled = true;

    private Text _text;

    private float _time; // [sec]

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void OnEnable()
    {
       CartObstacleCollision.OnCollision += PauseTime; 
    }

    void OnDisable()
    {
       CartObstacleCollision.OnCollision -= PauseTime; 
    }

    void PauseTime()
    {
        timeEnabled = false;
    }

    void Update()
    {
        if (timeEnabled) {
            _time += Time.deltaTime;

            // Display minutes, seconds, and centiseconds (milliseconds is a bit excessive)
            int min = (int)_time / 60;
            int sec = (int)_time % 60;
            int cs = (int)(_time * 100) % 100;
            _text.text = string.Format("{0,2:D2}m {1,2:D2}.{2,2:D2}s", min, sec, cs);
        }
    }
}
