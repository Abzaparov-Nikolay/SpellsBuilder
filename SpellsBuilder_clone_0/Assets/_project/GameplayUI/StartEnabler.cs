using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class StartEnabler : MonoBehaviour
{
    [SerializeField] private GameObject[] toEnable;

    void Start()
    {
        foreach (var go in toEnable)
        {
            go.SetActive(true);
        }
    }
}
