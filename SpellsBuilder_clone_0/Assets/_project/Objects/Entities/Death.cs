//using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Death : NetworkBehaviour
{
    public UnityEvent OnDeath;
    [HideInInspector] public UnityEvent<GameObject> OnDeathWithGameObject;
    public bool DestroyOnDeath = true;
    public void Die()
    {
        if (!IsServer) return;
        OnDeath?.Invoke();
        OnDeathWithGameObject?.Invoke(this.gameObject);
        if (DestroyOnDeath)
        {
            ServerDie(gameObject);
        }
        else
        {
            ServerSetDisabled(gameObject);
        }
    }

    //[ServerRpc(RequireOwnership = false)]
    private void ServerDie(GameObject obj)
    {
        if (!IsServer) return;
        Destroy(obj);
        //base.NetworkObject.Despawn(true);
        //ServerManager.Despawn(obj);
        //Destroy(obj);
    }

    //[ServerRpc(RequireOwnership = false)]
    private void ServerSetDisabled(GameObject obj)
    {
        if (!IsServer) return;
        //ServerManager
        obj.SetActive(false);
        //ServerManager.Despawn(obj);
        //Destroy(obj);
    }
}
