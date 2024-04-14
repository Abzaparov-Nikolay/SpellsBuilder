using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticleSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem system;
    private void Start()
    {
        system.Play();
    }
}
