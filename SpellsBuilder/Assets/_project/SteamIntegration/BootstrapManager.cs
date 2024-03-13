
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapManager : MonoBehaviour
{
    private static BootstrapManager instance;

    public static BootstrapManager Instance => instance;
    public static Action<string, bool> OnLobbyEnterSuccess;

    //public static Action<List<CSteamID>, LobbyDataUpdate_t> OnLobbyListUpdate;


    private void Awake()
    {
        if (instance != null)
            Debug.Log("Sussy");
        instance = this;
    }


    //[SerializeField] private NetworkManager networkManager;
    //[SerializeField] private FishySteamworks.FishySteamworks fishySteamworks;


    //Callbacks
    //protected Callback<LobbyCreated_t> lobbyCreated;
    //protected Callback<GameLobbyJoinRequested_t> joinRequest;
    //protected Callback<LobbyEnter_t> lobbyEntered;


    //LoobyCallbacks
    //protected Callback<LobbyMatchList_t> lobbyMatchList;
    //protected Callback<LobbyDataUpdate_t> lobbyDataUpdate;

    //private List<CSteamID> lobbyIDs = new();

    public static ulong CurrentLobbyID;

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(Names.MainMenuScene, LoadSceneMode.Additive);
    }

    public static void CreateLobby()
    {
        //SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 4);
    }

    public static void LeaveLobby()
    {
        //SteamMatchmaking.LeaveLobby(new CSteamID(CurrentLobbyID));
        CurrentLobbyID = 0;

        //instance.fishySteamworks.StopConnection(false);
        //if (instance.networkManager.IsServer)
        //{
        //    instance.fishySteamworks.StopConnection(true);
        //}
    }

    public static void JoinById()//CSteamID steamID)
    {
        Debug.Log("Trying to join");
        //if (SteamMatchmaking.RequestLobbyData(steamID))
        //{
        //    SteamMatchmaking.JoinLobby(steamID);
        //}
        //else
        //{
        //    Debug.Log("joining failed");
        //}
    }

    private void Start()
    {
        //lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        //joinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        //lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        //lobbyMatchList = Callback<LobbyMatchList_t>.Create(OnGetLobbiesMatchList);
        //lobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdated);
    }

    public static void GetLobbiesList()
    {
        //    if (instance.lobbyIDs.Count > 0) instance.lobbyIDs.Clear();

        //    SteamMatchmaking.AddRequestLobbyListResultCountFilter(60);
        //    SteamMatchmaking.RequestLobbyList();
    }

    public static int GetNumLobbyMembers()
    {
        return 0;//SteamMatchmaking.GetNumLobbyMembers(new CSteamID(CurrentLobbyID));
    }

    private void OnLobbyDataUpdated()//LobbyDataUpdate_t callback)
    {
        //OnLobbyListUpdate?.Invoke(lobbyIDs, callback);
    }

    private void OnGetLobbiesMatchList()//LobbyMatchList_t callback)
    {
        //for (var i = 0; i < callback.m_nLobbiesMatching; i++)
        //{
        //    var lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
        //    lobbyIDs.Add(lobbyID);
        //    SteamMatchmaking.RequestLobbyData(lobbyID);
        //}
    }

    private void OnLobbyCreated()//LobbyCreated_t callback)
    {
        //Debug.Log("Tets createion");
        //if (callback.m_eResult != EResult.k_EResultOK)
        //{
        //    return;
        //}
        //CurrentLobbyID = callback.m_ulSteamIDLobby;
        //SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID),
        //    Names.HostAddress,
        //    SteamUser.GetSteamID().ToString());

        //SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID),
        //    Names.Name,
        //    SteamFriends.GetPersonaName() + "'s lobby");

        //fishySteamworks.SetClientAddress(SteamUser.GetSteamID().ToString());
        //fishySteamworks.StartConnection(true);

        Debug.Log("Tets createion");

    }

    private void OnJoinRequest()//GameLobbyJoinRequested_t callback)
    {
        //SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered()//LobbyEnter_t callback)
    {
        Debug.Log("Tets Entering");


        //CurrentLobbyID = callback.m_ulSteamIDLobby;
        //fishySteamworks.SetClientAddress(
        //    SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), Names.HostAddress));
        //fishySteamworks.StartConnection(false);

        //OnLobbyEnterSuccess?.Invoke(SteamMatchmaking.GetLobbyData(
        //    new CSteamID(CurrentLobbyID), Names.Name),
        //    networkManager.IsServer);
    }
}
