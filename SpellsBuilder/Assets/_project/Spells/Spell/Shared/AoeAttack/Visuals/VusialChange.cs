using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class VusialChange : NetworkBehaviour
{
    [SerializeField] private GameObject damageSystem;
    [SerializeField] private GameObject healSystem;

    private bool heal;
    private bool attack;

    public void Heal()
    {
        heal = true;
    }

    public void Attack()
    {
        attack = true;
    }

    public void Play()
    {
        if (!IsServer) return;
        PlayClientRpc(this.attack, this.heal);
    }

    [ClientRpc]
    public void PlayClientRpc(bool attack, bool heal)
    {
        if (heal)
        {
            healSystem.SetActive(true);
        }
        else if (attack)
        {
            damageSystem.SetActive(true);
        }
        else
        {

        }
    }
}
