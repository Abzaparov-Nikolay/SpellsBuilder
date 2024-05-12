using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyAnimator : NetworkBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody body;
    [SerializeField] private MoveDirectionDeterminator determinator;
    [SerializeField] private Reference<float> attackRange;

    private void Update()
    {
        if (!IsServer)
            return;
        if (body.velocity != Vector3.zero)
        {
            Move();
        }
        else
        {
            StopMove();
        }
        var distance = determinator.GetDistance();
        if (distance < attackRange && distance > 0)
        {
            Attack();
        }
    }

    public void Attack()
    {
        anim.SetBool("Attacking", true);
        StartCoroutine(StopAttack());
    }

    public void Move()
    {
        anim.SetBool("Walking", true);
    }

    public void StopMove()
    {
        anim.SetBool("Walking", false);
    }

    public IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Attacking", false);
    }
}
