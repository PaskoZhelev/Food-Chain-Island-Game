using UnityEngine;
using System.Collections;

public enum UIAnimationTypes
{
    Move,
    MoveAndFade,
    Scale,
    ScaleX,
    ScaleY,
    Fade
}

public class UITweener : MonoBehaviour
{
    public GameObject objectToAnimate;

    public UIAnimationTypes animationType = UIAnimationTypes.Scale;
    public LeanTweenType easeType = LeanTweenType.easeSpring;
    public float duration = 0.35f;
    public float delay = 0.05f;

    public bool loop;
    public bool pingpong;

    public bool startPositionOffset = true;
    public Vector3 from = new Vector3(0, 0, 0);
    public Vector3 to = new Vector3(1, 1, 1);

    private LTDescr tweenObject;

    public bool showOnEnable = false;
    public bool workOnDisable = false;

    void Start()
    {
        if (objectToAnimate == null)
        {
            objectToAnimate = gameObject;
        }
    }

    public void AnimateMovement(Vector3 fromMovement, Vector3 toMovement)
    {
        AnimateMovement(fromMovement, toMovement, delay);
    }

    public void AnimateMovement(Vector3 fromMovement, Vector3 toMovement, float delayTime)
    {
        AssignObjectToAnimate();
        RectTransform rectTransform = objectToAnimate.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = fromMovement;

        tweenObject = LeanTween.move(rectTransform, toMovement, duration);
        tweenObject.setDelay(delayTime);
        tweenObject.setEase(easeType);
    }

    public void AnimateScaling()
    {
        AssignObjectToAnimate();
        AnimateScale(from, to);
    }

    public void AnimateReverseScaling()
    {
        AssignObjectToAnimate();
        AnimateScale(to, from);
    }

    public void AnimateReverseScaling(float delayTime)
    {
        AssignObjectToAnimate();
        AnimateScale(to, from, delayTime);
    }

    private void AnimateScale(Vector3 fromScale, Vector3 toScale)
    {
        if (startPositionOffset)
        {
            objectToAnimate.GetComponent<RectTransform>().localScale = fromScale;
        }
        tweenObject = LeanTween.scale(objectToAnimate, toScale, duration);
        tweenObject.setDelay(delay);
        tweenObject.setEase(easeType);
    }

    private void AnimateScale(Vector3 fromScale, Vector3 toScale, float delayTime)
    {
        if (startPositionOffset)
        {
            objectToAnimate.GetComponent<RectTransform>().localScale = fromScale;
        }
        tweenObject = LeanTween.scale(objectToAnimate, toScale, duration);
        tweenObject.setDelay(delayTime);
        tweenObject.setEase(easeType);
    }

    public void OnEnable()
    {
        if (showOnEnable)
        {
            Show();
        }
    }

    public void OnDisable()
    {
        if (workOnDisable)
        {
            Disable();
        }
    }

    public void Show()
    {
        HandleTween();
    }

    public void HandleTween()
    {
        if (objectToAnimate == null)
        {
            objectToAnimate = gameObject;
        }

        switch (animationType)
        {
            case UIAnimationTypes.Fade:
                Fade();
                break;
            case UIAnimationTypes.Move:
                MoveAbsolute();
                break;
            case UIAnimationTypes.MoveAndFade:
                MoveAndFade();
                break;
            case UIAnimationTypes.Scale:
                Scale();
                break;
            case UIAnimationTypes.ScaleX:
                Scale();
                break;
            case UIAnimationTypes.ScaleY:
                Scale();
                break;
        }

        tweenObject.setDelay(delay);
        tweenObject.setEase(easeType);

        if (loop)
        {
            tweenObject.loopCount = int.MaxValue;
        }

        if (pingpong)
        {
            tweenObject.setLoopPingPong();
        }
    }

    private void Fade()
    {
        if (gameObject.GetComponent<CanvasGroup>() == null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }

        CanvasGroup canvasGroup = objectToAnimate.GetComponent<CanvasGroup>();
        if (startPositionOffset)
        {
            canvasGroup.alpha = from.x;
        }
        tweenObject = LeanTween.alphaCanvas(canvasGroup, to.x, duration);
    }

    private void MoveAbsolute()
    {
        RectTransform rectTransform = objectToAnimate.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = from;

        tweenObject = LeanTween.move(rectTransform, to, duration);
    }

    private void MoveAndFade()
    {
        if (gameObject.GetComponent<CanvasGroup>() == null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }

        CanvasGroup canvasGroup = objectToAnimate.GetComponent<CanvasGroup>();

        RectTransform rectTransform = objectToAnimate.GetComponent<RectTransform>();
        Vector3 initialPosition = rectTransform.anchoredPosition;
        Vector3 newPosition = new Vector3(initialPosition.x, initialPosition.y - 38f);
        canvasGroup.alpha = 0.0f;

        tweenObject = LeanTween.move(rectTransform, newPosition, duration);
        tweenObject = LeanTween.alphaCanvas(canvasGroup, 1.0f, duration);
    }

    private void Scale()
    {
        if (startPositionOffset)
        {
            objectToAnimate.GetComponent<RectTransform>().localScale = from;
        }
        tweenObject = LeanTween.scale(objectToAnimate, to, duration);
    }

    void SwapDirection()
    {
        var temp = from;
        from = to;
        to = temp;
    }

    public void Disable()
    {
        SwapDirection();

        HandleTween();

        tweenObject.setOnComplete(() =>
        {
            SwapDirection();
        });
    }

    private void AssignObjectToAnimate()
    {
        if (objectToAnimate == null)
        {
            objectToAnimate = gameObject;
        }
    }

}
