using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{
    private void Start()
    {
        return;
        if (!NetworkManager.Singleton.IsServer)
            return;

        List<Transform> inScenePlacedObjects = new List<Transform>();
        foreach (Transform child in transform)
        {
            inScenePlacedObjects.Add(child);
        }
        foreach (Transform child in inScenePlacedObjects)
        {
            SpawnInPlace(child.gameObject);
            Destroy(child.gameObject);

        }
    }

    private void SpawnInPlace(GameObject gameObject)
    {
        var spawned = Instantiate(gameObject,
            gameObject.transform.position,
            gameObject.transform.rotation,
            this.transform);
        spawned.GetComponent<NetworkObject>().Spawn(true);
        spawned.GetComponent<NetworkObject>().TrySetParent(this.gameObject, true);
    }


}
