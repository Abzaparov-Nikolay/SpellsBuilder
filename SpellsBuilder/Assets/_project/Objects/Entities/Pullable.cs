using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Pullable : NetworkBehaviour
{
    [SerializeField] private Rigidbody body;

    private List<(Transform, float)> pullSourcesAndPowers = new();

    [ClientRpc]
    public void StartPullClientRpc(NetworkObjectReference target, float pullPower)
    {
        if (target.TryGet(out NetworkObject networkObject))
        {
            pullSourcesAndPowers.Add((networkObject.transform, pullPower));
        }
        ClearDestroyed();
    }

    [ClientRpc]
    public void StopPullClientRpc(NetworkObjectReference target)
    {
        if (target.TryGet(out NetworkObject networkObject))
        {
            pullSourcesAndPowers.RemoveAt(
                pullSourcesAndPowers.FindIndex(
                    tuple => tuple.Item1 == networkObject.transform));
        }
        ClearDestroyed();
    }

    private void ClearDestroyed()
    {

        for (var i = pullSourcesAndPowers.Count - 1; i >= 0; i--)
        {
            if (pullSourcesAndPowers[i].Item1 == null)
            {
                pullSourcesAndPowers.RemoveAt(i);
            }
        }
    }

    private void Update()
    {
        if (!IsOwner) return;
        foreach (var obj in pullSourcesAndPowers)
        {
            if (obj.Item1 == null) continue;
            var dif = (obj.Item1.position - transform.position);
            var dir = new Vector3(dif.x, 0, dif.z).normalized;
            body.AddForce(obj.Item2 * dir, ForceMode.Impulse);
        }
    }
}
