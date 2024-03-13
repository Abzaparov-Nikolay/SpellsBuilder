//using FishNet.Connection;
//using FishNet.Managing;
//using FishNet.Object;
//using FishNet;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnerPlayers : MonoBehaviour
{
    public static event Action<NetworkObject> OnSpawned;

    //[SerializeField]
    //private NetworkObject _playerPrefab;

    
    
    [FormerlySerializedAs("_spawns")]
    public Transform[] Spawns = new Transform[0];

    
    //private NetworkManager _networkManager;
    private int _nextSpawn;

    private void Start()
    {
        InitializeOnce();
    }

    private void OnDestroy()
    {
        //if (_networkManager != null) { }
    }

    public void Spawn()
    {
        //if (_networkManager == null)
        //    return;
        //SceneManager_OnClientLoadedStartScenes(_networkManager.ClientManager.Connection);
    }

    /// <summary>
    /// Initializes this script for use.
    /// </summary>
    private void InitializeOnce()
    {
        //_networkManager = InstanceFinder.NetworkManager;
        //if (_networkManager == null)
        //{
        //    Debug.LogWarning($"PlayerSpawner on {gameObject.name} cannot work as NetworkManager wasn't found on this object or within parent objects.");
        //    return;
        //}
        //_networkManager.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
    }



    /// <summary>
    /// Called when a client loads initial scenes after connecting.
    /// </summary>
    //[ServerRpc(RequireOwnership = false)]
    //private void SceneManager_OnClientLoadedStartScenes(NetworkConnection conn)
    //{
    //    //if (_networkManager.ClientManager.Connection != conn)
    //    //    return;
    //    if (_playerPrefab == null)
    //    {
    //        Debug.LogWarning($"Player prefab is empty and cannot be spawned for connection {conn.ClientId}.");
    //        return;
    //    }

    //    Vector3 position;
    //    Quaternion rotation;
    //    SetSpawn(_playerPrefab.transform, out position, out rotation);

    //    NetworkObject nob = _networkManager.GetPooledInstantiated(_playerPrefab, position, rotation, true);
    //    _networkManager.ServerManager.Spawn(nob, conn);

    //    //If there are no global scenes 
    //    //if (_addToDefaultScene)
    //    //    _networkManager.SceneManager.AddOwnerToDefaultScene(nob);

    //    OnSpawned?.Invoke(nob);
    //}




    /// <summary>
    /// Sets a spawn position and rotation.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    private void SetSpawn(Transform prefab, out Vector3 pos, out Quaternion rot)
    {
        //No spawns specified.
        if (Spawns.Length == 0)
        {
            SetSpawnUsingPrefab(prefab, out pos, out rot);
            return;
        }

        Transform result = Spawns[_nextSpawn];
        if (result == null)
        {
            SetSpawnUsingPrefab(prefab, out pos, out rot);
        }
        else
        {
            pos = result.position;
            rot = result.rotation;
        }

        //Increase next spawn and reset if needed.
        _nextSpawn++;
        if (_nextSpawn >= Spawns.Length)
            _nextSpawn = 0;
    }

    /// <summary>
    /// Sets spawn using values from prefab.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    private void SetSpawnUsingPrefab(Transform prefab, out Vector3 pos, out Quaternion rot)
    {
        pos = prefab.position;
        rot = prefab.rotation;
    }

}

