//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System;
using System.Collections;
//using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
//using Unity.VisualScripting;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

public class Health : NetworkBehaviour
{
    public Reference<float> MaxInit;
    private NetworkVariable<HealthStats> Stats = new();
    [SerializeField] private UnityEvent<float> OnDamaged;
    [SerializeField] private UnityEvent<float> OnCurrentZero;
    private float InventoryValue => PlayersInventory.GetStatValue(base.OwnerClientId, PlayerStat.MaxHealth);
    private float lastHealTick = 0;

    public Action<float, float> ShowChangesOnClient; //Max, Current

    private void ChangeCurrent(float newValue)
    {
        if (!IsServer) return;
        var amount = Stats.Value.Current - newValue;
        var nextValue = newValue < Stats.Value.Max ? newValue : Stats.Value.Max;
        SetNewValue(Stats.Value.Max, nextValue, Stats.Value.Regeneration);
        if (Stats.Value.Current <= 0)
        {
            OnCurrentZero?.Invoke(amount);
        }
        else
        {
            if (amount > 0)
            {
                OnDamaged?.Invoke(amount);
            }
        }
    }


    private void ChangeMax(float newValue)
    {
        if (!IsServer) return;
        SetNewValue(newValue, Stats.Value.Current, Stats.Value.Regeneration);
    }

    private void ChangeRegen(float newValue)
    {
        if (!IsServer) return;
        SetNewValue(Stats.Value.Max, Stats.Value.Current, newValue);
    }

    public void TakeDamage(float amount)
    {
        if (!base.IsServer) return;

        ChangeCurrent(Stats.Value.Current - amount);
    }

    public float Get()
    {
        return Stats.Value.Current;
    }

    private void Update()
    {
        if (!IsServer) return;
        lastHealTick += Time.deltaTime;
        if (lastHealTick >= 3)
        {
            ThreeSecUpdate();
            lastHealTick = 0;
        }
    }

    private void ThreeSecUpdate()
    {
        if (!IsServer) return;
        //if(NetworkManager.IsConnectedClient)
        if (NetworkManager.Singleton.ConnectedClients.Values.Select(pl => pl.PlayerObject).Any(po => po == NetworkObject))
        {
            ChangeMax(MaxInit * InventoryValue);
        }
        ChangeCurrent(Get() + Stats.Value.Regeneration);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Stats.OnValueChanged += OnValuesChange;
        if (!IsServer) return;
        SetNewValue(MaxInit, MaxInit, 0);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        Stats.OnValueChanged -= OnValuesChange;
    }

    public void AddBonus(float percentage)
    {
        if (!IsServer) return;
        var newMax = Stats.Value.Max * (1 + percentage / 100);
        var newCurrent = Stats.Value.Current < newMax
            ? newMax
            : Stats.Value.Current * newMax / Stats.Value.Max;
        ChangeMax(newMax);
        ChangeCurrent(newCurrent);
    }

    private void SetNewValue(float newMAx, float newCurrent, float newRegen)
    {
        Stats.Value = new HealthStats(newMAx, newCurrent, newRegen);
    }

    private void OnValuesChange(HealthStats prev, HealthStats current)
    {
        ShowChangesOnClient?.Invoke(current.Max, current.Current);
    }
}

[Serializable]
public struct HealthStats : IEquatable<HealthStats>, INetworkSerializable
{
    public float Max;
    public float Current;
    public float Regeneration;

    public HealthStats(float max, float current, float regen)
    {
        Max = max;
        Current = current;
        Regeneration = regen;
    }

    public bool Equals(HealthStats other)
    {
        return Max == other.Max
            && Current == other.Current
            && Regeneration == other.Regeneration;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Max);
        serializer.SerializeValue(ref Current);
        serializer.SerializeValue(ref Regeneration);
    }
}
