using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI connectionStatus;
    
    [SerializeField] private byte maxPlayersPerRoom = 4;
    
    private string gameVersion = "1";
    private bool isConnecting;
    

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        //Connect();
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            connectionStatus.SetText("Connecting...");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial Launcher: Connected to master.");
        connectionStatus.SetText("Connected!");
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"Failed to join room. Creating new one.");
        PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = maxPlayersPerRoom});
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"PUN Basics Tutorial Launcher: Disconnected from server. Cause: {cause}");
        connectionStatus.SetText("Not connected");
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Loading level 'Room for 1'");

            PhotonNetwork.LoadLevel("Room for 1");
        }
    }
}
