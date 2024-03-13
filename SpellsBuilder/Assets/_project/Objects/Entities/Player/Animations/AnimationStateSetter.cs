//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AnimationStateSetter : NetworkBehaviour
{
    [SerializeField] private Animator animator;

    private NetworkVariable<AnimationData> data;

    public void Set(PlayerState state)
    {
        SetServerServerRpc(state);
    }

    public void CastStarted()
    {
        SetServerCastingServerRpc(true);
    }

    public void CastEnded()
    {
        SetServerCastingServerRpc(false);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        data.OnValueChanged += UpdateHolder;
        //data.OnValueChanged += UpdateHolder;
        MagicInputHandler.SpellCastStarting += CastStarted;
        MagicInputHandler.SpellCastStopped += CastEnded;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        data.OnValueChanged -= UpdateHolder;
        //data.OnValueChanged -= UpdateHolder;
        MagicInputHandler.SpellCastStarting -= CastStarted;
        MagicInputHandler.SpellCastStopped -= CastEnded;
    }

    [ServerRpc]
    private void SetServerWalkingServerRpc(bool isWalking)
    {
        data.Value = new AnimationData(isWalking, data.Value.casting);
        //UpdateAnimator();
    }

    [ServerRpc]
    private void SetServerCastingServerRpc(bool isCasting)
    {
        data.Value = new AnimationData(data.Value.walking, isCasting);
        //UpdateAnimator();
    }

    [ServerRpc]
    private void SetServerServerRpc(PlayerState state)
    {
        data.Value = new AnimationData(state == PlayerState.Walking, data.Value.casting);
        //UpdateAnimator();
    }


    private void UpdateHolder(AnimationData prev, AnimationData next)
    {
        UpdateAnimator();
    }

    //[ClientRpc]
    private void UpdateAnimator()
    {
        animator.SetBool("Walking", data.Value.walking);
        animator.SetBool("2HandAttackStart", data.Value.casting);
        animator.SetBool("2HandAttackStop", !data.Value.casting);
    }
}

public enum PlayerState
{
    Idle,
    Walking,
    CastingSpell
}

public struct AnimationData : INetworkSerializable
{
    public bool casting;
    public bool walking;

    public AnimationData(bool walking, bool casting)
    {
        this.casting = casting;
        this.walking = walking;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref walking);
        serializer.SerializeValue(ref casting);
    }
}
