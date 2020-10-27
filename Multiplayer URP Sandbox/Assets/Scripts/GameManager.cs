using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    public GameObject playerPrefab;

    private void Awake()
    {
        if(Instance)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        if (playerPrefab)
        {
            
            if (PlayerManager.localPlayerInstance == null)
            {
                Debug.Log($"Instantiating LocalPlayer from {SceneManager.GetActiveScene().name} and automatically syncing it");
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 3.0f, 0), Quaternion.identity, 0);
            }
            else
            {
                Debug.Log("Ignore scene load.");
            }
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("Trying to load a level, but not as a master client!");
        }

        Debug.Log($"Photon: Loading level {PhotonNetwork.CurrentRoom.PlayerCount}");
        PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} entered room");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Player entered is master client.");
            
            LoadArena();
        }
    }
}
