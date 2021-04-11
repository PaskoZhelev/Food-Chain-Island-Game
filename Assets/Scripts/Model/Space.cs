using UnityEngine;
using UnityEngine.UI;

public class Space : MonoBehaviour
{

    [HideInInspector]
    public Card card;


    void Start()
    {

    }

    public void EnableHighlightedImage()
    {
        currentImage.sprite = UIHandler.Instance.highlightedSpaceSprite;
    }

    public void DisableHighlightedImage()
    {
        currentImage.sprite = UIHandler.Instance.normalSpaceSprite;
    }

    public void setCard(Card card)
    {
        this.card = card;
    }

    private RectTransform _rectTransform;
    [HideInInspector]
    public RectTransform rectTransform
    {
        get
        {
            if (null == _rectTransform)
            {
                _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
            return _rectTransform;
        }

        set
        {
            _rectTransform = value;
        }
    }

    private Image _currentImage;
    [HideInInspector]
    public Image currentImage
    {
        get
        {
            if (null == _currentImage)
            {
                _currentImage = GetComponent<Image>();
                return _currentImage;
            }
            return _currentImage;
        }

        set
        {
            _currentImage = value;
        }
    }

}
