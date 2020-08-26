using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IPointerClickHandler
{
    private Card myCard = null;

    [SerializeField]
    private Text myCost = null;
    [SerializeField]
    private Text myValue = null;
    [SerializeField]
    private Image myImage = null;

    private DeckBuilder myDeckBuilder = null;

    private int myIndex = -1;

    public void UpdateUI()
    {
        if (myCard == null)
            return;

        myCost.text = myCard.GetCost().ToString();
        myValue.text = myCard.GetCurrentValue().ToString();
        myImage.sprite = Data.GetSpriteOfType(myCard.GetCardType());
    }

    public void AssignCard(Card aCard, int anIndex)
    {
        myCard = aCard;
        myIndex = anIndex;
        UpdateUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (myDeckBuilder == null)
            myDeckBuilder = FindObjectOfType<DeckBuilder>();

        myDeckBuilder.FocusOnCard(myCard, myIndex);
    }
}
