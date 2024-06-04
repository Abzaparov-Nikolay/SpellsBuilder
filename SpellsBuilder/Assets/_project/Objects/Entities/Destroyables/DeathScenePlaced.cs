using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class DeathScenePlaced : NetworkBehaviour
{
    public UnityEvent OnDeath;
    [HideInInspector] public UnityEvent<GameObject> OnDeathWithGameObject;

    public void Die()
    {
        if (!IsServer) return;
        OnDeath?.Invoke();
        OnDeathWithGameObject?.Invoke(this.gameObject);
        ServerDie(gameObject);
    }

    private void ServerDie(GameObject obj)
    {
        if (!IsServer) return;
        NetworkObject.Despawn(obj);
    }
}
