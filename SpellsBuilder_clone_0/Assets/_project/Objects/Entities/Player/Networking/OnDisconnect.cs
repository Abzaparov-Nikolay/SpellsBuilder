//using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OnDisconnect : NetworkBehaviour
{
    public static Action<Transform> OnObjectDisconnect;

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        OnObjectDisconnect?.Invoke(transform);
    }
}
