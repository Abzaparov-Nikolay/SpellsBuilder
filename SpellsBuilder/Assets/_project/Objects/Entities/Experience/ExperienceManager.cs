//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System;
using Unity.Netcode;
using UnityEngine;

public class ExperienceManager : NetworkBehaviour
{

    public NetworkVariable<float> Amount;

    public Action<float> OnAmountChange;

    private static Action<float> AddAmountAll;

    public static void AddAll(float amount)
    {
        //if (!base.IsServer) return;
        AddAmountAll?.Invoke(amount);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        AddAmountAll += AddAmount;
    }
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        AddAmountAll -= AddAmount;
    }

    public void AddAmount(float amount)
    {
        if (!base.IsServer) return;
        RpcSetAmount(amount + Amount.Value);
    }

    //[ServerRpc]
    private void RpcSetAmount(float newValue)
    {
        if (!base.IsServer) { return; }
        Amount.Value = newValue;
        NotifyClientRpc();
    }

    [ClientRpc]
    private void NotifyClientRpc()
    {
        OnAmountChange?.Invoke(Amount.Value);
        //update ui
    }




}
