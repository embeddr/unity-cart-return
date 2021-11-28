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
        if (GameData.Dashes <= 9) {
            _text.text = string.Format("{0}", GameData.Dashes);
        } else {
            _text.text = ">9";
        }
    }
}
