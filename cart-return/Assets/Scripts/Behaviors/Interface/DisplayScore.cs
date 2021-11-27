// Score display behavior

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayScore : MonoBehaviour
{
    private Text _text;
    
    private int _totalReturnCount;

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void Start()
    {
        _totalReturnCount = GameData.ReturnCountNormal +
                            GameData.ReturnCountRed +
                            GameData.ReturnCountBlue +
                            GameData.ReturnCountGreen;
        DisplayReturnCount();
    }

    void OnEnable()
    {
        GameData.OnReturnCountChange += UpdateScore;
    }

    void OnDisable()
    {
        GameData.OnReturnCountChange -= UpdateScore;
    }

    void UpdateScore(int newCount, CartType cartType)
    {
        _totalReturnCount += GameData.getCartDelta(newCount, cartType);
        DisplayReturnCount();
    }

    void DisplayReturnCount()
    {
        _text.text = "Returned: " + _totalReturnCount.ToString("0");
    }
}
