//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class InvincibilityChanger : NetworkBehaviour
{

    private NetworkVariable<bool> SettableFlag;
    [SerializeField] private Reference<float> InvincibilityDurationInitial;
    private float InvincibilityDuration;

    [SerializeField] public UnityEvent<float> OnInvincible;
    private float elapsed;

    private void OnInvincibleChange(bool prev, bool next)
    {
        //if (!asServer)
        SettableFlag.Value = next;
        if (SettableFlag.Value)
        {
            OnInvincible?.Invoke(InvincibilityDuration);
        }
    }

    private void ChangeValue(bool newValue)
    {
        if (!IsServer) { return; }
        SettableFlag.Value = newValue;
        if (SettableFlag.Value)
        {
            OnInvincible?.Invoke(InvincibilityDuration);
        }
    }

    private void RpcChangeDuration(float newValue)
    {
        if (!IsServer) { return; }
        InvincibilityDuration = newValue;
    }

    public void BecomeInvincible()
    {
        if (!base.IsServer) return;
        ChangeValue(true);
        elapsed = 0;
    }

    public void ChangeDuration(float duration)
    {
        if (!IsServer) return;
        RpcChangeDuration(duration);
    }

    private void Update()
    {
        if (!IsServer) return;
        elapsed += Time.deltaTime;
        if (elapsed >= InvincibilityDuration)
        {
            ChangeValue(false);
        }
    }

    public bool Get() => SettableFlag.Value;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        InvincibilityDuration = InvincibilityDurationInitial;
    }
}
