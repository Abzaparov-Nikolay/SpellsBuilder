//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class SpawnAttacker : NetworkBehaviour
{
    [SerializeField] private SpawnAttackerBase BaseStats;

    [SerializeField]
    private NetworkVariable<float> AttackRate;

    private NetworkVariable<float> AttackRateMultiplier = new();
    private readonly Multiplier attackRateMultipliers = new();

    private GameObject AttackPrefab;
    private AttackSpawnType AttackSpawnType;
    private List<ElementType> modifiersToPass;

    [SerializeField] private TargetSelector targetSelector;
    private float timeSinceLastAttack;
    private float spawnImpulse = 5;

    private void Update()
    {
        if (!IsServer) return;
        if (targetSelector.TryGetTarget(out var targets))
        {
            var totalAttackInterval = 1 / (AttackRate.Value * AttackRateMultiplier.Value);
            timeSinceLastAttack += Time.deltaTime;
            while (timeSinceLastAttack > totalAttackInterval)
            {
                timeSinceLastAttack -= totalAttackInterval;
                PerformAttack(timeSinceLastAttack, targets.First());
            }
        }
    }

    private void PerformAttack(float elapsedTime, Transform target)
    {
        if (!IsServer) return;
        GameObject spawned = null;
        if (AttackPrefab == null) return;
        switch (AttackSpawnType)
        {
            case (AttackSpawnType.Shoot):
                var dir = (target.position - transform.position).normalized;
                dir.y = 0;
                dir.Normalize();
                spawned = Instantiate(AttackPrefab,
                    transform.position + Vector3.up + dir * spawnImpulse * elapsedTime,
                    Quaternion.LookRotation(dir));
                //spawned.GetComponent<NetworkObject>().Spawn(true);
                //spawned.transform.position += spawned.transform.forward * spawnImpulse * elapsedTime;
                if (spawned.TryGetComponent<Rigidbody>(out var body))
                {
                    body.AddForce(spawnImpulse * spawned.transform.forward, ForceMode.Impulse);
                }
                break;
            case (AttackSpawnType.Trap):
                var pos = target.transform.position;
                pos.y = 0;
                spawned = Instantiate(AttackPrefab, pos,
                    Quaternion.LookRotation((target.position - transform.position).normalized));
                //spawned.GetComponent<NetworkObject>().Spawn(true);
                break;
            case (AttackSpawnType.Aoe):
                spawned = Instantiate(AttackPrefab, transform.position,
                    Quaternion.identity);
                //spawned.GetComponent<NetworkObject>().Spawn(true);
                break;
        }
        if (spawned != null)
        {
            //ServerManager.Spawn(spawned);
            spawned.GetComponent<NetworkObject>().Spawn(true);
            spawned.GetComponent<SpellConfigurator>().SetModifiers(modifiersToPass);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        attackRateMultipliers.OnChange += UpdateMultiplier;
        SetBaseAttackRate();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        attackRateMultipliers.OnChange -= UpdateMultiplier;

    }

    private void UpdateMultiplier()
    {
        AttackRateMultiplier.Value = attackRateMultipliers;
    }

    private void SetBaseAttackRate()
    {
        AttackRate.Value = BaseStats.attackRates
            .First(pair => pair.AttackSpawnType == AttackSpawnType).AttackRate;
    }

    public void AddBonusAttackRate(float percentage)
    {
        if (!IsServer) return;
        attackRateMultipliers.Add(percentage);
    }

    public void Configure(float attackRateBuffPercentes,
        GameObject attackPrefab,
        AttackSpawnType spawnType,
        List<ElementType> modifiers)
    {
        attackRateMultipliers.Add(attackRateBuffPercentes);
        AttackSpawnType = spawnType;
        AttackPrefab = attackPrefab;
        modifiersToPass = modifiers;
    }

    public bool HasAttack()
    {
        return AttackPrefab != null;
    }
}
public enum AttackSpawnType
{
    Shoot,
    Trap,
    Aoe
}

[Serializable]
public class SpawnTypeAttackRate
{
    public AttackSpawnType AttackSpawnType;
    public float AttackRate;
}
