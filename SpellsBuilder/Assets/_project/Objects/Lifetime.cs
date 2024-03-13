//using FishNet.Object;
//using FishNet.Object.Synchronizing;
//using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Lifetime : NetworkBehaviour
{
    [SerializeField] public Reference<float> intervalBase;

    public NetworkVariable<float> intervalMultiplier = new();
    private readonly Multiplier intervalMultipliers = new();


    [SerializeField] public bool autoRestart;
    [SerializeField] private bool active;
    [SerializeField] public UnityEvent elapsed;
    [SerializeField] private bool restartOnEnable;
    private float timeLeft;

    //void Awake()
    //{
    //    timeLeft = intervalBase;
    //}

    void Update()
    {
        if (!IsServer) return;
        if (active)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                elapsed?.Invoke();
                if (autoRestart)
                    Restart();
                else
                    active = false;
            }
        }
    }

    public void Restart()
    {
        if (!IsServer) return;
        timeLeft = intervalBase * intervalMultiplier.Value;
        active = true;
    }

    public void AddTimeBonus(float percentage)
    {
        if (!IsServer) return;
        RpcAddTimeBonusServerRpc(percentage);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        UpdateMultiplier();
        timeLeft = intervalBase * intervalMultiplier.Value;
        intervalMultipliers.OnChange += UpdateMultiplier;
        //Restart();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        intervalMultipliers.OnChange -= UpdateMultiplier;
    }

    private void UpdateMultiplier()
    {
        intervalMultiplier.Value = intervalMultipliers;
    }

    [ServerRpc]
    private void RpcAddTimeBonusServerRpc(float percentage)
    {
        intervalMultipliers.Add(percentage);
    }

    private void OnEnable()
    {
        if (restartOnEnable)
            Restart();
    }
}
