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
    //[SerializeField] Reference<Transform> player;
    [SerializeField] Reference<int> EnemyMax;
    int currentEnemyCount;
    [SerializeField] GameObject enemyPrefab;
    //[SerializeField] PlayersTracker tracker;
    //private UnityEngine.Random random;

    private void Update()
    {
        if (!IsServer) return;
        if (GetMaxEnemyAmount() > currentEnemyCount)
        {

            //var randPos = new Vector3(10, 0, 10);
            var randPlayer = PlayersTracker.Instance.GetRandom();
            if (randPlayer == null) return;
            var rand = UnityEngine.Random.value;
            var randPos = new Vector3(10 * Mathf.Cos(rand * 2 * Mathf.PI) + randPlayer.position.x,
                0,
                10 * Mathf.Sin(rand * 2 * Mathf.PI) + randPlayer.position.z);
            var enemy = Instantiate(enemyPrefab, randPos, Quaternion.identity);
            enemy.GetComponent<Death>().OnDeathWithGameObject.AddListener(Countdown);
            enemy.GetComponent<NetworkObject>().Spawn(true);
            //Spawn(enemy);
            currentEnemyCount++;
        }
    }

    private void Countdown(GameObject go)
    {
        go.GetComponent<Death>().OnDeathWithGameObject.RemoveListener(Countdown);
        currentEnemyCount--;
    }

    private int GetMaxEnemyAmount()
    {
        return (int)(EnemyMax * Mathf.Log(LevelTimer.Value, 5));
    }
}
