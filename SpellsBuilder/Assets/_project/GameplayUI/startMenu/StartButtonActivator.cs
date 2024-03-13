//using FishNet.Connection;
//using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonActivator : NetworkBehaviour
{
    [SerializeField] private Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        SpawnerPlayers.OnSpawned += DeactivateButton;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        SpawnerPlayers.OnSpawned -= DeactivateButton;
    }

    void DeactivateButton(NetworkObject conn)
    {
        if (!base.IsServer) return;
        deactivateOnClientClientRpc(conn.OwnerClientId);
    }

    [ClientRpc]
    void deactivateOnClientClientRpc(ulong conn)
    {
        if(OwnerClientId == conn) 
        {
            
            this.gameObject.SetActive(false);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        this.gameObject.SetActive(true);
    }
}
