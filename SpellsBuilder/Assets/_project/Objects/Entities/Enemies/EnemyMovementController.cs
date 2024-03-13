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

    private void FixedUpdate()
    {
        if (!IsServer) return;
        //var playerPosition = PlayersTracker.Instance.GetNearest(transform);
        //if (playerPosition != null)
        body.velocity = directionProvider.GetMoveDirection() * moveSpeed;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer)
            body.isKinematic = true;
    }




}
