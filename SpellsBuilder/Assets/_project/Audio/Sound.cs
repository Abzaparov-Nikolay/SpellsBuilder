using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Sound : NetworkBehaviour
{
    [SerializeField] private ShitAssSound[] sounds;
    [SerializeField] private AudioSource source;

    public void PlaySound(string soundName)
    {
        if (!IsServer) return;
        PLayClientRpc(soundName, false);
    }

    public void PlayDestroying(string name)
    {
        if(!IsServer) return;
        PLayClientRpc(name, true);
    }

    [ClientRpc]
    public void PLayClientRpc(string name, bool destroying)
    {
        var sas = sounds.FirstOrDefault(s => s.name == name);
        if (sas.sound != null)
        {
            if (!destroying)
            {
                source.PlayOneShot(sas.Get(), sas.volume);
            }
            else
            {
                AudioSource.PlayClipAtPoint(sas.Get(), transform.position, sas.volume);

            }
        }
    }


}

[Serializable]
public struct ShitAssSound
{
    public string name;
    public AudioClip[] sound;
    public float volume;

    public ShitAssSound(string name, AudioClip clip, float volume)
    {
        this.name = null;
        this.volume = 1;
        this.sound = null;
    }

    public AudioClip Get()
    {
        return sound[UnityEngine.Random.Range(0, sound.Length)];
    }
}
