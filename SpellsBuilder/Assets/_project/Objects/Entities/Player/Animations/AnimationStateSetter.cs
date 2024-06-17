//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AnimationStateSetter : NetworkBehaviour
{
    [SerializeField] private Animator animator;

    private NetworkVariable<AnimationData> data = new(writePerm: NetworkVariableWritePermission.Owner);

    public void Set(PlayerState state)
    {
        if (base.IsOwner)
            SetServer(state);
    }

    public void CastStarted()
    {
        if (base.IsOwner)
            SetServerCasting(true);
    }

    public void CastEnded()
    {
        if (base.IsOwner)
            SetServerCasting(false);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        data.OnValueChanged += UpdateHolder;
        if (base.IsOwner)
        {
            MagicInputHandler.SpellCastStarting += CastStarted;
            MagicInputHandler.SpellCastStopped += CastEnded;
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        data.OnValueChanged -= UpdateHolder;
        //data.OnValueChanged -= UpdateHolder;
        if (base.IsOwner)
        {
            MagicInputHandler.SpellCastStarting -= CastStarted;
            MagicInputHandler.SpellCastStopped -= CastEnded;
        }
    }

    //[ServerRpc]
    private void SetServerWalking(bool isWalking)
    {
        data.Value = new AnimationData(isWalking, data.Value.casting);
        //UpdateAnimator();
    }

    //[ServerRpc]
    private void SetServerCasting(bool isCasting)
    {
        data.Value = new AnimationData(data.Value.walking, isCasting);
        //UpdateAnimator();
    }

    //[ServerRpc]
    private void SetServer(PlayerState state)
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
