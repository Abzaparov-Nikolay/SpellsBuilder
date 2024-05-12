//using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class DamageReceiver : NetworkBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private InvincibilityChanger Invincible;
    [SerializeField] private UnityEvent<float> OnTake;
    [SerializeField] private Variable<float> defence;

    private float InventoryDefence => PlayersInventory.GetStatValue(OwnerClientId, PlayerStat.Defence);

    public void TakeDamage(float amount)
    {
        if (!IsServer) return;
        if (Invincible != null && Invincible.Get() && amount > 0)
            return;
        if (defence != null)
        {
            amount = amount < 0 ? amount : amount * 50 / (defence * InventoryDefence);
        }
        OnTake?.Invoke(amount);
        health.TakeDamage(amount);
    }
}
