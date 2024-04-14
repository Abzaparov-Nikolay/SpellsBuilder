//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelTimer : NetworkBehaviour
{
    private static LevelTimer instance;

    private bool started;
    public static float Value => instance.value;

    //[SyncVar]
    private float value;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("LeverTimer already exists");
        }
        instance = this;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GameStater.GameStarted += StartTimer;
        GameStater.GameEnded += StopTimer;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        GameStater.GameStarted -= StartTimer;
        GameStater.GameEnded -= StopTimer;
    }


    void Update()
    {
        if (started)
            value += Time.deltaTime;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if(instance == this)
        {
            instance = null;
        }
    }

    void StartTimer()
    {
        started = true;
    }

    void StopTimer()
    {
        started = false;
    }
}
