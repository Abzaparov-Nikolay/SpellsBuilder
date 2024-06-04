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
        GameStater.GameStarted += StartTimerClientRpc;
        GameStater.GameEnded += StopTimerClientRpc;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        GameStater.GameStarted -= StartTimerClientRpc;
        GameStater.GameEnded -= StopTimerClientRpc;
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

    [ClientRpc]
    void StartTimerClientRpc()
    {
        started = true;
    }

    [ClientRpc]
    void StopTimerClientRpc()
    {
        started = false;
    }

    
}
