using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Card[] myDeck = null;

    private Card[] myChoices = null;

    private string myDeckIndex = "-1";

    private int myHealth = 3;

    private int myPlayerNumber = 0;

    private bool myIsMaster = false;

    private int myID = -1;

    private Photon.Realtime.Player myPhotonPlayer = null;

    public Player(int aPlayerNumber)
    {
        myPlayerNumber = aPlayerNumber;
    }

    public void SetPlayerNumber(int aPlayerNumber)
    {
        myPlayerNumber = aPlayerNumber;
    }

    public int GetPlayerNumber()
    {
        return myPlayerNumber;
    }

    public void SetPhotonPlayer(Photon.Realtime.Player aPlayer)
    {
        myPhotonPlayer = aPlayer;
    }

    public bool GetIsMaster()
    {
        return myIsMaster;
    }

    public void InitPlayer(bool isMaster)
    {
        myIsMaster = isMaster;
        myHealth = 3;
        myChoices = new Card[2];
        myDeck = new Card[10];
        //FillDeck();

        //CheckDeckValidity();

        //DEBUGFILLDECK();
    }

    public void SetDeck(int[] aDeck)
    {
        int deckIndex = 0;
        for (int i = 0; i < aDeck.Length; i += 2)
        {
            CardType cardType = (CardType)aDeck[i];
            int value = aDeck[i + 1];
            myDeck[deckIndex] = new Card();
            myDeck[deckIndex].InitCard(cardType, value);
            deckIndex++;
        }
    }

    public void ShuffleDeck()
    {
        for(int i = 0; i < myDeck.Length; i++)
        {
            Card temp = myDeck[i];
            int rand = Random.Range(0, myDeck.Length);
            myDeck[i] = myDeck[rand];
            myDeck[rand] = temp;
        }
    }

    private void FillDeck()
    {
        Debug.Log("Player "+ myPlayerNumber + " choose deck " + GameSettings.GetInstance().GetPlayerDeck(myPlayerNumber));

        myDeckIndex = GameSettings.GetInstance().GetPlayerDeck(myPlayerNumber);

        if(myDeckIndex == "-1")
        {
            CreateRandomDeck();
            return;
        }

        string[] lines = null;
#if UNITY_EDITOR
        lines = File.ReadAllLines("Assets/Resources/Deck/" + myDeckIndex + ".json");
#else
        lines = File.ReadAllLines(Application.dataPath + "/Resources/Deck/" + myDeckIndex + ".json");
#endif
        if (lines != null)
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

    private void CreateRandomDeck()
    {
        Debug.Log("Create Random Deck");

        List<Card> deck = new List<Card>();

        int total = 0;

        CardType cardType = CardType.BOW;
        int value = 1;
        Card card = new Card();
        deck.Add(card);
        card.InitCard(cardType, value);
        total += card.GetCost();

        cardType = CardType.MOVEMENTLEFT;
        value = Random.Range(1, 6);
        card = new Card();
        deck.Add(card);
        card.InitCard(cardType, value);
        total += card.GetCost();

        cardType = CardType.MOVEMENTRIGHT;
        value = Random.Range(1, 6);
        card = new Card();
        deck.Add(card);
        card.InitCard(cardType, value);
        total += card.GetCost();

        cardType = CardType.SHIELD;
        value = Random.Range(1, 6);
        card = new Card();
        deck.Add(card);
        card.InitCard(cardType, value);
        total += card.GetCost();

        cardType = CardType.SPELL;
        value = Random.Range(1, 6);
        card = new Card();
        deck.Add(card);
        card.InitCard(cardType, value);
        total += card.GetCost();

        cardType = CardType.SWORD;
        value = Random.Range(1, 6);
        card = new Card();
        deck.Add(card);
        card.InitCard(cardType, value);
        total += card.GetCost();

        int tries = 0;

        while(tries < 10 && deck.Count < 10)
        {
            tries++;
            cardType = (CardType)Random.Range(0, 6);
            value = Random.Range(1, 6);

            if (cardType == CardType.BOW && total + 15 > 100)
                continue;
            else if (cardType != CardType.BOW && total + value > 100)
                continue;

            card = new Card();
            deck.Add(card);
            card.InitCard(cardType, value);
            total += card.GetCost();
        }

        myDeck = new Card[deck.Count];
        for(int i = 0; i < myDeck.Length; i++)
        {
            myDeck[i] = deck[i];
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
        int total = 0;
        for (int i = 0; i < myDeck.Length; i++)
        {
            if(myDeck[i] != null)
                total += myDeck[i].GetCost();
        }
        Debug.Log("Deck is " + total + " points and " + myDeck.Length + " cards");
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

    public void SetHealth(int aHealth)
    {
        myHealth = aHealth;
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
