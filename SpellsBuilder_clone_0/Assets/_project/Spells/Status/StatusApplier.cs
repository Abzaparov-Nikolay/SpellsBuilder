//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StatusApplier : NetworkBehaviour
{
    
    public NetworkVariable<StatusEffect> Status;

    

    public NetworkVariable<float> Power;
    private readonly Multiplier power = new();
    private float InventoryPower => PlayersInventory.GetStatValue(OwnerClientId, PlayerStat.StatusPower);

    public void SetStatusEffect(StatusEffect statusEffect)
    {
        if (!IsServer) return;
        Status.Value = statusEffect;
    }

    public void Apply(GameObject target)
    {
        if (!IsServer) return;
        //INVENTORYPOWER USE IT OR I LL KMS
        //getcomponent => setstatus
    }

    public void AddPowerBonus(float percentage)
    {
        if (!IsServer) return;
        power.Add(percentage);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        UpdateMultiplier();
        power.OnChange += UpdateMultiplier;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        power.OnChange -= UpdateMultiplier;
    }

    private void UpdateMultiplier()
    {
        Power.Value = power;
    }
}

