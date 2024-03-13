//using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PrefabSpawner : NetworkBehaviour
{
    [SerializeField] GameObject prefab;

    public void Spawn()
    {
        if (!IsServer) return;
        var spawned = Instantiate(prefab, transform.position, transform.rotation); 
        spawned.GetComponent<NetworkObject>().Spawn(true);
        //ServerManager.Spawn(spawned);
    }
}
