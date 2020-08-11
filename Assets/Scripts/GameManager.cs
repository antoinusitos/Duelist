using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Player myPlayer1 = null;
    private Player myPlayer2 = null;

    private Card myPlayer1PickedCard = null;
    private Card myPlayer2PickedCard = null;

    [SerializeField]
    private Text myPlayerTurnText = null;

    [SerializeField]
    private Text myChoice1TypeText = null;
    [SerializeField]
    private Text myChoice1ValueText = null;

    [SerializeField]
    private Button myChoice1Button = null;

    [SerializeField]
    private Text myChoice2TypeText = null;
    [SerializeField]
    private Text myChoice2ValueText = null;

    [SerializeField]
    private Button myChoice2Button = null;

    private bool myCardSelected = false;

    private bool myGameEnded = false;

    [SerializeField]
    private Text myResolutionText = null;

    [SerializeField]
    private Text myPlayer1HealthText = null;

    [SerializeField]
    private Text myPlayer2HealthText = null;

    [SerializeField]
    private Transform[] myPositions = null;

    [SerializeField]
    private Transform myPlayer1Image = null;
    private int myPlayer1Position = 0;

    [SerializeField]
    private Transform myPlayer2Image = null;
    private int myPlayer2Position = 0;

    private bool myIsInResolution = false;

    private void Start()
    {
        myPlayer1 = new Player();
        myPlayer2 = new Player();

        StartCoroutine("GameLoop");
    }

    private void Update()
    {
        myPlayer1HealthText.text = "Player 1 health : " + GetPlayerHealth(1);
        myPlayer2HealthText.text = "Player 2 health : " + GetPlayerHealth(2);

        if(Input.GetKeyDown(KeyCode.P))
        {
            myPlayer1.DeckToString();
            myPlayer2.DeckToString();
        }
    }

    private IEnumerator GameLoop()
    {
        myPlayer1.InitPlayer();
        myPlayer2.InitPlayer();

        yield return new WaitForSeconds(0.5f);

        myPlayer1Position = 1;
        myPlayer2Position = 5;

        myPlayer1Image.position = myPositions[myPlayer1Position].position;
        myPlayer2Image.position = myPositions[myPlayer2Position].position;

        while (!myGameEnded)
        {
            for (int i = 1; i <= 2; i++)
            {
                myCardSelected = false;

                myPlayerTurnText.text = "Player " + i + " turn";

                Card[] playerCards = null;
                if(i == 1)
                    playerCards = myPlayer1.GetChoices();
                else
                    playerCards = myPlayer2.GetChoices();

                yield return new WaitForSeconds(2f);

                myPlayerTurnText.text = "";

                myChoice1Button.gameObject.SetActive(true);
                myChoice2Button.gameObject.SetActive(true);

                myChoice1TypeText.text = playerCards[0].GetCardType().ToString();
                myChoice1ValueText.text = playerCards[0].GetValue().ToString();
                myChoice1Button.onClick.AddListener(delegate { SelectCard(playerCards[0], i); });

                myChoice2TypeText.text = playerCards[1].GetCardType().ToString();
                myChoice2ValueText.text = playerCards[1].GetValue().ToString();
                myChoice2Button.onClick.AddListener(delegate { SelectCard(playerCards[1], i); });

                while (!myCardSelected)
                {
                    yield return null;
                }
            }

            myPlayerTurnText.text = "RESOLUTION...";
            myResolutionText.text = "Player 1 took :" + myPlayer1PickedCard.GetCardType() + " of " + myPlayer1PickedCard.GetValue() + "\n" +
                                    "Player 2 took :" + myPlayer2PickedCard.GetCardType() + " of " + myPlayer2PickedCard.GetValue();

            StartCoroutine(Resolution());

            while(myIsInResolution)
            {
                yield return null;
            }

            myPlayer1.MoveUsedCard(myPlayer1PickedCard);
            myPlayer2.MoveUsedCard(myPlayer2PickedCard);

            yield return new WaitForSeconds(1f);
        }
    }

    public void SelectCard(Card aCard, int aPlayerNumber)
    {
        myCardSelected = true;

        myChoice1Button.gameObject.SetActive(false);
        myChoice2Button.gameObject.SetActive(false);

        if (aPlayerNumber == 1)
            myPlayer1PickedCard = aCard;
        else
            myPlayer2PickedCard = aCard;
    }

    private IEnumerator Resolution()
    {
        myIsInResolution = true;

        myResolutionText.text = "";

        Card localCard1 = myPlayer1PickedCard;
        Card localCard2 = myPlayer2PickedCard;

        if (localCard1.GetCardType() == localCard2.GetCardType() &&
            (localCard1.GetCardType() != CardType.MOVEMENTLEFT || localCard1.GetCardType() != CardType.MOVEMENTRIGHT)
        )
        {
            myResolutionText.text = "DUCE !";
            yield return new WaitForSeconds(0.5f);
            myResolutionText.text = "";
        }
        else
        {
            while (localCard1.GetCurrentValue() > 0 || localCard2.GetCurrentValue() > 0)
            {
                if (localCard1.GetCurrentValue() > 0)
                {
                    localCard1.Use();

                    switch (localCard1.GetCardType())
                    {
                        case CardType.SWORD:
                            {
                                if (myPlayer1Position + 1 < myPlayer2Position)
                                {
                                    if (localCard1.GetUsed())
                                    {
                                        //continue bounce
                                        if (myPlayer1Position > 0)
                                            myPlayer1Position--;
                                    }
                                    else
                                    {
                                        //move toward target
                                        myPlayer1Position++;
                                    }
                                }
                                else if (myPlayer1Position + 1 == myPlayer2Position)
                                {
                                    //can attack
                                    if (!localCard1.GetUsed())
                                    {
                                        localCard1.SetUsed(true);
                                        //safe
                                        if (localCard2.GetCardType() == CardType.SHIELD && localCard2.GetCurrentValue() >= localCard1.GetCurrentValue())
                                        {

                                        }
                                        //attack
                                        else
                                        {
                                            myPlayer2.TakeHit();
                                            CheckHealth();
                                        }
                                    }
                                    else
                                    {
                                        //bounce
                                        if(myPlayer1Position > 0)
                                            myPlayer1Position--;
                                    }
                                }
                                break;
                            }
                        case CardType.MOVEMENTRIGHT:
                            {
                                if (myPlayer1Position + 1 <= myPlayer2Position)
                                {
                                    myPlayer1Position++;
                                }
                                break;
                            }
                        case CardType.MOVEMENTLEFT:
                            {
                                if (myPlayer1Position - 1 >= 0)
                                {
                                    myPlayer1Position--;
                                }
                                break;
                            }
                    }
                }
                if (localCard2.GetCurrentValue() > 0)
                {
                    localCard2.Use();
                    switch (localCard2.GetCardType())
                    {
                        case CardType.SWORD:
                            {
                                if (myPlayer2Position - 1 > myPlayer1Position)
                                {
                                    if (localCard2.GetUsed())
                                    {
                                        //continue bounce
                                        if (myPlayer2Position < 6)
                                            myPlayer2Position++;
                                    }
                                    else
                                    {
                                        //move toward target
                                        myPlayer2Position--;
                                    }
                                }
                                else if (myPlayer2Position - 1 == myPlayer1Position)
                                {
                                    //can attack
                                    if (!localCard2.GetUsed())
                                    {
                                        localCard2.SetUsed(true);
                                        //safe
                                        if (localCard1.GetCardType() == CardType.SHIELD && localCard1.GetCurrentValue() >= localCard2.GetCurrentValue())
                                        {

                                        }
                                        //attack
                                        else
                                        {
                                            myPlayer1.TakeHit();
                                            CheckHealth();
                                        }
                                    }
                                    else
                                    {
                                        //bounce
                                        if (myPlayer2Position < 6)
                                            myPlayer2Position++;
                                    }
                                }
                                break;
                            }
                        case CardType.MOVEMENTRIGHT:
                            {
                                if (myPlayer2Position < 6)
                                {
                                    myPlayer2Position++;
                                }
                                break;
                            }
                        case CardType.MOVEMENTLEFT:
                            {
                                if (myPlayer2Position - 1 > myPlayer1Position)
                                {
                                    myPlayer2Position--;
                                }
                                break;
                            }
                    }
                }

                if(myPlayer1Position == myPlayer2Position)
                {
                    if (myPlayer1Position > 0)
                        myPlayer1Position--;
                    if (myPlayer2Position < 6)
                        myPlayer2Position++;
                }

                myPlayer1Image.position = myPositions[myPlayer1Position].position;
                myPlayer2Image.position = myPositions[myPlayer2Position].position;

                yield return new WaitForSeconds(0.5f);
            }
        }
        localCard1.ResetUsage();
        localCard2.ResetUsage();
        myIsInResolution = false;
    }

    private bool CheckHealth()
    {
        if(myPlayer1.GetHealth() <= 0)
        {
            Debug.Log("Player 1 lost");
            myPlayerTurnText.text = "Player 1 Lost !";
            myGameEnded = true;
            return true;
        }
        else if (myPlayer2.GetHealth() <= 0)
        {
            Debug.Log("Player 2 lost");
            myPlayerTurnText.text = "Player 2 Lost !";
            myGameEnded = true;
            return true;
        }

        return false;
    }

    private int GetPlayerHealth(int aPlayer)
    {
        if(aPlayer == 1 && myPlayer1 != null)
        {
            return myPlayer1.GetHealth();
        }
        else if (aPlayer == 2 && myPlayer2 != null)
        {
            return myPlayer2.GetHealth();
        }
        return 0;
    }
}
