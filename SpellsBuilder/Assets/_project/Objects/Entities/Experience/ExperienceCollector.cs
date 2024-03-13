//using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ExperienceCollector : NetworkBehaviour
{
    [SerializeField] ExperienceManager playerExperience;
    private void OnTriggerEnter(Collider other)
    {
        if(!IsServer) return;
        if(other.TryGetComponent<ExpParticle>(out var particle))
        {
            ExperienceManager.AddAll(particle.amount);
            particle.Die();
        }
    }
}
