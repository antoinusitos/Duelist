using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class NetworkController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputField myInputField = null;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server !");
        PhotonNetwork.AutomaticallySyncScene = true;
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
