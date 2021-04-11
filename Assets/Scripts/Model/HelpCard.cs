using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HelpCard : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    public bool clicked;
    public bool used;

    public GameObject cross;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameHandler.Instance.setClickedCardImage(currentImage.sprite);
    }

    float lastTimeClick = 0f;
    // Double Tile
    public void OnPointerClick(PointerEventData eventData)
    {

        float currentTimeClick = eventData.clickTime;
        if (Mathf.Abs(currentTimeClick - lastTimeClick) < 0.3f)
        {
            Debug.Log("Double Click");
            if(used)
            {
                disableCross();
            } else
            {
                enableCross();
            }
        }
        lastTimeClick = currentTimeClick;
    }

    private void enableCross()
    {
        cross.SetActive(true);
        clicked = false;
        used = true;
    }

    private void disableCross()
    {
        cross.SetActive(false);
        clicked = false;
        used = false;
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
