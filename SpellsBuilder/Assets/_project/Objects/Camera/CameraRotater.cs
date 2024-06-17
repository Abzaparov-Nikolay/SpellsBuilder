using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotater : MonoBehaviour
{
    public void StartLeft()
    {
        StartCoroutine(Rotate(true));
    }

    public void StopLeft()
    {
        StopAllCoroutines();
    }

    public void StartRight()
    {
        StartCoroutine(Rotate(false));
    }

    public void StopRight()
    {
        StopAllCoroutines();
    }

    private IEnumerator Rotate(bool left)
    {
        while (true)
        {
            transform.RotateAround(transform.position, Vector3.up, left ? -2 : 2);
            //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + 1, transform.rotation.z);
            yield return null;
        }
    }

    private void Start()
    {
        InputCameraRotator.RotateLeft += StartLeft;
        InputCameraRotator.StopRotateLeft += StopLeft;
        InputCameraRotator.RotateRight += StartRight;
        InputCameraRotator.StopRotateRight += StopRight;

    }

    private void OnDestroy()
    {
        InputCameraRotator.RotateLeft -= StartLeft;
        InputCameraRotator.StopRotateLeft -= StopLeft;
        InputCameraRotator.RotateRight -= StartRight;
        InputCameraRotator.StopRotateRight -= StopRight;
    }
}
