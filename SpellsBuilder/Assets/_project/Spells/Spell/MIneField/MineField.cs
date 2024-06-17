using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class MineField : NetworkBehaviour
{
    [SerializeField] Reference<float> spawnRadius;
    [SerializeField] Reference<float> maxCount;
    [SerializeField] private GameObject minePrefab;
    private List<ElementType> modifiersToPass;

    private List<Transform> spawnedMines = new();

    public void Configure(List<ElementType> modifiers)
    {
        modifiersToPass = modifiers.ToList();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        StartCoroutine(SpawnMine());

    }

    private void Spawn()
    {
        if (!IsServer) return;
        var spawned = Instantiate(minePrefab, transform.position + GetPos(), Quaternion.identity);
        spawnedMines.Add(spawned.transform);
        spawned.GetComponent<SpellConfigurator>().SetModifiers(modifiersToPass.ToList());
        spawned.GetComponent<Death>().OnDeathWithGameObject.AddListener(MineExploded);
        spawned.GetComponent<NetworkObject>().Spawn();
    }

    private Vector3 GetPos()
    {
        return new Vector3(UnityEngine.Random.Range(-spawnRadius, spawnRadius),
            0,
            UnityEngine.Random.Range(-spawnRadius, spawnRadius));
    }

    private void MineExploded(GameObject mine)
    {
        spawnedMines.Remove(mine.transform);
    }

    private IEnumerator SpawnMine()
    {
        yield return new WaitForSeconds(1);

        while (true)
        {
            if (spawnedMines.Count < maxCount)
            {
                Spawn();
            }
            yield return new WaitForSeconds(1);
        }
    }
}
