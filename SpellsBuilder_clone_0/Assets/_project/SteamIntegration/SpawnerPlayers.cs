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

public class SpawnerPlayers : NetworkBehaviour
{
    public static event Action<NetworkObject> OnSpawned;

    public GameObject _playerPrefab;

    public Transform SpawnPoint;


    public void SpawnPlayer()
    {
        SpawnPlayerServerRpc(base.NetworkManager.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(ulong clientId)
    {
        SetSpawn(_playerPrefab.transform, out var position, out var rotation);
        var player = Instantiate(_playerPrefab, position, rotation);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        OnSpawned?.Invoke(player.GetComponent<NetworkObject>());
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

    private void SetSpawn(Transform prefab, out Vector3 pos, out Quaternion rot)
    {
        Transform result = SpawnPoint;
        if (result == null)
        {
            SetSpawnUsingPrefab(prefab, out pos, out rot);
        }
        else
        {
            pos = result.position;
            rot = result.rotation;
        }
    }

    private void SetSpawnUsingPrefab(Transform prefab, out Vector3 pos, out Quaternion rot)
    {
        pos = prefab.position;
        rot = prefab.rotation;
    }
}

