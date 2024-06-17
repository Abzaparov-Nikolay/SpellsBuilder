using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class AoeSpawner : NetworkBehaviour
{
    public GameObject aoePrefab;
    [HideInInspector] public List<ElementType> modifiersToPass;

    public void Spawn()
    {
        if (!IsServer) return;
        var spawned = Instantiate(aoePrefab, transform.position, Quaternion.identity);
        spawned.GetComponent<SpellConfigurator>().SetModifiers(modifiersToPass.ToList());
        spawned.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, true);
    }
}
