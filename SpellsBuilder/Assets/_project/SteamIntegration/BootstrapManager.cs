
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
//using UnityEditor;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
//using System.Net;

public class BootstrapManager : MonoBehaviour
{
    private static BootstrapManager instance;

    public static BootstrapManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
            Debug.Log("Sussy");
        instance = this;
        DontDestroyOnLoad(instance);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(Names.MainMenuScene, LoadSceneMode.Single);
    }

    private void Start()
    {
        GoToMainMenu();
        SceneManager.UnloadSceneAsync(Names.BootstrapScene);
    }

    public void SingleLobby()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void HostLobby(string ipAddress, ushort port)
    {
        uPnPHelper.NewMessage += LogMessage;
        uPnPHelper.Start(uPnPHelper.Protocol.UDP, port, 0, "SpellsBuilder");
        uPnPHelper.Start(uPnPHelper.Protocol.TCP, port, 0, "SpellsBuilder");

        (NetworkManager.Singleton.NetworkConfig.NetworkTransport as UnityTransport)
            .SetConnectionData("0.0.0.0", port, "0.0.0.0");


        NetworkManager.Singleton.StartHost();
    }

    public void ClientLobby(string ipAddress, ushort port)
    {
        uPnPHelper.NewMessage += LogMessage;

        uPnPHelper.Start(uPnPHelper.Protocol.UDP, port, 0, "SpellsBuilder");
        uPnPHelper.Start(uPnPHelper.Protocol.TCP, port, 0, "SpellsBuilder");

        (NetworkManager.Singleton.NetworkConfig.NetworkTransport as UnityTransport)
            .SetConnectionData(ipAddress, port);


        NetworkManager.Singleton.StartClient();
    }

    public void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();
        ClosePorts();
        GoToMainMenu();
    }

    public void ClosePorts()
    {
        uPnPHelper.CloseAll();
        uPnPHelper.NewMessage -= LogMessage;
    }

    private void OnDestroy()
    {
        
        ClosePorts();
    }

    private void LogMessage(string message)
    {
        Debug.Log(message);
    }
}
