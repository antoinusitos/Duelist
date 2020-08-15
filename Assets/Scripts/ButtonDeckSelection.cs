using UnityEngine;
using UnityEngine.UI;

public class ButtonDeckSelection : MonoBehaviour
{
    [SerializeField]
    private int myPlayer = 1;
    [SerializeField]
    private int myDeckSelection = 1;

    private Image myImage = null;

    private void Awake()
    {
        myImage = GetComponent<Image>();
    }

    public void ShowSelected(bool aNewState)
    {
        if(aNewState)
            myImage.color = Color.red;
        else
            myImage.color = Color.white;
    }

    public int GetTeam()
    {
        return myPlayer;
    }

    public int GetDeckSelection()
    {
        return myDeckSelection;
    }
}
