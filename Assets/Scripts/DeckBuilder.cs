using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class DeckBuilder : MonoBehaviour
{
    [SerializeField]
    private Slider mySlider = null;

    [SerializeField]
    private Text myText = null;

    private int myCurrentDeckValue = 0;

    private Card[] myDeck = null;

    [SerializeField]
    private CardUI[] myCardUIs = null;

    private bool myIsBusy = false;

    [SerializeField]
    private Image myInspectImage = null;

    [SerializeField]
    private Text myInspectCost = null;

    [SerializeField]
    private Text myInspectValue = null;

    [SerializeField]
    private Text myInspectName = null;

    [SerializeField]
    private GameObject myInspectPanel = null;

    private Card myFocusedCard = null;
    private int myFocusedIndex = -1;

    [SerializeField]
    private Slider myNewValueSlider = null;

    [SerializeField]
    private InputField myInputField = null;
    private string myDeckName = "";

    [SerializeField]
    private Button mySaveButton = null;

    [SerializeField]
    private GameObject myChooseSection = null;

    [SerializeField]
    private GameObject myEditSection = null;

    [SerializeField]
    private LoadDeck myLoadDeck = null;

    private void Start()
    {
        /* myDeck = new Card[10];
         for(int i = 0; i < myDeck.Length; i++)
         {
             myDeck[i] = new Card();
             myDeck[i].InitCard(CardType.MOVEMENTRIGHT, 1);
             myCardUIs[i].AssignCard(myDeck[i], i);
         }
         Calculate();*/
        myNewValueSlider.onValueChanged.AddListener(delegate { ChangeCurrentValue((int)myNewValueSlider.value); });
    }

    public void Init(FileInfo aFile)
    {
        StreamReader reader = null;

#if UNITY_EDITOR
        reader = new StreamReader("Assets/Resources/Deck/" + aFile.Name);
#else
        reader = new StreamReader(Application.dataPath + "/Resources/Deck/" + aFile.Name);
#endif

        List<string> lines = new List<string>();
        while (!reader.EndOfStream)
        {
            lines.Add(reader.ReadLine());
        }

        string name = aFile.Name.Split('.')[0];

        myInputField.text = name;
        myDeckName = name;

        myDeck = new Card[10];
        for (int i = 0; i < myDeck.Length; i++)
        {
            string line = lines[i];
            string[] parameters = line.Split('-');

            myDeck[i] = new Card();
            myDeck[i].InitCard((CardType)int.Parse(parameters[0]), int.Parse(parameters[1]));
        }
        RefreshUI();
        Calculate();
    }

    public void InitEmpty()
    {
        myInputField.text = "";
        myDeckName = "";

        myDeck = new Card[10];
        for (int i = 0; i < myDeck.Length; i++)
        {
            myDeck[i] = new Card();
            myDeck[i].InitCard(CardType.MOVEMENTRIGHT, 1);
        }
        RefreshUI();
        Calculate();
    }

    private void Calculate()
    {
        myCurrentDeckValue = 0;
        for (int i = 0; i < myDeck.Length; i++)
        {
            myCurrentDeckValue += myDeck[i].GetCost();
        }
        mySlider.value = myCurrentDeckValue / 100.0f;
        myText.text = myCurrentDeckValue + " / 100";
        if(myCurrentDeckValue > 100)
        {
            myText.color = Color.red;
            mySaveButton.interactable = false;
        }
        else
        {
            myText.color = Color.white;
            mySaveButton.interactable = true;
        }
    }

    public void ChangeCurrentValue(int aValue)
    {
        if (myFocusedCard == null)
            return;

        myFocusedCard.InitCard(myFocusedCard.GetCardType(), aValue);
        myInspectCost.text = myFocusedCard.GetCost().ToString();
        myInspectValue.text = myFocusedCard.GetCurrentValue().ToString();
        RefreshUI();
        Calculate();
    }

    public void FocusOnCard(Card aCard, int anIndex)
    {
        Debug.Log("focus on card " + anIndex);

        if (aCard == null || myIsBusy)
        {
            myIsBusy = false;
            myInspectPanel.SetActive(false);
            myFocusedCard = null;
            myFocusedIndex = -1;
            return;
        }

        myNewValueSlider.value = 1;

        myInspectPanel.SetActive(true);

        myIsBusy = true;

        myFocusedCard = aCard;
        myFocusedIndex = anIndex;

        myInspectName.text = aCard.GetCardType().ToString();
        ChangeImage();
        myInspectCost.text = aCard.GetCost().ToString();
        myInspectValue.text = aCard.GetCurrentValue().ToString();

        if (aCard.GetCardType() == CardType.BOW)
            myNewValueSlider.gameObject.SetActive(false);
        else
            myNewValueSlider.gameObject.SetActive(true);
    }

    private void ChangeImage()
    {
        myInspectImage.sprite = Data.GetSpriteOfType(myFocusedCard.GetCardType());
    }

    public void ChangeCardTo(int aType)
    {
        CardType type = (CardType)aType;
        myNewValueSlider.value = 1;
        myFocusedCard.InitCard(type, (int)myNewValueSlider.value);
        if (type == CardType.BOW)
            myNewValueSlider.gameObject.SetActive(false);
        else
            myNewValueSlider.gameObject.SetActive(true);
            myInspectCost.text = myFocusedCard.GetCost().ToString();
        myInspectValue.text = myFocusedCard.GetCurrentValue().ToString();
        ChangeImage();
        Calculate();
        RefreshUI();
    }

    private void RefreshUI()
    {
        for (int i = 0; i < myDeck.Length; i++)
        {
            myCardUIs[i].AssignCard(myDeck[i], i);
        }
    }

    public void Save()
    {
        myDeckName = myInputField.text;

        if (myDeckName == "")
            return;

        if (myCurrentDeckValue > 100)
            return;

        string path = "";


#if UNITY_EDITOR
        Directory.CreateDirectory("Assets/Resources/Deck/");
        path = "Assets/Resources/Deck/" + myDeckName + ".json";
#else
        Directory.CreateDirectory(Application.dataPath + "/Resources/Deck/");
        path = Application.dataPath + "/Resources/Deck/" + myDeckName + ".json";
#endif

        StreamWriter writer = new StreamWriter(path, false);
        for (int i = 0; i < myDeck.Length; i++)
        {
            writer.WriteLine((int)myDeck[i].GetCardType() + "-" + myDeck[i].GetCurrentValue());
        }
        writer.Close();

        Debug.Log("File saved");

#if UNITY_EDITOR
        AssetDatabase.ImportAsset("Assets/Resources/Deck/" + myDeckName + ".json");
#endif

        myEditSection.SetActive(false);
        myChooseSection.SetActive(true);
        myLoadDeck.Clean();
    }
}
