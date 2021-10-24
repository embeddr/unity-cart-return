// Nudge count display behavior

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayNudge : MonoBehaviour
{
    private Text _text;

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void Update()
    {
        _text.text = string.Format("Nudges: {0}", GameData.Nudges);
    }
}
