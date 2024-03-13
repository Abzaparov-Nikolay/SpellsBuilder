//using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class OnTriggerEvent : NetworkBehaviour
{
    [SerializeField] private Team team;
    [SerializeField] private UnityEvent<GameObject> onTriggerEnter;
    [SerializeField] private UnityEvent<GameObject> onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        if (other.gameObject.activeInHierarchy
            && other.gameObject.TryGetComponentInParent<TeamMember>(out var member)
            && member.isHostileTo(team))
        {
            onTriggerEnter?.Invoke(other.gameObject);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer) return;
        if (other.gameObject.activeInHierarchy
            && other.gameObject.TryGetComponentInParent<TeamMember>(out var member)
            && member.isHostileTo(team))
            onTriggerExit?.Invoke(other.gameObject);
    }
}
