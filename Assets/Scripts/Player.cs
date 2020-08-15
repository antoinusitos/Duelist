using System.IO;
using UnityEngine;

public class Player
{
    private Card[] myDeck = null;

    private Card[] myChoices = null;

    private int myDeckIndex = 0;

    private int myHealth = 3;

    private int myPlayerNumber = 0;

    public Player(int aPlayerNumber)
    {
        myPlayerNumber = aPlayerNumber;
    }

    public void InitPlayer()
    {
        myHealth = 3;
        myChoices = new Card[2];
        myDeck = new Card[10];
        FillDeck();

        CheckDeckValidity();

        //DEBUGFILLDECK();
    }

    private void FillDeck()
    {
        Debug.Log("Player "+ myPlayerNumber + " choose deck " + GameSettings.GetInstance().GetPlayerDeck(myPlayerNumber));
        string[] lines = File.ReadAllLines("Assets/Resources/Deck" + GameSettings.GetInstance().GetPlayerDeck(myPlayerNumber)+".json");
        if(lines != null)
        {
            for(int i = 0; i < lines.Length; i++)
            {
                Debug.Log(lines[i]);
                string[] line = lines[i].Split('-');
                CardType cardType = (CardType)int.Parse(line[0]);
                int value = int.Parse(line[1]);
                myDeck[i] = new Card();
                myDeck[i].InitCard(cardType, value);
            }
        }
    }

    private void DEBUGFILLDECK()
    {
        for(int i = 0; i < myDeck.Length; i++)
        {
            CardType cardType = (CardType)Random.Range(0, 4);
            int value = Random.Range(1, 6);
            myDeck[i] = new Card();
            myDeck[i].InitCard(cardType, value);
        }
    }

    private void CheckDeckValidity()
    {
        //TODO : Add all card values to see if we are under 100 points
        int total = 0;
        for (int i = 0; i < myDeck.Length; i++)
        {
            if(myDeck[i] != null)
                total += myDeck[i].GetCost();
        }
        Debug.Log("Deck is " + total + " points");
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
