//using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Destroyable : NetworkBehaviour
{
    [SerializeField] Health health;

    public void TakeDamage(float damage)
    {
        if (!IsServer) return;
        health.TakeDamage(damage);
    }
}
