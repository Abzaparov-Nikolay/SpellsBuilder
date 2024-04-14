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
    private NetworkVariable<float> Max = new();
    private NetworkVariable<float> Current = new();

    private NetworkVariable<float> Regeneration = new();

    [SerializeField] private UnityEvent<float> OnDamaged;
    [SerializeField] private UnityEvent<float> OnCurrentZero;
    private float InventoryValue => PlayersInventory.GetStatValue(base.OwnerClientId, PlayerStat.MaxHealth);
    private float lastHealTick = 0;

    public Action<float, float> ShowChangesOnClient; //Max, Current

    private void ChangeCurrent(float newValue)
    {
        if (!IsServer) return;
        var amount = Current.Value - newValue;
        Current.Value = newValue < Max.Value ? newValue : Max.Value;
        if (Current.Value <= 0)
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
        Max.Value = newValue;
    }

    private void ChangeRegen(float newValue)
    {
        if (!IsServer) return;
        Regeneration.Value = newValue;
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
        ChangeCurrent(Get() + Regeneration.Value);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Max.OnValueChanged += OnValuesChange;
        Current.OnValueChanged += OnValuesChange;
        if (!IsServer) return;
        Max.Value = MaxInit;
        Current.Value = MaxInit;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        Max.OnValueChanged -= OnValuesChange;
        Current.OnValueChanged -= OnValuesChange;
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

    private void OnValuesChange(float prev, float current)
    {
        ShowChangesOnClient?.Invoke(Max.Value, Current.Value);
    }
}
