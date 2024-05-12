//using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DamageTextController : NetworkBehaviour
{
    [HideInInspector] public DamageTextData data;
    [SerializeField] private TMPro.TextMeshPro tmp;
    [HideInInspector] public Vector3 velocity;
    [SerializeField] private Reference<float> lifetime;
    //[SerializeField] private MeshFilter filter;
    [SerializeField] private Reference<GameObject> cache;
    [SerializeField] private Variable<Transform> cameraPos;

    private void Start()
    {
        //filter.mesh = cache.Get().GetComponent<DamageTextDrawer>().GetNumberMesh(data);
        //tmp.text = data.number.ToString();

    }


    private void Update()
    {
        transform.position += velocity;
    }

    public void DirtyStart()
    {
        transform.SetSiblingIndex(0);

        string niceTime = string.Format("{0:0.0}", Mathf.Abs(data.number));

        tmp.text = $"{(data.number > 0 ? "" : "+")}{niceTime}";
        RotateToCamera();
        if (data.number < 0)
        {
            tmp.color = new Color(79 / 255, 255 / 255, 54 / 255);
            tmp.color = new Color(101 / 255, 255 / 255, 100 / 255);
        }
    }

    public void RotateToCamera()
    {
        var dirToCam = (cameraPos.Value.position - transform.position).normalized;
        transform.LookAt(dirToCam);//.rotation.SetLookRotation(dirToCam);
        transform.rotation = Quaternion.Euler(new Vector3(33, -37, 0));//.SetEulerRotation(new Vector3(0,-45,0));
    }

    
}
