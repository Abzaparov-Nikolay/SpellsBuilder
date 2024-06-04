using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TakeDamageAnimator : NetworkBehaviour
{
    [SerializeField] private Animation animation;
    // Start is called before the first frame update
    

    public void Animate()
    {
        if(!IsServer) return;
        AnimateClientRpc();
    }

    [ClientRpc]
    public void AnimateClientRpc()
    {
        animation.Play();
    }
}
