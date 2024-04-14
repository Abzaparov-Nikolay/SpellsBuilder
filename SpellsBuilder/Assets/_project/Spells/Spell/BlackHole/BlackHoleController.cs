using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BlackHoleController : NetworkBehaviour
{
    [SerializeField] private Reference<float> moveSpeed;
    private Vector3 moveDirection = Vector3.zero;
    private NetworkVariable<bool> move = new(true);


    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;
        if (move.Value)
        {
            if(moveDirection != Vector3.zero)
            {
                transform.position += moveDirection * Time.deltaTime;
            }
            else
            {
                transform.position += new Vector3(Random.Range(0, 1), 0, Random.Range(0, 1)).normalized
                * moveSpeed
                * Time.deltaTime;
            }
        }
    }

    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    public void StopMoving()
    {
        move.Value = false;
    }
}
