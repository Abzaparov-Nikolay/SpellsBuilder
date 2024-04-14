//using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyDD : NetworkBehaviour
{
    //todo change multiplier from reference to global syncvar int Level of enemies
    [SerializeField] private Reference<float> baseDamage;
    [SerializeField] private Reference<float> damageMultiplier;
    [SerializeField] private Team team;
    private void OnTriggerStay(Collider collider)
    {
        if(!IsServer) return;
        if(collider.gameObject.activeInHierarchy
            && collider.gameObject.TryGetComponentInParent<TeamMember>(out var otherTeam)
            && otherTeam.isHostileTo(team)
            && collider.gameObject.TryGetComponentInParent<DamageReceiver>(out var receiver))
        {
            receiver.TakeDamage(baseDamage * damageMultiplier);
        }
    }
}
