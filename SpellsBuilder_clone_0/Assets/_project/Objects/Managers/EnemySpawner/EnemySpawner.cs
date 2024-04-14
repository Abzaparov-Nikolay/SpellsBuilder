//using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] Reference<int> EnemyMax;
    public int CurrentEnemyCount { get; private set; }
    [SerializeField] GameObject enemyPrefab;

    private bool stop;

    private void Update()
    {
        if (!IsServer) return;
        if (stop) return;
        if (GetMaxEnemyAmount() > CurrentEnemyCount)
        {
            var hits = new RaycastHit[8];
            //var randPos = new Vector3(10, 0, 10);
            var randPlayer = PlayersTracker.Instance.GetRandom();
            if (randPlayer == null) return;
            var rand = UnityEngine.Random.value;
            var randPos = new Vector3(10 * Mathf.Cos(rand * 2 * Mathf.PI) + randPlayer.position.x,
                0,
                10 * Mathf.Sin(rand * 2 * Mathf.PI) + randPlayer.position.z);
            while (Physics.BoxCastNonAlloc(randPos, new Vector3(0.5f, 0.5f, 0.5f), Vector3.up, hits, Quaternion.identity, 1f) != 0)
            {
                rand = UnityEngine.Random.value;
                randPos = new Vector3(10 * Mathf.Cos(rand * 2 * Mathf.PI) + randPlayer.position.x,
                    1,
                    10 * Mathf.Sin(rand * 2 * Mathf.PI) + randPlayer.position.z);
            }
            var enemy = Instantiate(enemyPrefab, randPos, Quaternion.identity);
            enemy.GetComponent<Death>().OnDeathWithGameObject.AddListener(Countdown);
            enemy.GetComponent<NetworkObject>().Spawn(true);
            //Spawn(enemy);
            CurrentEnemyCount++;
        }
    }

    private void Countdown(GameObject go)
    {
        go.GetComponent<Death>().OnDeathWithGameObject.RemoveListener(Countdown);
        CurrentEnemyCount--;
    }

    private int GetMaxEnemyAmount()
    {
        return (int)(EnemyMax * Mathf.Log(LevelTimer.Value, 5));
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GameStater.GamePaused += Stop;
        GameStater.GameUnpaused += Go;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        GameStater.GamePaused -= Stop;
        GameStater.GameUnpaused -= Go;
    }

    public void Stop()
    {
        stop = true;
    }

    public void Go()
    {
        stop = false;
    }
}
