using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private Player myPlayer1 = null;
    private Player myPlayer2 = null;

    private Card myPlayer1PickedCard = null;
    private Card myPlayer2PickedCard = null;

    [SerializeField]
    private GameObject myPlayer1ShowSide = null;
    [SerializeField]
    private GameObject myPlayer2ShowSide = null;

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

    [SerializeField]
    private GameObject myPlayer1Card = null;
    [SerializeField]
    private GameObject myPlayer2Card = null;

    [SerializeField]
    private Image myPlayer1Choice = null;
    [SerializeField]
    private Text myPlayer1ChoiceValue = null;
    [SerializeField]
    private Image myPlayer2Choice = null;
    [SerializeField]
    private Text myPlayer2ChoiceValue = null;

    private int myPlayer1Selection = -1;
    private int myPlayer2Selection = -1;

    private int myPlayer1ClientHealth = 0;
    private int myPlayer2ClientHealth = 0;

    private string myPlayer1Name = "Player 1";
    private string myPlayer2Name = "Player 2";

    private void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("IsMaster");
            StartCoroutine("GameLoop");
        }
        else
        {
            Debug.Log("NotMaster");
        }
    }

    private void Update()
    {
        myPlayer1HealthText.text = "<b>" + myPlayer1Name +" -</b> health : " + GetPlayerHealth(1);
        myPlayer2HealthText.text = "<b>" + myPlayer2Name + " -</b> health : " + GetPlayerHealth(2);

        if(Input.GetKeyDown(KeyCode.P))
        {
            myPlayer1.DeckToString();
            myPlayer2.DeckToString();
        }
    }

    /////////////////////////////////////////////////
    ///NETWORKING
    /////////////////////////////////////////////////

    private void SendChoices(int[] someCards)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(Data.CardEventCode, someCards, raiseEventOptions, SendOptions.SendReliable);
    }

    private void SendSelectedToMaster(int aSelection)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(Data.SelectionEventCode, aSelection, raiseEventOptions, SendOptions.SendReliable);
    }

    private void ShowClientSide(int aSide)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(Data.ClientShowSide, aSide, raiseEventOptions, SendOptions.SendReliable);
    }

    private void SendHidePickedCardsClient()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(Data.HidePickedEventCode, null, raiseEventOptions, SendOptions.SendReliable);
    }

    private void SendClientPlayersName()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        string[] toSend = new string[] { myPlayer1Name, myPlayer2Name };
        PhotonNetwork.RaiseEvent(Data.ClientSendPlayersName, toSend, raiseEventOptions, SendOptions.SendReliable);
    }

    private void SendClientUpdateValues()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        int[] toSend = new int[] { myPlayer1Position, myPlayer2Position, myPlayer1.GetHealth(), myPlayer2.GetHealth() };
        PhotonNetwork.RaiseEvent(Data.UpdateClientValuesEventCode, toSend, raiseEventOptions, SendOptions.SendReliable);
    }

    private void SendResolutionToClient(string aResolution)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(Data.ClientUpdateResolutionText, aResolution, raiseEventOptions, SendOptions.SendReliable);
    }

    private void ClientLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void ShowPickedCardsToClient()
    {
        int[] toSend = new int[] { (int)myPlayer1PickedCard.GetCardType(), myPlayer1PickedCard.GetValue(), (int)myPlayer2PickedCard.GetCardType(), myPlayer2PickedCard.GetValue() };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(Data.PickedEventCode, toSend, raiseEventOptions, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == Data.CardEventCode)
        {
            if (PhotonNetwork.IsMasterClient)
                return;

            object data = photonEvent.CustomData;

            int[] cards = (int[])data;

            ShowChoices(cards, false);
        }
        else if (eventCode == Data.SelectionEventCode)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            object data = photonEvent.CustomData;

            int card = (int)data;

            myPlayer2Selection = card;
        }
        else if (eventCode == Data.PickedEventCode)
        {
            if (PhotonNetwork.IsMasterClient)
                return;

            object data = photonEvent.CustomData;

            int[] cards = (int[])data;

            myPlayer1PickedCard = new Card();
            myPlayer1PickedCard.InitCard((CardType)cards[0], cards[1]);
            myPlayer2PickedCard = new Card();
            myPlayer2PickedCard.InitCard((CardType)cards[2], cards[3]);

            ShowPickedCards();
        }
        else if(eventCode == Data.HidePickedEventCode)
        {
            if(PhotonNetwork.IsMasterClient)
                return;

            HidePickedCards();
        }
        else if(eventCode == Data.UpdateClientValuesEventCode)
        {
            if (PhotonNetwork.IsMasterClient)
                return;

            object data = photonEvent.CustomData;

            int[] values = (int[])data;

            UpdateClientValues(values);
        }
        else if (eventCode == Data.LoserEventCode)
        {
            if (PhotonNetwork.IsMasterClient)
                return;

            object data = photonEvent.CustomData;

            int value = (int)data;

            ClientReceiveLoser(value);
        }
        else if (eventCode == Data.ClientLeaveRoom)
        {
            if (PhotonNetwork.IsMasterClient)
                return;

            ClientLeaveRoom();
        }
        else if(eventCode == Data.ClientUpdateResolutionText)
        {
            if (PhotonNetwork.IsMasterClient)
                return;

            object data = photonEvent.CustomData;

            string value = (string)data;

            myResolutionText.text = value;
        }
        else if(eventCode == Data.ClientShowSide)
        {
            if (PhotonNetwork.IsMasterClient)
                return;

            object data = photonEvent.CustomData;

            int value = (int)data;

            if (value == 1)
                myPlayer1ShowSide.SetActive(true);
            else
                myPlayer2ShowSide.SetActive(true);
        }
        else if(eventCode == Data.ClientSendPlayersName)
        {
            if (PhotonNetwork.IsMasterClient)
                return;

            object data = photonEvent.CustomData;

            string[] values = (string[])data;

            myPlayer1Name = values[0];
            myPlayer2Name = values[1];
        }
    }

    /////////////////////////////////////////////////
    ///NETWORKING
    /////////////////////////////////////////////////

    private void ShowChoices(int[] cards, bool isMaster)
    {
        Debug.Log("cards 1:" + cards[0] + " value : " + cards[1]);
        Debug.Log("cards 2:" + cards[2] + " value : " + cards[3]);

        myChoice1Button.gameObject.SetActive(true);
        myChoice2Button.gameObject.SetActive(true);

        myChoice1TypeText.text = ((CardType)cards[0]).ToString();
        myChoice1ValueText.text = cards[1].ToString();
        if (isMaster)
        {
            myChoice1Button.onClick.AddListener(delegate { 
                SelectCard(0, 1);
                myChoice1Button.gameObject.SetActive(false);
                myChoice2Button.gameObject.SetActive(false);
            });
        }
        else
        {
            //send the selected card
            myChoice1Button.onClick.AddListener(delegate { 
                SendSelectedToMaster(0);
                myChoice1Button.gameObject.SetActive(false);
                myChoice2Button.gameObject.SetActive(false);
            });
        }

        myChoice2TypeText.text = ((CardType)cards[2]).ToString();
        myChoice2ValueText.text = cards[3].ToString();
        if (isMaster)
        {
            myChoice2Button.onClick.AddListener(delegate { 
                SelectCard(1, 1);
                myChoice1Button.gameObject.SetActive(false);
                myChoice2Button.gameObject.SetActive(false);
            });
        }
        else
        {
            //send the selected card
            myChoice2Button.onClick.AddListener(delegate { 
                SendSelectedToMaster(1);
                myChoice1Button.gameObject.SetActive(false);
                myChoice2Button.gameObject.SetActive(false);
            });
        }
    }

    private IEnumerator GameLoop()
    {
        Player[] players = null;

        while(players == null || players.Length < 2)
        {
            Debug.Log("Searching for players");
            players = FindObjectsOfType<Player>();
            yield return null;
        }

        if(players[0].GetIsMaster())
        {
            myPlayer1 = players[0];
            myPlayer2 = players[1];
            myPlayer1.SetPlayerNumber(2);
            myPlayer2.SetPlayerNumber(1);
            myPlayer1ShowSide.SetActive(true);
            foreach(KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
            {
                if(player.Value.IsMasterClient)
                {
                    myPlayer1Name = player.Value.NickName;
                    myPlayer1.SetPhotonPlayer(player.Value);
                }
                else
                {
                    myPlayer2Name = player.Value.NickName;
                    myPlayer2.SetPhotonPlayer(player.Value);
                }

            }
            ShowClientSide(2);
        }
        else
        {
            myPlayer1 = players[1];
            myPlayer2 = players[0];
            myPlayer1.SetPlayerNumber(1);
            myPlayer2.SetPlayerNumber(2);
            myPlayer2ShowSide.SetActive(true);
            foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
            {
                if (player.Value.IsMasterClient)
                {
                    myPlayer2Name = player.Value.NickName;
                    myPlayer2.SetPhotonPlayer(player.Value);
                }
                else
                {
                    myPlayer1Name = player.Value.NickName;
                    myPlayer1.SetPhotonPlayer(player.Value);
                }

            }
            ShowClientSide(1);
        }

        SendClientPlayersName();

        myPlayer1.ShuffleDeck();
        myPlayer2.ShuffleDeck();

        yield return new WaitForSeconds(0.5f);

        myPlayer1Position = 1;
        myPlayer2Position = 5;

        myPlayer1Image.position = myPositions[myPlayer1Position].position;
        myPlayer2Image.position = myPositions[myPlayer2Position].position;

        SendClientUpdateValues();

        while (!myGameEnded)
        {
            myPlayer1Selection = -1;
            myPlayer2Selection = -1;

            Card[] playerCards1 = myPlayer1.GetChoices();
            Card[] playerCards2 = myPlayer2.GetChoices();

            yield return new WaitForSeconds(2f);

            //master side
            ShowChoices(new int[] { (int)playerCards1[0].GetCardType(), playerCards1[0].GetValue(), (int)playerCards1[1].GetCardType(), playerCards1[1].GetValue() }, true);

            //Send to client
            SendChoices(new int[] { (int)playerCards2[0].GetCardType(), playerCards2[0].GetValue(), (int)playerCards2[1].GetCardType(), playerCards2[1].GetValue() });

            while (myPlayer1Selection == -1 || myPlayer2Selection == -1)
            {
                yield return null;
            }

            if(myPlayer1.GetPlayerNumber() == 2)
            {
                myPlayer1PickedCard = playerCards1[myPlayer1Selection];
                myPlayer2PickedCard = playerCards2[myPlayer2Selection];
            }
            else
            {
                myPlayer2PickedCard = playerCards1[myPlayer1Selection];
                myPlayer1PickedCard = playerCards2[myPlayer2Selection];
            }

            ShowPickedCards();

            ShowPickedCardsToClient();

            myResolutionText.text = "RESOLUTION...";
            SendResolutionToClient("RESOLUTION...");
            //myResolutionText.text = "Player 1 took :" + myPlayer1PickedCard.GetCardType() + " of " + myPlayer1PickedCard.GetValue() + "\n" +
            //"Player 2 took :" + myPlayer2PickedCard.GetCardType() + " of " + myPlayer2PickedCard.GetValue();
            Debug.Log("Player 1 took :" + myPlayer1PickedCard.GetCardType() + " of " + myPlayer1PickedCard.GetValue() + "\n" +
                                    "Player 2 took :" + myPlayer2PickedCard.GetCardType() + " of " + myPlayer2PickedCard.GetValue());

            yield return new WaitForSeconds(0.5f);

            StartCoroutine(ShowPickedCardsAnimation());

            StartCoroutine(Resolution());

            while(myIsInResolution)
            {
                yield return null;
            }

            myPlayer1Card.SetActive(false);
            myPlayer2Card.SetActive(false);

            SendHidePickedCardsClient();

            SendClientUpdateValues();

            myPlayer1.MoveUsedCard(myPlayer1PickedCard);
            myPlayer2.MoveUsedCard(myPlayer2PickedCard);

            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(5);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(Data.ClientLeaveRoom, null, raiseEventOptions, SendOptions.SendReliable);

        PhotonNetwork.LeaveRoom();
    }

    private void ShowPickedCards()
    {
        myPlayer1Card.transform.rotation = Quaternion.Euler(0, 90, 0);
        myPlayer2Card.transform.rotation = Quaternion.Euler(0, 90, 0);

        myPlayer1Card.SetActive(true);
        myPlayer2Card.SetActive(true);

        myPlayer1Choice.sprite = Data.GetSpriteOfType(myPlayer1PickedCard.GetCardType());
        myPlayer1ChoiceValue.text = myPlayer1PickedCard.GetValue().ToString();
        myPlayer2Choice.sprite = Data.GetSpriteOfType(myPlayer2PickedCard.GetCardType());
        myPlayer2ChoiceValue.text = myPlayer2PickedCard.GetValue().ToString();

        StartCoroutine(ShowPickedCardsAnimation());
    }

    private void HidePickedCards()
    {
        myPlayer1Card.SetActive(false);
        myPlayer2Card.SetActive(false);
    }

    private IEnumerator ShowPickedCardsAnimation()
    {
        float timer = 0;
        while (timer < 2)
        {
            myPlayer1Card.transform.rotation = Quaternion.Euler(0, timer * 360 * 2, 0);
            myPlayer2Card.transform.rotation = Quaternion.Euler(0, timer * 360 * 2, 0);
            //myPlayer1Card.transform.Rotate(Vector3.up, Time.deltaTime * 1000);
            //myPlayer2Card.transform.Rotate(Vector3.up, Time.deltaTime * 1000);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void SelectCard(int aCard, int aTeam)
    {
        if(aTeam == 1)
            myPlayer1Selection = aCard;
        else
            myPlayer2Selection = aCard;
    }

    private IEnumerator Resolution()
    {
        myIsInResolution = true;

        myResolutionText.text = "";
        SendResolutionToClient("");

        Card localCard1 = myPlayer1PickedCard;
        Card localCard2 = myPlayer2PickedCard;

        if (localCard1.GetCardType() == localCard2.GetCardType() &&
            (localCard1.GetCardType() != CardType.MOVEMENTLEFT && localCard1.GetCardType() != CardType.MOVEMENTRIGHT)
        )
        {
            myResolutionText.text = "DUCE !";
            SendResolutionToClient("DUCE !");
            yield return new WaitForSeconds(2.0f);
            myResolutionText.text = "";
            SendResolutionToClient("");
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
                        case CardType.BOW:
                            {
                                if (!localCard1.GetUsed())
                                {
                                    localCard1.SetUsed(true);
                                    //safe
                                    if (localCard2.GetCardType() == CardType.SHIELD)
                                    {

                                    }
                                    //attack
                                    else
                                    {
                                        myPlayer2.TakeHit();
                                        CheckHealth();
                                    }
                                }
                                break;
                            }
                        case CardType.SPELL:
                            {
                                if (!localCard1.GetUsed())
                                {
                                    localCard1.SetUsed(true);
                                    //safe
                                    if (localCard2.GetCardType() == CardType.MOVEMENTLEFT || localCard2.GetCardType() == CardType.MOVEMENTRIGHT ||
                                        myPlayer2Position - myPlayer1Position > localCard1.GetCurrentValue())
                                    {

                                    }
                                    //attack
                                    else
                                    {
                                        myPlayer2.TakeHit();
                                        CheckHealth();
                                    }
                                }
                                break;
                            }
                        case CardType.SHIELD:
                            {

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
                        case CardType.BOW:
                            {
                                if (!localCard2.GetUsed())
                                {
                                    localCard2.SetUsed(true);
                                    //safe
                                    if (localCard1.GetCardType() == CardType.SHIELD)
                                    {

                                    }
                                    //attack
                                    else
                                    {
                                        myPlayer1.TakeHit();
                                        CheckHealth();
                                    }
                                }
                                break;
                            }
                        case CardType.SPELL:
                            {
                                if (!localCard2.GetUsed())
                                {
                                    localCard2.SetUsed(true);
                                    //safe
                                    if (localCard1.GetCardType() == CardType.MOVEMENTLEFT || localCard1.GetCardType() == CardType.MOVEMENTRIGHT ||
                                        myPlayer2Position - myPlayer1Position > localCard2.GetCurrentValue())
                                    {

                                    }
                                    //attack
                                    else
                                    {
                                        myPlayer2.TakeHit();
                                        CheckHealth();
                                    }
                                }
                                break;
                            }
                        case CardType.SHIELD:
                            {

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

                SendClientUpdateValues();

                yield return new WaitForSeconds(2f);
            }
        }
        localCard1.ResetUsage();
        localCard2.ResetUsage();
        myIsInResolution = false;
    }

    private void UpdateClientValues(int[] someValues)
    {
        myPlayer1Position = someValues[0];
        myPlayer2Position = someValues[1];
        myPlayer1Image.position = myPositions[myPlayer1Position].position;
        myPlayer2Image.position = myPositions[myPlayer2Position].position;
        SetPlayerHealth(1, someValues[2]);
        SetPlayerHealth(2, someValues[3]);
    }

    private bool CheckHealth()
    {
        if(myPlayer1.GetHealth() <= 0)
        {
            Debug.Log("Player 1 lost");
            myPlayerTurnText.text = "Player 1 Lost !";
            myGameEnded = true;
            SendLoserToClient(1);
            return true;
        }
        else if (myPlayer2.GetHealth() <= 0)
        {
            Debug.Log("Player 2 lost");
            myPlayerTurnText.text = "Player 2 Lost !";
            myGameEnded = true;
            SendLoserToClient(2);
            return true;
        }

        return false;
    }

    private void SendLoserToClient(int aLoser)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(Data.LoserEventCode, aLoser, raiseEventOptions, SendOptions.SendReliable);
    }

    private void ClientReceiveLoser(int aLoser)
    {
        myPlayerTurnText.text = "Player " + aLoser + " Lost !";
    }

    private int GetPlayerHealth(int aPlayer)
    {
        if(aPlayer == 1)
        {
            if (myPlayer1 != null)
                return myPlayer1.GetHealth();
            else
                return myPlayer1ClientHealth;
        }
        else if (aPlayer == 2)
        {
            if(myPlayer2 != null)
                return myPlayer2.GetHealth();
            else
                return myPlayer2ClientHealth;
        }
        return 0;
    }

    private void SetPlayerHealth(int aPlayer, int aHealth)
    {
        if (aPlayer == 1)
        {
            if (myPlayer1 != null)
                myPlayer1.SetHealth(aHealth);
            else
                myPlayer1ClientHealth = aHealth;
        }
        else if (aPlayer == 2)
        {
            if(myPlayer2 != null)
                myPlayer2.SetHealth(aHealth);
            else
                myPlayer2ClientHealth = aHealth;
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
}
