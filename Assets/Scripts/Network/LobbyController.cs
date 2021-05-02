using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField]
    private int multiplayerSceneIndex;

    [SerializeField]
    private Text myText = null;

    private int myPlayerReady = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CreatePlayer();
        List<string> names = new List<string>();
        foreach(KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            names.Add(player.Value.NickName);
        }
        myText.text = names[0] + " vs " + names[1];
    }

    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerSelection"), Vector3.zero, Quaternion.identity);
    }

    public void SetReady(int[] aDeck)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            AddReady(aDeck, true);
        }
        else
        {
            SendReady(aDeck);
        }
    }

    private void SendReady(int[] aDeck)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(Data.ReadyEventCode, aDeck, raiseEventOptions, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        byte eventCode = photonEvent.Code;

        if (eventCode == Data.ReadyEventCode)
        {
            object data = photonEvent.CustomData;

            int[] deck = (int[])data;

            AddReady(deck, false);
        }
    }

    private void AddReady(int[] aDeck, bool isMaster)
    {
        Debug.Log("Creating Player");
        GameObject playerObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayerMain"), Vector3.zero, Quaternion.identity);
        playerObj.transform.parent = transform;
        Player player = playerObj.GetComponent<Player>();
        player.InitPlayer(isMaster);
        player.SetDeck(aDeck);
        myPlayerReady++;
        if(myPlayerReady == 2)
        {
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }
    }
}
