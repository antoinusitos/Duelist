using UnityEngine;
using UnityEngine.UI;

public class DeckBuilder : MonoBehaviour
{
    [SerializeField]
    private Slider mySlider = null;

    private int myCurrentDeckValue = 0;

    private Card[] myDeck = null;

    private void Start()
    {
        myDeck = new Card[10];
    }
}
