//using FishNet.Managing.Scened;
//using FishNet.Object;
//using HeathenEngineering.SteamworksIntegration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapNetworkManager : NetworkBehaviour
{
    private static BootstrapNetworkManager instance;

    public static BootstrapNetworkManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("BNM already exists");
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        NetworkManager.OnServerStarted += GoToPlay;
    }

    public void GoToPlay()
    {
        NetworkManager.SceneManager.LoadScene(Names.GameplayScene, LoadSceneMode.Single);
        NetworkManager.SceneManager.OnSceneEvent += PlaySceneLoaded;
    }


    public void PlaySceneLoaded(SceneEvent cb)
    {
        if(cb.SceneEventType == SceneEventType.LoadEventCompleted)
        {

        }
    }
}
