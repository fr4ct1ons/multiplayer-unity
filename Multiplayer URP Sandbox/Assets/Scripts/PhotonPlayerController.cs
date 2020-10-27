using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class PhotonPlayerController : MonoBehaviourPun
{
    [SerializeField] private Animator animator;
    [SerializeField] private CameraWork cameraWork;
    [SerializeField] private float directionDampTime = 0.25f;
    [SerializeField] private GameObject playerCamera;
    
    private void Awake()
    {
        if (!animator)
            animator = GetComponent<Animator>();

        if (!cameraWork)
            cameraWork = GetComponent<CameraWork>();
    }

    private void Start()
    {
        if (!photonView.IsMine)
        {
            Debug.Log("Disabling camera.");
            playerCamera.SetActive(false);
        }
    }

    private void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        
        float h = Input.GetAxis("Horizontal"), v = Input.GetAxis("Vertical");

        if (v < 0)
        {
            v = 0;
        }
        
        animator.SetFloat("Speed", h * h + v * v);
        animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
    }
}
