using UnityEngine;

public class Card
{
    private CardType myCardType = CardType.MOVEMENTRIGHT;

    private int myValue = 0;
    private int myCurrentValue = 0;
    private int myCost = 0;
    private bool myUsed = false;

    public void InitCard(CardType aCardType, int aValue, int aCost)
    {
        myCardType = aCardType;
        myValue = aValue;
        myCost = aCost;
        myCurrentValue = myValue;

        if (myCardType == CardType.BOW || myCardType == CardType.SPELL)
            myCurrentValue = 1;
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
        Debug.Log(myCardType + " for " + myValue);
    }
}
