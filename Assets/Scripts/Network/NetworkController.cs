using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class NetworkController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputField myInputField = null;

    [SerializeField]
    private Button myStartButton = null;

    private bool myIsConnected = false;

    private void Start()
    {
        myStartButton.interactable = false;
        if(!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server !");
        PhotonNetwork.AutomaticallySyncScene = true;
        myIsConnected = true;
    }

    private void Update()
    {
        if(myIsConnected && !myStartButton.interactable && myInputField.text != string.Empty)
        {
            myStartButton.interactable = true;
        }
        else if(myIsConnected && myStartButton.interactable && myInputField.text == string.Empty)
        {
            myStartButton.interactable = false;
        }
    }

    public void QuickStart()
    {
        PhotonNetwork.LocalPlayer.NickName = myInputField.text;

        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Joining Random Room");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join Random Room");
        CreateRoom();
    }

    private void CreateRoom()
    {
        Debug.Log("Creating new room");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)2 };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
        Debug.Log(randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room.. trying again");
        CreateRoom();
    }

    public void QuickCancel()
    {
        PhotonNetwork.LeaveRoom();
    }
}
