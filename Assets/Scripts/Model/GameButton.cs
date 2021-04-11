using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour
{
    public ButtonMode mode;

    public Sprite clickedSprite;
    public Sprite unclickedSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ClickButton()
    {
        currentImage.sprite = clickedSprite;
    }

    public void UnclickButton()
    {
        currentImage.sprite = unclickedSprite;
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
