//using FishNet.Managing.Server;
//using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DamageTextSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private Reference<Vector3> positionVariation;
    [SerializeField] private Reference<Vector3> velocityVariation;
    public void Spawn(float damage)
    {
        if (!base.IsServer) return;
        var pos = transform.position + new Vector3(0, 1.5f, 0);
        pos += new Vector3(Random.Range(-positionVariation.Get().x, positionVariation.Get().x),
                            Random.Range(-positionVariation.Get().y, positionVariation.Get().y),
                            Random.Range(-positionVariation.Get().z, positionVariation.Get().z));
        var velocity = new Vector3(Random.Range(-velocityVariation.Get().x, velocityVariation.Get().x),
                                    Random.Range(-velocityVariation.Get().y, velocityVariation.Get().y),
                                    Random.Range(-velocityVariation.Get().z, velocityVariation.Get().z));
        SpawnServer(damage, velocity, damageTextPrefab, pos, Quaternion.identity);
    }

    
    private void SpawnServer(float damage, Vector3 velocity, GameObject text, Vector3 pos, Quaternion rot)
    {
        if (!base.IsServer) return;
        var newText = Instantiate(text, pos, Quaternion.identity);
        var data = new DamageTextData(damage, Color.white, velocity);

        newText.GetComponent<DamageTextController>().data = data;
        newText.GetComponent<DamageTextController>().RotateToCamera();
        newText.GetComponent<DamageTextController>().DirtyStart();

        newText.GetComponent<NetworkObject>().Spawn(true);
        //SetTextDataClientRpc(newText.GetComponent<DamageTextController>(), data);
    }

    //[ClientRpc]
    //private void SetTextDataClientRpc(DamageTextController cntr, DamageTextData data)
    //{
    //    cntr.data = data;
    //    cntr.RotateToCamera();
    //    cntr.DirtyStart();
    //}
}
