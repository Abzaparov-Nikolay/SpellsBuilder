//using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyMovementController : NetworkBehaviour
{
    [SerializeField] private Reference<float> moveSpeed;
    [SerializeField] private Rigidbody body;
    [SerializeField] private MoveDirectionDeterminator directionProvider;
    private bool stop = false;

    private void Update()
    {
        if (!IsServer) return;
        body.velocity = directionProvider.GetMoveDirection() * moveSpeed;

        if (stop)
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
        if(!IsServer) return;
        stop = true;
    }

    private void Go()
    {
        if (!IsServer) return;
        stop = false;
    }


}
