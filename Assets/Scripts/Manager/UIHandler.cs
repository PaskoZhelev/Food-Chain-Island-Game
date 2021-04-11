using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
	public Sprite[] cardSprites;

	public Sprite highlightedSpaceSprite;
	public Sprite normalSpaceSprite;

	// Start is called before the first frame update
	void Start()
    {
        
    }

	/* SINGLETON */
	private static UIHandler instance;
	public static UIHandler Instance { get => instance; set => instance = value; }

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
