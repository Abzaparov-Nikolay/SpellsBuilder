using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Puller : NetworkBehaviour
{
    public Reference<float> force;
    //public AreaTracker tracker;

    public NetworkVariable<bool> PullOut = new(false);
    public NetworkVariable<float> PullForceMultiplier = new();


    private Multiplier pullForceMultiplier = new Multiplier();

    public void SetPull(bool pullOut)
    {
        if (!IsServer) return;
        PullOut.Value = pullOut;
    }

    public void AddPullBonus(float pullBonus)
    {
        if (!IsServer) return;
        pullForceMultiplier.Add(pullBonus);
    }

    private void Update()
    {
        ////if (!IsServer) return;
        //foreach (var obj in new List<Transform>())
        //{
        //    if (obj == null) continue;

        //    if (!obj.gameObject.TryGetComponentInParent<NetworkObject>(out var no))
        //        continue;

        //    if (no.OwnerClientId != NetworkManager.Singleton.LocalClientId)
        //        continue;

        //    if (obj.gameObject.TryGetComponentInParent<Rigidbody>(out var rb))
        //    {
        //        var dif = (transform.position - obj.position);
        //        var dir = new Vector3(dif.x, 0, dif.z).normalized;
        //        //var distance = dif.magnitude;
        //        var multiplier = PullOut.Value ? -1 : 1;
        //        rb.AddForce(multiplier * PullForceMultiplier.Value * dir * force, ForceMode.Impulse);
        //    }
        //}
    }

    public void StartPull(GameObject obj)
    {
        if (!IsServer) return;
        if (obj.gameObject.TryGetComponentInParent<Pullable>(out var pullable))
        {
            var multiplier = PullOut.Value ? -1 : 1;
            pullable.StartPullClientRpc(new NetworkObjectReference(base.NetworkObject), multiplier * PullForceMultiplier.Value * force);
        }
    }

    public void StopPull(GameObject obj)
    {
        if (!IsServer) return;
        if (obj.gameObject.TryGetComponentInParent<Pullable>(out var pullable))
        {
            pullable.StopPullClientRpc(new NetworkObjectReference(base.NetworkObject));
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        UpdateMultiplier();
        pullForceMultiplier.OnChange += UpdateMultiplier;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        pullForceMultiplier.OnChange -= UpdateMultiplier;
    }

    private void UpdateMultiplier()
    {
        if (!IsServer) return;
        PullForceMultiplier.Value = pullForceMultiplier;
    }
}
