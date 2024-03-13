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
    }

    public static void ChangeNetworkScene(string sceneToLoad, string[] scenesToClose)
    {


        //SceneLoadData sld = new SceneLoadData(sceneToLoad);
        //SceneUnloadData sud = new SceneUnloadData(scenesToClose);

        //var conns = instance.ServerManager.Clients.Values.ToArray();
        //instance.SceneManager.UnloadConnectionScenes(conns, sud);
        //instance.SceneManager.OnLoadEnd += SetMainSceneOnLoad;
        //instance.SceneManager.LoadConnectionScenes(conns, sld);
    }

    private static void SetMainSceneOnLoad()//SceneLoadEndEventArgs e)
    {

        //if (e.LoadedScenes.Count() != 0 && e.LoadedScenes.FirstOrDefault() != null)
        //    UnityEngine.SceneManagement.SceneManager.SetActiveScene(e.LoadedScenes.FirstOrDefault());

        //instance.SceneManager.OnLoadEnd -= SetMainSceneOnLoad;
    }

    public int GetPlayerCount()
    {
        return BootstrapManager.GetNumLobbyMembers();
    }

    public static void Disconnect()
    {
        if (!instance.IsServer)
        {
            BootstrapManager.LeaveLobby();
            UnityEngine.SceneManagement.SceneManager.LoadScene(Names.MainMenuScene, LoadSceneMode.Additive);

            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(Names.GameplayScene);
        }
        else
        {
            instance.HostDisconnectingServerRpc();
            BootstrapManager.LeaveLobby();
        }
    }

    [ServerRpc]
    public void HostDisconnectingServerRpc()
    {
        if (!instance.IsServer) return;
        ChangeNetworkScene(Names.MainMenuScene, new string[] { Names.GameplayScene });
        instance.HostDisconnectingObserverClientRpc();
    }

    [ClientRpc]
    public void HostDisconnectingObserverClientRpc()
    {
        if (instance.IsServer) return;
        BootstrapManager.LeaveLobby();
    }

    //[ServerRpc(RequireOwnership = false)]
    //private void CloseScenes(string[] scenes)
    //{
    //    CloseScenesObserver(scenes);
    //}

    //[ObserversRpc]
    //private void CloseScenesObserver(string[] scenes)
    //{
    //    foreach (var sceneName in scenes)
    //    {
    //        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
    //    }
    //}
}
