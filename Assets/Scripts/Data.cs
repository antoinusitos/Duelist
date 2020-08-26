using UnityEngine;

public enum CardType
{
    SHIELD,
    MOVEMENTLEFT,
    MOVEMENTRIGHT,
    SWORD,
    SPELL,
    BOW
}

public class Data
{
    public static Sprite GetSpriteOfType(CardType aCardType)
    {
        switch (aCardType)
        {
            case CardType.BOW:
                {
                    return Resources.Load<Sprite>("Textures/Bow");
                }
            case CardType.MOVEMENTLEFT:
                {
                    return Resources.Load<Sprite>("Textures/Run_L");
                }
            case CardType.MOVEMENTRIGHT:
                {
                    return Resources.Load<Sprite>("Textures/Run_R");
                }
            case CardType.SHIELD:
                {
                    return Resources.Load<Sprite>("Textures/Shield");
                }
            case CardType.SPELL:
                {
                    return Resources.Load<Sprite>("Textures/Spell");
                }
            case CardType.SWORD:
                {
                    return Resources.Load<Sprite>("Textures/Sword");
                }
        }
        return null;
    }
}
