//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Health : NetworkBehaviour
{
    public Reference<float> MaxInit;
    private NetworkVariable<float> Max = new();
    private NetworkVariable<float> Current = new();

    private NetworkVariable<float> Regeneration = new();

    //[SerializeField] private readonly Reference<bool> Invincible;

    [SerializeField] private UnityEvent<float> OnDamaged;
    [SerializeField] private UnityEvent<float> OnCurrentZero;


    public Action<float, float> ShowChangesOnClient;

    //private void OnCurrentChange(float prev, float next, bool asServer)
    //{
    //    //if (!IsServer) return;
    //    if (!asServer)
    //        Current = next;
    //    var amount = prev - next;

    //    if (next <= 0)
    //    {
    //        OnCurrentZero?.Invoke(amount);
    //    }
    //    else
    //    {
    //        OnDamaged?.Invoke(amount);
    //    }
    //}

    //[ServerRpc(RequireOwnership = false)]
    private void ChangeCurrent(float newValue)
    {
        if (!IsServer) return;
        var amount = Current.Value - newValue;
        Current.Value = newValue;
        if (Current.Value <= 0)
        {
            OnCurrentZero?.Invoke(amount);
        }
        else
        {
            OnDamaged?.Invoke(amount);
        }
        NotifyChangesClientRpc();
    }

    [ClientRpc]
    private void NotifyChangesClientRpc()
    {
        ShowChangesOnClient?.Invoke(Max.Value, Current.Value);
    }

    //[ServerRpc(RequireOwnership = false)]
    private void ChangeMax(float newValue)
    {
        if (!IsServer) return;
        Max.Value = newValue;
        NotifyChangesClientRpc();
    }

    //[ServerRpc(RequireOwnership = false)]
    private void ChangeRegen(float newValue)
    {
        if (!IsServer) return;
        Regeneration.Value = newValue;
        NotifyChangesClientRpc();
    }

    public void TakeDamage(float amount)
    {
        if (!base.IsServer) return;

        ChangeCurrent(Current.Value - amount);
    }

    public float Get()
    {
        return Current.Value;
    }

    private void Update()
    {
        if (!IsServer) return;
        //ChangeCurrent(Get() + Regeneration * Time.deltaTime);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer) return;
        Max.Value = MaxInit;
        Current.Value = MaxInit;
    }

    public void AddBonus(float percentage)
    {
        if (!IsServer) return;
        var newMax = Max.Value * (1 + percentage / 100);
        var newCurrent = Current.Value < newMax
            ? newMax
            : Current.Value * newMax / Max.Value;
        ChangeMax(newMax);
        ChangeCurrent(newCurrent);
    }
}
