using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private static GameSettings myInstance = null;

    private int myPlayer1Deck = -1;
    private int myPlayer2Deck = -1;

    private void Awake()
    {
        myInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static GameSettings GetInstance()
    {
        return myInstance;
    }

    public void SetPlayerDeck(int aPlayer, int aDeck)
    {
        if (aPlayer == 1)
            myPlayer1Deck = aDeck;
        else
            myPlayer2Deck = aDeck;
    }

    public int GetPlayerDeck(int aPlayer)
    {
        if (aPlayer == 1)
            return myPlayer1Deck;
        else
            return myPlayer2Deck;
    }
}
