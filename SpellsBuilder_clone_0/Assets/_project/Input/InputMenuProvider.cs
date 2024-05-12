using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMenuProvider : MonoBehaviour
{
    public static Action EscapePressed;
    public void OnEscapeClick(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            EscapePressed?.Invoke();
        }
    }
}
