//using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OnFirstSpanw : NetworkBehaviour
{
    public static event Action<Transform> OnObjectClienSpawn;
    public static event Action<Transform> OnObjectServerSpawn;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (base.IsOwner)
        {
            OnObjectClienSpawn?.Invoke(transform);
        }
        OnObjectServerSpawn?.Invoke(transform);
    }

    //public override void OnStartNetwork()
    //{
    //    base.OnStartNetwork();
    //    if (!IsServer) return;
    //    OnObjectServerSpawn?.Invoke(transform);
    //}
}
