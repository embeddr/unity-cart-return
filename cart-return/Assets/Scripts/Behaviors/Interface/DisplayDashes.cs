// Dash count display behavior

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayDashes : MonoBehaviour
{
    private Text _text;

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void Update()
    {
        _text.text = string.Format("Dashes: {0}", GameData.Dashes);
    }
}
