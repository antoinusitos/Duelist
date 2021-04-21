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
        string path = "";

#if UNITY_EDITOR
        path = "Assets/Resources/Deck/";
#else
        path =  Application.dataPath + "/Resources/Deck/";
#endif

        DirectoryInfo info = new DirectoryInfo(path);
        FileInfo[] fileInfo = info.GetFiles();
        foreach (FileInfo file in fileInfo)
        {
            if (file.Name.Contains(".meta"))
                continue;
            Transform panel = null;
            panel = myPlayer1Panel;
            ButtonDeckSelection button = Instantiate(myButtonDeckSelectionPrefab, panel);
            string name = file.Name.Split('.')[0];
            button.transform.GetChild(0).GetComponent<Text>().text = name;
            button.SetTeam(1);
            button.SetDeckSelection(name);
            button.GetComponent<Button>().onClick.AddListener(delegate { SelectDeck(button);});
        }
    }

    public void SelectDeck(ButtonDeckSelection aButton)
    {
        /*if (aButton.GetTeam() == 1)
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
        }*/

        if (myPlayer1CurrentSelection != null)
        {
            myPlayer1CurrentSelection.ShowSelected(false);
        }
        myPlayer1CurrentSelection = aButton;
        myPlayer1CurrentSelection.ShowSelected(true);
        GameSettings.GetInstance().SetPlayerDeck(1, myPlayer1CurrentSelection.GetDeckSelection());

        int[] deck = new int[20];

        string[] lines = null;
#if UNITY_EDITOR
        lines = File.ReadAllLines("Assets/Resources/Deck/" + myPlayer1CurrentSelection.GetDeckSelection() + ".json");
#else
        lines = File.ReadAllLines(Application.dataPath + "/Resources/Deck/" + myPlayer1CurrentSelection.GetDeckSelection() + ".json");
#endif
        if (lines != null)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split('-');
                deck[i] = int.Parse(line[0]);
                deck[i + 1] = int.Parse(line[1]);
            }
        }

        FindObjectOfType<LobbyController>().SetReady(deck);

        if (myPlayer1CurrentSelection != null && myPlayer2CurrentSelection != null)
        {
            SceneManager.LoadScene(2);
        }
    }
}
