using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Teleport : NetworkBehaviour
{
    [SerializeField] private List<Team> teamsTracked;
    [SerializeField] private Transform teleportPoint;
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.activeInHierarchy
            && other.gameObject.TryGetComponentInParent<TeamMember>(out var otherTeam)
            //&& otherTeam.isHostileTo(team)
            && teamsTracked.Contains(otherTeam.Team))
            //&& other.gameObject.TryGetComponentInParent<Rigidbody>(out var rb))
        {
            if (!otherTeam.gameObject.GetComponent<NetworkObject>().IsOwner && !IsServer) return;

            //otherTeam.gameObject.GetComponent<Rigidbody>();
            TeleportThisIdiot(otherTeam.gameObject.GetComponent<Rigidbody>());
            //entitiesInRadius.Add(other.transform);
        }
    }


    public void TeleportThisIdiot(Rigidbody rb)
    {
        var vel = rb.velocity;
        var angVel = rb.angularVelocity;
        var kinemati = rb.isKinematic;
        rb.isKinematic = true;
        rb.Sleep();
        //rb.gameObject.SetActive(false);

        rb.position = teleportPoint.position;

        //rb.gameObject.SetActive(true);
        rb.WakeUp();
        rb.isKinematic = kinemati;
        rb.velocity = vel;
        rb.angularVelocity = angVel;


    }
}
