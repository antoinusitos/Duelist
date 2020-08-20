using UnityEngine;

public class Card
{
    private CardType myCardType = CardType.MOVEMENTRIGHT;

    private int myValue = 0;
    private int myCurrentValue = 0;
    private int myCost = 0;
    private bool myUsed = false;

    public void InitCard(CardType aCardType, int aValue)
    {
        myCardType = aCardType;
        myValue = aValue;
        myCurrentValue = myValue;

        if (myCardType == CardType.BOW)
            myCurrentValue = 1;

        switch (aCardType)
        {
            case CardType.MOVEMENTLEFT:
                {
                    myCost = myValue;
                    break;
                }
            case CardType.MOVEMENTRIGHT:
                {
                    myCost = myValue;
                    break;
                }
            case CardType.SHIELD:
                {
                    myCost = myValue;
                    break;
                }
            case CardType.SWORD:
                {
                    myCost = 4 * myValue;
                    break;
                }
            case CardType.SPELL:
                {
                    myCost = 4 * myValue;
                    break;
                }
            case CardType.BOW:
                {
                    myCost = 15;
                    break;
                }
        }
    }

    public void ResetUsage()
    {
        myCurrentValue = myValue;
        myUsed = false;
    }

    public CardType GetCardType()
    {
        return myCardType;
    }

    public int GetValue()
    {
        return myValue;
    }

    public int GetCurrentValue()
    {
        return myCurrentValue;
    }

    public int GetCost()
    {
        return myCost;
    }

    public void Use()
    {
        myCurrentValue--;
    }

    public void SetUsed(bool aNewState)
    {
        myUsed = aNewState;
    }

    public bool GetUsed()
    {
        return myUsed;
    }

    public void CardToString()
    {
        Debug.Log(myCardType + " for " + myValue + " of cost : " + myCost);
    }
}
