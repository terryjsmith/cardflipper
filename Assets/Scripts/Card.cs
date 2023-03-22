using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Sprite sprite;
    
    [SerializeField]
    public Sprite cardSprite;

    public bool flipped;

    public float flipBackCounter;

    public bool stayFlipped;

    private bool clicked;

    // Start is called before the first frame update
    void Start()
    {
        flipped = false;
        flipBackCounter = -1.0f;
        stayFlipped = false;
        clicked = false;
    }

    private void OnMouseDown()
    {
        clicked = true;
        Flip();
    }

    public void Flip()
    {
        if (stayFlipped == true) return;

        // Flip this card
        if (flipped == false)
        {
            GetComponent<SpriteRenderer>().sprite = sprite;
            flipped = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = cardSprite;
            flipped = false;
        }

        if (clicked == false) return;
        clicked = false;

        // Check for another card in the flipped state
        GameObject[] list = GameObject.FindGameObjectsWithTag("Card");
        int flippedCounter = 0;
        for(int i = 0; i < list.Length; i++)
        {
            Card otherCard = list[i].GetComponent<Card>();
            if (otherCard.flipped || otherCard.stayFlipped) flippedCounter++;

            // If this card is the card we're analyzing, move on
            if (list[i] == this.gameObject) continue;

            // Otherwise, if we have another flipped card, check for a pair
            if (otherCard.flipped == true)
            {
                if (otherCard.sprite == this.sprite)
                {
                    // We have a match, mark both as permanently flipped
                    otherCard.stayFlipped = true;
                    otherCard.flipped = false;
                    this.stayFlipped = true;
                    this.flipped = false;
                    Debug.Log("Stay flipped.");
                }
                else
                {
                    // Set flip timer for both cards
                    otherCard.flipBackCounter = 2.0f;
                    this.flipBackCounter = 2.0f;
                    Debug.Log("Flip back.");
                }
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(flipBackCounter > 0)
        {
            if(flipBackCounter - Time.deltaTime <= 0)
            {
                // flip back
                Flip();
                flipBackCounter = -1.0f;
            }
            else
            {
                // Decrement
                flipBackCounter -= Time.deltaTime;
            }
        }
    }
}
