using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using FishNet;
//using FishNet.Connection;
//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private InputDirectionProvider inputDirection;
    [SerializeField] private InputMousePosProvider inputMousePosition;
    [SerializeField]
    private Rigidbody body;
    [SerializeField]
    private float speed;

    [SerializeField] private AnimationStateSetter stateSetter;

    private bool rotateToMouse = false;

    //public override void OnStartClient()
    //{
    //    base.OnStartClient();
    //    if (!base.IsOwner)
    //        this.enabled = false;
    //    MagicInputHandler.SpellCastStarting += StartRotateToMouse;
    //    MagicInputHandler.SpellCastStopped += StopRotationToMouse;
    //}
    //public override void OnStopClient()
    //{
    //    base.OnStopClient();
    //    MagicInputHandler.SpellCastStarting -= StartRotateToMouse;
    //    MagicInputHandler.SpellCastStopped -= StopRotationToMouse;
    //}

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!base.IsOwner)
            this.enabled = false;
        MagicInputHandler.SpellCastStarting += StartRotateToMouse;
        MagicInputHandler.SpellCastStopped += StopRotationToMouse;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        MagicInputHandler.SpellCastStarting -= StartRotateToMouse;
        MagicInputHandler.SpellCastStopped -= StopRotationToMouse;
    }

    


    private void FixedUpdate()
    {
        body.velocity = new Vector3(inputDirection.Get().x, 0, inputDirection.Get().y) * speed;
        if (rotateToMouse)
        {

            var facingDir = (inputMousePosition.Get() - body.position).normalized;
            var dir = new Vector3(facingDir.x, 0, facingDir.z).normalized;
            body.rotation = Quaternion.LookRotation(dir, Vector3.up);

        }

        if (body.velocity.magnitude > 0)
        {
            if (!rotateToMouse)
                body.rotation = Quaternion.LookRotation(body.velocity.normalized, Vector3.up);
            body.constraints = (RigidbodyConstraints)80;
            ChangeAnimationState(PlayerState.Walking);
        }
        else
        {
            body.constraints = (RigidbodyConstraints)112;
            ChangeAnimationState(PlayerState.Idle);

        }
    }

    private void StartRotateToMouse()
    {
        rotateToMouse = true;
    }

    private void StopRotationToMouse()
    {
        rotateToMouse = false;
    }

    private void ChangeAnimationState(PlayerState state)
    {
        if (stateSetter != null)
            stateSetter.Set(state);
    }
}
