//using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyAggrer : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        if(other.gameObject.activeInHierarchy 
            && other.gameObject.TryGetComponentInParent<MoveDirectionDeterminator>(out var enemyCtrl))
        {
            enemyCtrl.AddAggroSource(this.gameObject);
        }
    }
}
