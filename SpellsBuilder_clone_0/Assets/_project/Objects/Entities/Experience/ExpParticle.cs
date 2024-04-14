//using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ExpParticle : NetworkBehaviour
{
    public float amount;
    private Transform playerTransform;
    [SerializeField] private Reference<float> MoveSpeed;
    [SerializeField] private Death deathSource;
    private bool attracted;

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        if (attracted && !playerTransform.IsDestroyed())
        {
            transform.position += (playerTransform.position - transform.position).normalized * MoveSpeed * Time.deltaTime;
        }
    }

    public void StartAttraction(Transform player)
    {
        playerTransform = player;
        attracted = true;
    }

    public void Die()
    {
        deathSource.Die();
    }
}
