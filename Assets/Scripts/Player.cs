using UnityEngine;

public class Player
{
    private Card[] myDeck = null;

    private Card[] myChoices = null;

    private int myDeckIndex = 0;

    private int myHealth = 3;

    public void InitPlayer()
    {
        myHealth = 3;
        myChoices = new Card[2];
        myDeck = new Card[10];
        DEBUGFILLDECK();
    }

    private void DEBUGFILLDECK()
    {
        for(int i = 0; i < myDeck.Length; i++)
        {
            CardType cardType = (CardType)Random.Range(0, 4);
            int value = Random.Range(1, 6);
            myDeck[i] = new Card();
            myDeck[i].InitCard(cardType, value, value);
        }
    }

    public Card[] GetChoices()
    {
        myChoices[0] = myDeck[0];
        myChoices[1] = myDeck[1];

        return myChoices;
    }

    public void MoveUsedCard(Card aCard)
    {
        int indexCard = 0;
        for(int i = 0; i < 2; i++)
        {
            if(myDeck[i] == aCard)
            {
                indexCard = i;
            }
        }
        for (int i = indexCard; i < myDeck.Length - 1; i++)
        {
            myDeck[i] = myDeck[i + 1];
        }
        myDeck[myDeck.Length - 1] = aCard;
    }

    public void TakeHit()
    {
        myHealth--;
    }

    public int GetHealth()
    {
        return myHealth;
    }

    public void DeckToString()
    {
        Debug.Log("Player Deck :");
        for (int i = 0; i < myDeck.Length; i++)
        {
            Debug.Log(i + ":"); myDeck[i].CardToString();
        }
    }
}
