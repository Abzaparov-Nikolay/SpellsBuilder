//using FishNet.Connection;
//using FishNet.Object;
//using HeathenEngineering.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameStater : NetworkBehaviour
{
    //public Action<NetworkConnection> PlayerDyied;
    private List<Transform> players = new();
    private int deadCount = 0;
    public static Action GameStarted;
    public static Action GameEnded;


    //private void Start()
    //{
    //    //OnFirstSpanw.OnObjectClienSpawn += PlayerSpawned;
    //}

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        OnFirstSpanw.OnObjectServerSpawn += PlayerSpawned;

    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        OnFirstSpanw.OnObjectServerSpawn -= PlayerSpawned;
    }


    private void PlayerSpawned(Transform transform)
    {
        if (!IsServer) return;
        players.Add(transform);
        transform.gameObject.GetComponent<Death>().OnDeath.AddListener(PlayerDied);
        if (players.Count == BootstrapNetworkManager.Instance.GetPlayerCount())
        {
            GameStarted?.Invoke();
        }
    }

    
    private void PlayerDisconnected(Transform transform)
    {
        if (!IsServer) return;
        players.Remove(transform);
        transform.GetComponent<Death>().OnDeath.RemoveListener(PlayerDied);
    }

    //[Server]
    private void PlayerDied()
    {
        if (!IsServer)
            return;
        deadCount++;
        if (deadCount == players.Count)
        {
            GameLostClientRpc();
        }
    }

    [ClientRpc]
    public void GameLostClientRpc()
    {
        GameEnded?.Invoke();
    }


}
