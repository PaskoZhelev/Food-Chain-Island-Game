using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
	public Space[] spaces;
	public Card cardPrefab;
	public GameObject cardsHolder;

	[HideInInspector]
	public Card clickedCard;
	public Image clickedCardImage;
	public Text clickedCardStatus;

	[HideInInspector]
	public List<Card> allCards;

	public GameButton[] allButtons;
	[HideInInspector]
	public GameButton currentSelectedButton;

	// Start is called before the first frame update
	void Start()
    {
		GenerateCards();
		// eat mode selected by default
		currentSelectedButton = allButtons[0];
	}

	public void GenerateCards()
    {
		List<int> randomizedCardNumbers = randomizeCardNumbers();
		foreach (var i in Constants.initialSpaceNumbers)
        {
			Space currSpace = spaces[i];
			Card card = Instantiate(cardPrefab, currSpace.transform.position, Quaternion.identity, cardsHolder.transform);

			int cardNum = removeAndGet(randomizedCardNumbers);
			Sprite sprite = UIHandler.Instance.cardSprites[cardNum];
			card.InitializeCard(currSpace, cardNum, sprite);
			currSpace.setCard(card);
			allCards.Add(card);
		}
    }

	public void setClickedCardImage(Card card)
    {
		clickedCard = card;
		clickedCardImage.sprite = card.currentImage.sprite;
		clickedCardStatus.text = clickedCard.stacked ? "Stacked" : "Unstacked";
	}

	public void setClickedCardImage(Sprite sprite)
	{
		clickedCardImage.sprite = sprite;
	}

	public void EatModeButtonClicked()
    {
		currentSelectedButton = allButtons[0];
		UnclickButtons();
	}

	public void BeEatenModeButtonClicked()
	{
		currentSelectedButton = allButtons[1];
		UnclickButtons();
	}

	public void SwapModeButtonClicked()
	{
		currentSelectedButton = allButtons[2];
		UnclickButtons();
	}

	private void UnclickButtons()
    {
        foreach (var btn in allButtons)
        {
			if(currentSelectedButton == btn)
            {
				currentSelectedButton.ClickButton();
				continue;
            }

			btn.UnclickButton();
		}
    }

	public void DiscardButtonClicked()
	{
		if(null != clickedCard)
        {
			clickedCard.occupiedSpace.card = null;
			clickedCard.uiTweener.AnimateReverseScaling();
			ResetClickedImage();
		}
	}

	public void ResetClickedImage()
    {
		clickedCard = null;
		clickedCardImage.sprite = UIHandler.Instance.highlightedSpaceSprite;
		clickedCardStatus.text = "";
	}

	private int removeAndGet(List<int> numbers)
    {
		int num = numbers[0];
		numbers.RemoveAt(0);
		return num;
    }

	private List<int> randomizeCardNumbers()
    {
		var cardNumbers = Constants.cardNumbers;
		return cardNumbers.OrderBy(a => Guid.NewGuid()).ToList();
	}

	/* SINGLETON */
	private static GameHandler instance;
	public static GameHandler Instance { get => instance; set => instance = value; }

	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;

		//DontDestroyOnLoad(gameObject);

	}
}
