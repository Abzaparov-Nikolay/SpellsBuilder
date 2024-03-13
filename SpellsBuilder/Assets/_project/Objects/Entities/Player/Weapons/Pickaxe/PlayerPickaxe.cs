//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerPickaxe : Dealer
{

    [SerializeField] private Reference<float> DiggingDamageInit;
    private NetworkVariable<float> DiggingDamage;

    public override void Deal(GameObject target)
    {
        if (target.TryGetComponent<Destroyable>(out var destroyable))
        {
            destroyable.TakeDamage(DiggingDamage.Value);
        }
    }

    public void ChangeDamage(float newValue)
    {
        if (!IsServer) return;
        RpcChangeDamage(newValue);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer) { return; }
        RpcChangeDamage(DiggingDamageInit);
    }

    //[ServerRpc(RequireOwnership = false)]
    private void RpcChangeDamage(float newValue)
    {
        DiggingDamage.Value = newValue;
    }
}
