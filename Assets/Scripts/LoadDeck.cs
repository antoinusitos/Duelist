using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadDeck : MonoBehaviour
{
    [SerializeField]
    private Button myPrefabDeckChoosing = null;

    [SerializeField]
    private Transform myButtonHandler = null;

    [SerializeField]
    private GameObject myEditingPanel = null;

    [SerializeField]
    private GameObject myChosingPanel = null;

    [SerializeField]
    private DeckBuilder myDeckBuilder = null;

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
            Button button = Instantiate(myPrefabDeckChoosing, myButtonHandler);
            button.transform.GetChild(0).GetComponent<Text>().text = file.Name;
            button.onClick.AddListener(delegate { OpenEdit(file); } );
        }
    }

    private void OpenEdit(FileInfo aFileInfo)
    {
        myEditingPanel.SetActive(true);
        myChosingPanel.SetActive(false);
        myDeckBuilder.Init(aFileInfo);
    }

    public void OpenEmpty()
    {
        myEditingPanel.SetActive(true);
        myChosingPanel.SetActive(false);
        myDeckBuilder.InitEmpty();
    }
}
