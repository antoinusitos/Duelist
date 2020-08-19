using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckSelection : MonoBehaviour
{
    private ButtonDeckSelection myPlayer1CurrentSelection = null;
    private ButtonDeckSelection myPlayer2CurrentSelection = null;

    [SerializeField]
    private ButtonDeckSelection myButtonDeckSelectionPrefab = null;

    [SerializeField]
    private Transform myPlayer1Panel = null;
    [SerializeField]
    private Transform myPlayer2Panel = null;
    private void Start()
    {
        DirectoryInfo info = new DirectoryInfo("Assets/Resources/Deck/");
        FileInfo[] fileInfo = info.GetFiles();
        for (int i = 1; i < 3; i++)
        {
            foreach (FileInfo file in fileInfo)
            {
                if (file.Name.Contains(".meta"))
                    continue;
                Transform panel = null;
                if (i == 1)
                    panel = myPlayer1Panel;
                else
                    panel = myPlayer2Panel;
                ButtonDeckSelection button = Instantiate(myButtonDeckSelectionPrefab, panel);
                string name = file.Name.Split('.')[0];
                button.transform.GetChild(0).GetComponent<Text>().text = name;
                button.SetTeam(i);
                button.SetDeckSelection(name);
                button.GetComponent<Button>().onClick.AddListener(delegate { SelectDeck(button); });
            }
        }
    }

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
