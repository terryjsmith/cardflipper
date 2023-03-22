using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGenerator : MonoBehaviour
{
    [SerializeField]
    List<Sprite> sprites;

    [SerializeField]
    GameObject prefab;

    bool playing = false;

    // Start is called before the first frame update
    void Start()
    {
        PopulateCards();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space) && playing == false)
        {
            PopulateCards();
            return;
        }

        // Check for another card in the flipped state
        GameObject[] list = GameObject.FindGameObjectsWithTag("Card");
        int flippedCounter = 0;
        for (int i = 0; i < list.Length; i++)
        {
            Card otherCard = list[i].GetComponent<Card>();
            if (otherCard.flipped || otherCard.stayFlipped) flippedCounter++;
        }

        if (flippedCounter == 16)
        {
            // Show game over message
            for (int i = 0; i < list.Length; i++)
            {
                Destroy(list[i]);

                GameObject gameObject = GameObject.Find("WinMessage");
                gameObject.GetComponent<Text>().enabled = true;
                playing = false;
            }
        }
    }

    // Randomly distribute cards
    void PopulateCards()
    {
        // Remove any old cards
        GameObject[] list = GameObject.FindGameObjectsWithTag("Card");
        for(int i = 0; i < list.Length; i++)
        {
            Destroy(list[i]);
        }

        // Generate 8 random cards, removing the options from the set as we go
        List<Sprite> selectedSprites = new List<Sprite>();
        List<Sprite> remaining = new List<Sprite>(sprites);
        for(int i = 0; i < 8; i++)
        {
            int index = Random.Range(0, remaining.Count - 1);
            selectedSprites.Add(remaining[index]);
            selectedSprites.Add(remaining[index]); // Add it twice so we can just use it twice
            remaining.RemoveAt(index);
        }

        GameObject camera = GameObject.Find("Main Camera");
        Camera c = camera.GetComponent<Camera>();

        // Randomly distribute those cards to places on the "map"
        for(int y = 0; y < 4; y++)
        {
            for(int x = 0; x < 4; x++)
            {
                // Pick a random card from our selected list
                int index = Random.Range(0, selectedSprites.Count - 1);

                // Screen coords
                Vector2 screenCenter = c.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
                float screenX = screenCenter.x - 4.0f + (x * 2.5f);
                float screenY = screenCenter.y - 4.0f + (y * 2.6f);

                GameObject card = GameObject.Instantiate(prefab, new Vector3(screenX, screenY, 0), Quaternion.identity);

                card.GetComponent<Card>().sprite = selectedSprites[index];
                selectedSprites.RemoveAt(index);
            }
        }

        playing = true;
    }
}
