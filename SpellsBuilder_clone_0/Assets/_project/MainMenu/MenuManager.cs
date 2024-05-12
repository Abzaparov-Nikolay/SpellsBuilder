//using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipAddress;
    [SerializeField] private TMP_InputField port;
    [SerializeField] private GameObject ShutdownButton;

    private void Start()
    {
        NetworkManager.Singleton.OnClientStarted += ActivateButton;
        NetworkManager.Singleton.OnClientStopped += DeactivateButtong;
        DeactivateButtong(false);
    }

    private void OnDestroy()
    {
        uPnPHelper.CloseAll();
        if(NetworkManager.Singleton!= null)
        {
            NetworkManager.Singleton.OnClientStarted -= ActivateButton;
            NetworkManager.Singleton.OnClientStopped -= DeactivateButtong;
        }
    }

    public void StartHost()
    {
        BootstrapManager.Instance.HostLobby(ipAddress.text, ushort.Parse(port.text));
    }

    public void StartClient()
    {
        BootstrapManager.Instance.ClientLobby(ipAddress.text, ushort.Parse(port.text));
    }

    public void Shutdown()
    {
        BootstrapManager.Instance.Disconnect();
    }

    private void ActivateButton()
    {
        ShutdownButton.SetActive(true);
    }

    private void DeactivateButtong(bool shit)
    {
        ShutdownButton.SetActive(false);

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
