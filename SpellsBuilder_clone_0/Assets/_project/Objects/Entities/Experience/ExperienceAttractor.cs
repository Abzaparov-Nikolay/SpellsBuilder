//using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ExperienceAttractor : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        if (other.TryGetComponent<ExpParticle>(out var particle))
        {
            particle.StartAttraction(transform);
        }
    }
}
