using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyAttacker : NetworkBehaviour
{
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private MoveDirectionDeterminator dirDeter;
    [SerializeField] private Reference<float> attackRate;
    [SerializeField] private Reference<float> attackRange;
    private float lastAttackSec = 0;

    private void Update()
    {
        if (!IsServer) return;
        if (!(lastAttackSec > 1 / attackRate))
        {
            lastAttackSec += Time.deltaTime;
        }
        var distance = dirDeter.GetDistance();
        if (distance < attackRange)
        {
            if (lastAttackSec >= 1 / attackRate)
            {
                PerformAttack();
                lastAttackSec = 0;
            }
        }
    }

    private void PerformAttack()
    {
        //var target = dirDeter.GetTarget();
        var target = dirDeter.GetTarget();
        if (target != null)
        {
            var rota = Quaternion.LookRotation(target.position + Vector3.up - attackPosition.position, Vector3.up);
            var spawned = Instantiate(attackPrefab, attackPosition.position, rota);
            spawned.GetComponent<NetworkObject>().Spawn();
        }
    }
}
