using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnabler : MonoBehaviour
{
    [SerializeField] private GameObject[] toEnable;

    void Start()
    {
        foreach(var go in toEnable)
        {
            go.SetActive(true);
        }
    }
}
