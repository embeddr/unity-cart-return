// Score display behavior

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayScore : MonoBehaviour
{
    private Text _text;

    void Awake()
    {
        _text = GetComponent<Text>();
        UpdateScore(GameData.ReturnCountTotal);
    }

    void OnEnable()
    {
        GameData.OnReturnCountChange += UpdateScore;
    }

    void OnDisable()
    {
        GameData.OnReturnCountChange -= UpdateScore;
    }

    void UpdateScore(uint newTotalCount)
    {
        _text.text = "Returned: " + newTotalCount.ToString("0");
    }
}
