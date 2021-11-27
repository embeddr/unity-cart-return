using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayReturnIndicator : MonoBehaviour
{
    [Tooltip("Cart return indicator sound effect")]
    [SerializeField]
    private AudioSource _returnSound;
    private AudioSource _returnSoundAlt;

    [Tooltip("Cart return indicator TextFader object")]
    [SerializeField]
    private GameObject _textFader;

    [Tooltip("Canvas under which to create indicator text")]
    [SerializeField]
    private GameObject _canvas;

    private float _soundInterval = 0.1F;

    private float _soundTime = 0.0F;

    Queue<ReturnIndicator> _indicatorQueue = new Queue<ReturnIndicator>();
    struct ReturnIndicator {
        public int count;
        public CartType type;
    }

    void OnEnable()
    {
        GameData.OnReturnCountChange += UpdateIndicatorQueue;
    }

    void OnDisable()
    {
        GameData.OnReturnCountChange -= UpdateIndicatorQueue;
    }

    void UpdateIndicatorQueue(int newCount, CartType cartType)
    {
        var delta = GameData.getCartDelta(newCount, cartType);

        if (delta > 0) {
            var indicator = new ReturnIndicator();
            indicator.count = delta;
            indicator.type = cartType;

            _indicatorQueue.Enqueue(indicator);
        }
    }

    Color32 GetCartColor(CartType cartType)
    {
        Color32 color;
        switch (cartType) {
            case CartType.Red:
                color = new Color32(0xe9, 0x41, 0x41, 0xff); 
                break;
            case CartType.Blue:
                color = new Color32(0x2e, 0xa7, 0xe3, 0xff); 
                break;
            case CartType.Green:
                color = new Color32(0x6c, 0xdd, 0x73, 0xff); 
                break;
            case CartType.Normal:
            default: // intentional fall-through
                color = new Color32(0xeb, 0xeb, 0xeb, 0xff); 
                break;
        }
        return color;
    }

    void FixedUpdate()
    {
        if (_indicatorQueue.Count > 0) {
            if ((_soundTime += Time.fixedDeltaTime) > _soundInterval) {
                _soundTime = 0.0F;

                var indicatorObj = Instantiate(_textFader,
                                               GameData.BackCart.transform.position,
                                               _textFader.transform.rotation,
                                               _canvas.transform);

                var indicatorData = _indicatorQueue.Dequeue();
                var text = indicatorObj.GetComponent<Text>();
                text.text = "+" + indicatorData.count.ToString();
                text.color = GetCartColor(indicatorData.type);

                _returnSound.Play();
            }
        }
    }
}
