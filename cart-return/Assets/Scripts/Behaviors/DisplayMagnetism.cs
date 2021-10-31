// Magnetism time display behavior

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayMagnetism : MonoBehaviour
{
    private Text _text;

    void Start()
    {
        _text = GetComponent<Text>();
    }

    void Update()
    {
        
        _text.text = string.Format("Magnetism: {0:0.00}", GameData.MagnetismTime);
    }
}
