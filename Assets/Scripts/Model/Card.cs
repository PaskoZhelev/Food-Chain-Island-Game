using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    
    private bool isMoving;
    // whether it has eaten some cards already
    public bool stacked;
    public int number;
    private Vector3 initialPosition;

    [HideInInspector]
    public Space occupiedSpace;

    private GameObject currentCollidedTileObj;


    void Start()
    {
        initialPosition = rectTransform.anchoredPosition;
        stacked = false;
        currentImage = GetComponent<Image>();
    }

    public void InitializeCard(Space occupiedSpace, int number, Sprite sprite)
    {
        setOccupiedSpace(occupiedSpace);
        setImage(sprite);
        this.number = number;
    }

    public void setOccupiedSpace(Space occupiedSpace)
    {
        this.occupiedSpace = occupiedSpace;
        occupiedSpace.setCard(this);
    }

    public void setImage(Sprite sprite)
    {
        currentImage.sprite = sprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameHandler.Instance.setClickedCardImage(this);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = rectTransform.anchoredPosition;
        isMoving = true;
        // for showing the rune above all the others
        transform.SetAsLastSibling();
    }

    // Drag the selected item.
    public void OnDrag(PointerEventData data)
    {
        if (isMoving)
        {
            transform.position = data.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isMoving = false;

        if (null != currentCollidedTileObj)
        {
            Space collidedSpace = currentCollidedTileObj.GetComponent<Space>();
            ButtonMode currentButtonMode = GameHandler.Instance.currentSelectedButton.mode;

            // if Space is EMPTY
            if (null == collidedSpace.card)
            {
                Debug.Log("Replace Empty Space");
                replaceEmptySpace(collidedSpace);

                currentCollidedTileObj = null;
                return;
            } else
            {
                if(ButtonMode.EATING == currentButtonMode)
                {
                    collidedSpace.card.uiTweener.AnimateReverseScaling(0.45f);
                    replaceEmptySpace(collidedSpace);
                    stacked = true;
                    GameHandler.Instance.clickedCardStatus.text = "Stacked";

                    currentCollidedTileObj = null;
                } 
                else if(ButtonMode.BE_EATEN == currentButtonMode)
                {
                    Space currSpace = occupiedSpace;
                    //uiTweener.AnimateMovement(rectTransform.anchoredPosition, collidedSpace.rectTransform.anchoredPosition);
                    uiTweener.AnimateReverseScaling();
                    collidedSpace.card.changeOpacity(1.0f);
                    collidedSpace.card.stacked = true;
                    currSpace.card = null;
                    GameHandler.Instance.ResetClickedImage();

                    return;
                }
                // ButtonMode.SWAP
                else
                {
                    Debug.Log("Swap Tile");
                    collidedSpace.card.changeOpacity(1.0f);
                    replaceWithTile(collidedSpace.card);

                    currentCollidedTileObj = null;
                    return;
                }
            }

        }

        currentCollidedTileObj = null;
        uiTweener.AnimateMovement(rectTransform.anchoredPosition, initialPosition);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (isMoving && occupiedSpace.gameObject != collider.gameObject)
        {
            if (null == currentCollidedTileObj)
            {
                currentCollidedTileObj = collider.gameObject;
                Space collidedSpace = currentCollidedTileObj.GetComponent<Space>();
                if(null != collidedSpace.card)
                {
                    collidedSpace.card.changeOpacity(0.5f);
                } else
                {
                    collidedSpace.EnableHighlightedImage();
                }
                
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (isMoving && null != currentCollidedTileObj)
        {
            Space collidedSpace = currentCollidedTileObj.GetComponent<Space>();
            if (null != collidedSpace.card)
            {
                collidedSpace.card.changeOpacity(1.0f);
            } else
            {
                collidedSpace.DisableHighlightedImage();
            }

            currentCollidedTileObj = null;
        }
    }

    public void changeOpacity(float opacityValue)
    {
        Color c = currentImage.color;
        c.a = opacityValue;
        currentImage.color = c;
    }

    public void replaceEmptySpace(Space collidedSpace)
    {
        Space currSpace = occupiedSpace;
        occupyNewSpace(collidedSpace.rectTransform.anchoredPosition, collidedSpace);
        currSpace.card = null;
        collidedSpace.DisableHighlightedImage();
    }

    public void replaceWithTile(Card collidedCard)
    {
        Vector3 currInitialPosition = initialPosition;
        Space currSpace = occupiedSpace;
        occupyNewSpace(collidedCard.initialPosition, collidedCard.occupiedSpace);
        collidedCard.occupyNewSpace(currInitialPosition, currSpace);
    }

    public void occupyNewSpace(Vector3 newPosition, Space spaceToOccupy)
    {
        setOccupiedSpace(spaceToOccupy);
        uiTweener.AnimateMovement(rectTransform.anchoredPosition, newPosition);
        initialPosition = newPosition;
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

    private UITweener _uiTweener;
    [HideInInspector]
    public UITweener uiTweener
    {
        get
        {
            if (null == _uiTweener)
            {
                _uiTweener = GetComponent<UITweener>();
                return _uiTweener;
            }
            return _uiTweener;
        }

        set
        {
            _uiTweener = value;
        }
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
}
