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
    public void TakeDamage(float amount)
    {
        if (!IsServer) return;
        if (Invincible != null && Invincible.Get())
            return;
        OnTake?.Invoke(amount);
        health.TakeDamage(amount);
    }
}
