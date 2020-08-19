using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private static GameSettings myInstance = null;

    private string myPlayer1Deck = "-1";
    private string myPlayer2Deck = "-1";

    private void Awake()
    {
        myInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static GameSettings GetInstance()
    {
        return myInstance;
    }

    public void SetPlayerDeck(int aPlayer, string aDeck)
    {
        if (aPlayer == 1)
            myPlayer1Deck = aDeck;
        else
            myPlayer2Deck = aDeck;
    }

    public string GetPlayerDeck(int aPlayer)
    {
        if (aPlayer == 1)
            return myPlayer1Deck;
        else
            return myPlayer2Deck;
    }
}
