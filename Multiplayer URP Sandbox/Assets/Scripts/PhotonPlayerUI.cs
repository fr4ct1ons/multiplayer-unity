using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonPlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Vector3 instantiatedPosition;
    

    private PlayerManager target;

    private void Awake()
    {
        transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        transform.position = instantiatedPosition;
    }

    public void SetTarget(PlayerManager newTarget)
    {
        target = newTarget;
        
        playerNameText.SetText(target.photonView.Owner.NickName);
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }
    }
}
