using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] Camera _camera;
    private static Camera holdedCamera;
    private static Plane plane;

    public static Vector3 ScreenToWorld(Vector3 pos)
    {
        var ray = holdedCamera.ScreenPointToRay(pos);
        if (plane.Raycast(ray, out var enter))
        {
            return ray.GetPoint(enter);
        }
        return Vector3.zero;
        //return holdedCamera.ScreenToWorldPoint(pos);
    }

    public void Awake()
    {
        holdedCamera = _camera;
        plane = new Plane(Vector3.up, new Vector3(0, 1, 0));
    }

    public void OnDestroy()
    {
        holdedCamera = null;
        //plane = null;
    }
}
