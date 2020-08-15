using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckSelection : MonoBehaviour
{
    private ButtonDeckSelection myPlayer1CurrentSelection = null;
    private ButtonDeckSelection myPlayer2CurrentSelection = null;

    public void SelectDeck(ButtonDeckSelection aButton)
    {
        if (aButton.GetTeam() == 1)
        {
            if(myPlayer1CurrentSelection != null)
            {
                myPlayer1CurrentSelection.ShowSelected(false);
            }
            myPlayer1CurrentSelection = aButton;
            myPlayer1CurrentSelection.ShowSelected(true);
            GameSettings.GetInstance().SetPlayerDeck(1, myPlayer1CurrentSelection.GetDeckSelection());
        }
        else
        {
            if (myPlayer2CurrentSelection != null)
            {
                myPlayer2CurrentSelection.ShowSelected(false);
            }
            myPlayer2CurrentSelection = aButton;
            myPlayer2CurrentSelection.ShowSelected(true);
            GameSettings.GetInstance().SetPlayerDeck(2, myPlayer2CurrentSelection.GetDeckSelection());
        }

        if(myPlayer1CurrentSelection != null && myPlayer2CurrentSelection != null)
        {
            SceneManager.LoadScene(2);
        }
    }
}
