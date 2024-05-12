using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Mover : NetworkBehaviour
{
    [SerializeField] private Reference<float> flySpeed;



    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;
        transform.position += transform.forward * flySpeed * Time.deltaTime;
    }
}
