using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class ChargeSpawner : NetworkBehaviour
{
    //[SerializeField] private Lifetime lifetime;
    [HideInInspector] public List<ElementType> modifiers;
    [SerializeField] private GameObject prefab;

    public void Spawn()
    {
        if (!IsServer) return;
        var spawned = Instantiate(prefab, transform.position, transform.rotation);
        if (spawned.TryGetComponent<BlackHoleController>(out var contrl))
        {
            contrl.SetMoveDirection(transform.forward);
        }
        spawned.GetComponent<SpellConfigurator>().SetModifiers(modifiers.ToList());
        spawned.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, true);

    }
}
