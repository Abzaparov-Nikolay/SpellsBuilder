//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

public class ExperienceManager : NetworkBehaviour
{
    [SerializeField] private Reference<int> levelThreshHold;
    public NetworkVariable<float> Amount;
    public NetworkVariable<int> Level;

    public Action<float> OnAmountChange;

    private static Action<float> AddAmountAll;

    public static Action<int> OnLvlUp;

    public static void AddAll(float amount)
    {
        //if (!base.IsServer) return;
        AddAmountAll?.Invoke(amount);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        AddAmountAll += AddAmount;
        Amount.OnValueChanged += AmountUpped;
        Level.OnValueChanged += LevelUpped;
    }
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        AddAmountAll -= AddAmount;
        Amount.OnValueChanged -= AmountUpped;
        Level.OnValueChanged -= LevelUpped;

    }

    public void AddAmount(float amount)
    {
        if (!base.IsServer) return;
        Amount.Value = amount + Amount.Value;

    }

    private void AmountUpped(float prev, float now)
    {
        OnAmountChange?.Invoke(now);
        if (IsServer && Level.Value < (int)now / levelThreshHold)
        {
            Level.Value = (int)now / levelThreshHold * (Level.Value + 1);
        }
        //update ui
    }

    private void LevelUpped(int prev, int now)
    {
        OnLvlUp?.Invoke(now);
    }
}
