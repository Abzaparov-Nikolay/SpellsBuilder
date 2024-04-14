//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AreaTracker : NetworkBehaviour, IEnumerable<Transform>
{
    [SerializeField] private Collider _collider;
    [SerializeField] private Reference<float> radiusBase;


    private NetworkVariable<float> radiusMultiplier = new();
    private readonly Multiplier radiusMultipliers = new();
    [SerializeField] private List<Team> teamsToTrack;
    private readonly HashSet<Transform> entitiesInRadius = new();

    private void Start()
    {
        if (_collider == null)
            _collider = GetComponent<Collider>();
        if (!_collider.isTrigger)
            _collider.isTrigger = true;
    }

    private void Awake()
    {
        radiusMultiplier.OnValueChanged += OnMultiplierChange;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        radiusMultiplier.OnValueChanged += OnMultiplierChange;
    }

    public void AddBonus(float percentage)
    {
        if (!IsServer) return;
        radiusMultipliers.Add(percentage);
        radiusMultiplier.Value = radiusMultipliers;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        radiusMultiplier.Value = radiusMultipliers;
    }

    private void OnMultiplierChange(float prev, float next)
    {
        //if (!asServer) radiusMultiplier = next;
        if (_collider is SphereCollider)
        {
            (_collider as SphereCollider).radius = radiusBase * radiusMultiplier.Value;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        if (other.gameObject.activeInHierarchy
            && other.gameObject.TryGetComponentInParent<TeamMember>(out var otherTeam)
            //&& otherTeam.isHostileTo(team)
            && teamsToTrack.Contains(otherTeam.Team))
        {
            entitiesInRadius.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer) return;
        entitiesInRadius.Remove(other.gameObject.transform);
        RemoveDestroyed();
    }

    private void RemoveDestroyed()
    {
        entitiesInRadius.Remove(null);
    }

    public IEnumerator<Transform> GetEnumerator()
    {
        return entitiesInRadius.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
