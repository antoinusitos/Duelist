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
    public static byte ReadyEventCode = 1;
    public static byte CardEventCode = 2;
    public static byte SelectionEventCode = 3;
    public static byte PickedEventCode = 4;
    public static byte HidePickedEventCode = 5;
    public static byte UpdateClientValuesEventCode = 6;
    public static byte LoserEventCode = 7;
    public static byte ClientLeaveRoom = 8;
    public static byte ClientUpdateResolutionText = 9;
    public static byte ClientShowSide = 10;
    public static byte ClientSendPlayersName = 11;

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
