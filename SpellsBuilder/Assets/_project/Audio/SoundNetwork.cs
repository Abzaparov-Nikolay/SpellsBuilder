using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Sound))]
public class SoundNetwork : NetworkBehaviour
{
    [SerializeField] private Sound sound;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        sound.SoundPlaying += Share;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        sound.SoundPlaying -= Share;
    }

    public void Share(string name, bool destroying)
    {
        if (!IsServer) return;
        PlayClientRpc(name, destroying);
    }

    [ClientRpc]
    private void PlayClientRpc(string name, bool destroying)
    {
        if(IsServer) return;
        if (destroying)
        {
            sound.PlayDestroying(name);
        }
        else
        {
            sound.PlaySound(name);
        }
    }
}
