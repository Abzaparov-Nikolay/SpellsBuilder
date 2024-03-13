//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
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

    //public override void OnStartServer()
    //{
    //    base.OnStartServer();
    //    GameStater.GameStarted += StartTimer;
    //    GameStater.GameEnded += StopTimer;
    //}

    //public override void OnStopServer()
    //{
    //    base.OnStopServer();
    //    GameStater.GameStarted -= StartTimer;
    //    GameStater.GameEnded -= StopTimer;
    //}

    
    void Update()
    {
        if (started)
            value += Time.deltaTime;
    }

    private void OnDestroy()
    {
        instance = null;
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
