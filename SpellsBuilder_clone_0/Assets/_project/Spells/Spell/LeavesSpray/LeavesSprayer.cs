//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class LeavesSprayer : NetworkBehaviour
{
    [SerializeField] private GameObject leafPrefab;

    [SerializeField] private LeavesSprayDefaultValues defaultValues;

    public List<ElementType> leavesModificators;

    private NetworkVariable<float> fireRate = new();
    private NetworkVariable<float> firerateMultiplier = new();
    private NetworkVariable<float> degreesOfAttack = new();
    private NetworkVariable<float> degreesOfAttackMultiplier = new();
    private NetworkVariable<float> leafImpulse = new();
    private NetworkVariable<float> leafImpulseMultiplier = new();

    private float timeSinceLastAttack;

    private void Update()
    {
        if (!IsServer) return;

        var totalAttackInterval = 1 / (fireRate.Value * firerateMultiplier.Value);
        timeSinceLastAttack += Time.deltaTime;
        while (timeSinceLastAttack > totalAttackInterval)
        {
            timeSinceLastAttack -= totalAttackInterval;
            PerformAttack(timeSinceLastAttack);
        }
        //spawn leaves
    }
    private void PerformAttack(float timeSinceLastAttack)
    {
        if (!IsServer) return;

        SpawnLeaf(leafPrefab,
            leavesModificators,
            transform.position + timeSinceLastAttack * transform.forward,
            transform.rotation
            * Quaternion.AngleAxis(
                UnityEngine.Random.Range(-degreesOfAttack.Value * degreesOfAttackMultiplier.Value / 2
                , degreesOfAttack.Value * degreesOfAttackMultiplier.Value / 2)
                , Vector3.up),
            leafImpulse.Value * leafImpulseMultiplier.Value);
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        fireRate.Value = defaultValues.fireRate;
        firerateMultiplier.Value = defaultValues.firerateMultiplier;
        degreesOfAttack.Value = defaultValues.degreesOfAttack;
        degreesOfAttackMultiplier.Value = defaultValues.degreesOfAttackMultiplier;
        leafImpulse.Value = defaultValues.leafImpulse;
        leafImpulseMultiplier.Value = defaultValues.leafImpulseMultiplier;
        //leavesModificators = elements.Where(mod => mod);
    }

    private void SpawnLeaf(GameObject leaf,
        List<ElementType> modifiers,
        Vector3 position,
        Quaternion rotation,
        float impulse)
    {
        var spawned = Instantiate(leaf, position, rotation);
        spawned.GetComponent<SpellConfigurator>().SetModifiers(modifiers.ToList());
        spawned.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, true);

        spawned.GetComponent<Rigidbody>().AddForce(spawned.transform.forward * impulse, ForceMode.Impulse);
    }

    public void AddFirerateBonus(float percentage)
    {
        if (!IsServer) return;
        firerateMultiplier.Value += percentage;
    }

    public void AddAccuracyBonus(float percentage)
    {
        if (!IsServer) return;
        degreesOfAttackMultiplier.Value += percentage;
        if (degreesOfAttackMultiplier.Value < 0)
            degreesOfAttackMultiplier.Value = 0;
    }

    public void AddLeafImpulseBonus(float percentage)
    {
        if (!IsServer) return;
        leafImpulseMultiplier.Value += percentage;
    }
}
