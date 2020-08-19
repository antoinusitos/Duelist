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
        switch(myCard.GetCardType())
        {
            case CardType.BOW:
                {
                    myImage.sprite = Resources.Load<Sprite>("Textures/Bow");
                    break;
                }
            case CardType.MOVEMENTLEFT:
                {
                    myImage.sprite = Resources.Load<Sprite>("Textures/Run_L");
                    break;
                }
            case CardType.MOVEMENTRIGHT:
                {
                    myImage.sprite = Resources.Load<Sprite>("Textures/Run_R");
                    break;
                }
            case CardType.SHIELD:
                {
                    myImage.sprite = Resources.Load<Sprite>("Textures/Shield");
                    break;
                }
            case CardType.SPELL:
                {
                    myImage.sprite = Resources.Load<Sprite>("Textures/Spell");
                    break;
                }
            case CardType.SWORD:
                {
                    myImage.sprite = Resources.Load<Sprite>("Textures/Sword");
                    break;
                }
        }
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
