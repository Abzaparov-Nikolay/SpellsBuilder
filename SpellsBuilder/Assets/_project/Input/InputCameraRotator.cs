using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCameraRotator : MonoBehaviour
{

    public static Action RotateLeft;
    public static Action StopRotateLeft;
    public static Action RotateRight;
    public static Action StopRotateRight;

    public void OnLeftHold(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            RotateLeft?.Invoke();
            Debug.Log("LETF START");
        }
        if(context.canceled)
        {
            Debug.Log("LETF STOP");

            StopRotateLeft?.Invoke();
        }
    }

    public void OnRightHold(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RotateRight?.Invoke();

        }
        if (context.canceled)
        {
            StopRotateRight?.Invoke();
        }
    }
}
