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

    public void StartHost()
    {
        BootstrapManager.Instance.HostLobby(ipAddress.text, ushort.Parse(port.text));
    }

    private void OnDestroy()
    {
        uPnPHelper.CloseAll();
    }

    public void StartClient()
    {
        BootstrapManager.Instance.ClientLobby(ipAddress.text, ushort.Parse(port.text));
    }
}
