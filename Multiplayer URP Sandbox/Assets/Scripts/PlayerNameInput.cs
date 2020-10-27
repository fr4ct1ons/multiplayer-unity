using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;

    private const string playerNameKey = "PlayerUserName";

    [Space]
    [SerializeField] private string currentName;

    private void Awake()
    {
        if (!nameField)
            nameField = GetComponent<TMP_InputField>();

        if (PlayerPrefs.HasKey(playerNameKey))
        {
            currentName = PlayerPrefs.GetString(playerNameKey);
        }
        else
        {
            currentName = nameField.text;
            PlayerPrefs.SetString(playerNameKey, currentName);
            PlayerPrefs.Save();
        }

        PhotonNetwork.NickName = currentName;
        
        nameField.SetTextWithoutNotify(currentName);
    }

    public void UpdatePlayerName(string newName)
    {
        currentName = newName;
        PlayerPrefs.SetString(playerNameKey, currentName);
        PlayerPrefs.Save();
        PhotonNetwork.NickName = currentName;
    }
}
