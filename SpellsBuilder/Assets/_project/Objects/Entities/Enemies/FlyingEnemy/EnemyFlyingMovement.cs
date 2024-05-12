using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyFlyingMovement : NetworkBehaviour
{
    [SerializeField] private Reference<float> moveSpeed;
    [SerializeField] private Rigidbody body;
    [SerializeField] private MoveDirectionDeterminator directionProvider;
    [SerializeField] private Reference<float> attackRange;
    private bool stop = false;

    private void Update()
    {
        if (!IsServer) return;
        var dir = directionProvider.GetMoveDirection().NoY().normalized;
        var distance = directionProvider.GetDistance();

        dir = new Vector3(dir.x, 0, dir.z).normalized;
        body.velocity = dir * moveSpeed;
        body.rotation = Quaternion.LookRotation(dir, Vector3.up);
        if (stop)
        {
            body.velocity = Vector3.zero;
            return;
        }
        if (distance < attackRange && distance > 0)
        {
            body.velocity = Vector3.zero;
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer)
            body.isKinematic = true;
        GameStater.GamePaused += Stop;
        GameStater.GameUnpaused += Go;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        GameStater.GamePaused -= Stop;
        GameStater.GameUnpaused -= Go;
    }

    private void Stop()
    {
        if (!IsServer) return;
        stop = true;
    }

    private void Go()
    {
        if (!IsServer) return;
        stop = false;
    }
}
