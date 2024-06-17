//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DamageDealer : Dealer
{
    public Reference<float> BaseDamage;
    //[SyncObject]
    private readonly Multiplier damageMultipliers = new();
    public NetworkVariable<float> DamageMultiplier = new();
    //[SyncVar]
    private NetworkVariable<bool> healing = new();
    private float InventoryDamage => PlayersInventory.GetStatValue(OwnerClientId, PlayerStat.Damage);

    public override void Deal(GameObject target)
    {
        if (!IsServer) return;
        if (target.TryGetComponentInParent<DamageReceiver>(out var damageReceiver))
        {
            damageReceiver.TakeDamage(InventoryDamage * BaseDamage * DamageMultiplier.Value * (!healing.Value == true ? 1 : -1));
        }
        if (target.TryGetComponentInParent<Destroyable>(out var damageRec))
        {
            damageRec.TakeDamage(BaseDamage);
        }
    }

    public void AddDamageBonus(float percentage)
    {
        if (!IsServer) return;
        damageMultipliers.Add(percentage);
    }

    public void AddDamageBonus(float percentage, bool SetHealing)
    {
        if (!IsServer) return;
        damageMultipliers.Add(percentage);
        healing.Value = SetHealing;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        damageMultipliers.OnChange += UpdateMultiplier;
        UpdateMultiplier();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        damageMultipliers.OnChange -= UpdateMultiplier;
    }

    private void UpdateMultiplier()
    {
        DamageMultiplier.Value = damageMultipliers;
    }
}
