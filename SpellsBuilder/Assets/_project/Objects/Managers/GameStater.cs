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
    private static GameStater _instance;
    //public Action<NetworkConnection> PlayerDyied;
    private List<Transform> players = new();
    private int deadCount = 0;
    public static Action GameStarted;
    public static Action GamePaused;
    public static Action GameUnpaused;
    public static Action GameEnded;
    public static Action GameWon;

    private bool notStarted = true;
    private int stopSources = 0;

    private void Awake()
    {
        if(_instance != null)
        {
            Debug.Log("gameStater already exists");
        }
        _instance = this;
    }

    //private void Start()
    //{
    //    //OnFirstSpanw.OnObjectClienSpawn += PlayerSpawned;
    //}

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        OnFirstSpanw.OnObjectServerSpawn += PlayerSpawned;
        if (IsServer)
        {
            StartCoroutine(CheckVictory());
        }
        //ExperienceManager.OnLvlUp += StopGame;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        OnFirstSpanw.OnObjectServerSpawn -= PlayerSpawned;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _instance = null;
    }


    private void PlayerSpawned(Transform transform)
    {
        if (!IsServer) return;
        players.Add(transform);
        transform.gameObject.GetComponent<Death>().OnDeath.AddListener(PlayerDied);
        if (players.Count == NetworkManager.ConnectedClientsIds.Count && notStarted)
        {
            GameStarted?.Invoke();
            notStarted = false;
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

    private void Victory()
    {
        if (!IsServer)
            return;
        GameWonClientRpc();
    }

    [ClientRpc]
    public void GameWonClientRpc()
    {
        GameWon?.Invoke();
    }


    public static void StopGame()
    {
        if (!_instance.IsServer) return;
        _instance.stopSources += 1;
        if(_instance.stopSources == 1 )
        {
            GamePaused?.Invoke();
        }
    }

    public static void Continue()
    {
        if (!_instance.IsServer) return;
        _instance.stopSources -= 1;
        if (_instance.stopSources <= 0)
        {
            GameUnpaused?.Invoke();
        }
    }

    private IEnumerator CheckVictory()
    {
        while (true)
        {
            int minutes = Mathf.FloorToInt(LevelTimer.Value / 60F);
            if (minutes >= 10)
            {
                Victory();
            }
            yield return new WaitForSeconds(1);
        }
    }
}
