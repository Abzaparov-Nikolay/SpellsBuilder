using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Reference<Transform> mainCamera;

    private void OnEnable()
    {
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.Get().forward);
    }
}
