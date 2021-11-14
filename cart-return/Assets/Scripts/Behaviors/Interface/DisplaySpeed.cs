// Scroll speed display behavior

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplaySpeed : MonoBehaviour
{
    private Text _text;

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void Update()
    {
        _text.text = string.Format("Speed: {0:0.0}", GameData.ScrollSpeed);
    }
}
