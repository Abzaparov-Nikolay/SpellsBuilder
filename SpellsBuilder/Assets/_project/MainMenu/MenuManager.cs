//using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject lobbyMenu;
    [SerializeField] private GameObject lobbiesListMenu;

    [SerializeField] private TMP_InputField lobbyIDInput;
    [SerializeField] private TextMeshProUGUI lobbyName;
    [SerializeField] private TextMeshProUGUI lobbyIDText;
    [SerializeField] private Button startGameButton;

    private void OnEnable()
    {
        BootstrapManager.OnLobbyEnterSuccess += OnLobbyEntered;
    }
    private void OnDisable()
    {
        BootstrapManager.OnLobbyEnterSuccess -= OnLobbyEntered;
    }


    public void CreateLobby()
    {
        BootstrapManager.CreateLobby();
    }

    public void JoinLobby()
    {
        //CSteamID steamID = new CSteamID(Convert.ToUInt64(lobbyIDInput.text));
        BootstrapManager.JoinById();
    }

    //public void JoinLobby()//CSteamID steamID)
    //{
    //    BootstrapManager.JoinById();
    //}

    public void LeaveLobby()
    {
        BootstrapManager.LeaveLobby();
    }

    public void StartGame()
    {
        var scenesToClose = new string[] { Names.MainMenuScene };//piss
        BootstrapNetworkManager.ChangeNetworkScene(Names.GameplayScene, scenesToClose);
    }

    public void CopyLobbyIdToClipboard()
    {
        GUIUtility.systemCopyBuffer = lobbyIDText.text;
    }

    private void OnLobbyEntered(string LobbyName, bool isHost)
    {
        lobbyName.text = LobbyName;
        if (isHost)
        {
            startGameButton.gameObject.SetActive(true);
        }
        else
        {
            startGameButton.gameObject.SetActive(false);
        }
        lobbyIDText.text = BootstrapManager.CurrentLobbyID.ToString();
        OpenLobbyMenu();

    }


    private void OpenMainMenu()
    {
        CloseMenus();
        mainMenu.gameObject.SetActive(true);
    }

    private void OpenLobbyMenu()
    {
        CloseMenus();
        lobbyMenu.SetActive(true);
    }

    private void CloseMenus()
    {
        mainMenu.gameObject.SetActive(false);
        lobbyMenu.gameObject.SetActive(false);
        lobbiesListMenu.SetActive(false);
    }

    
}
