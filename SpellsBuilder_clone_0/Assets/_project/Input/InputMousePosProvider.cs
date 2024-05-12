//using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputMousePosProvider : MonoBehaviour
{
    private Vector3 output;
    private InputAction.CallbackContext lastCallback;
    private static InputMousePosProvider instance;
    private void Awake()
    {
        instance = this;
    }
    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public void OnMousePosChange(InputAction.CallbackContext callbackContext)
    {
        lastCallback = callbackContext;
    }

    public void Recalculate()
    {
        var screen = lastCallback.ReadValue<Vector2>();
        output = CameraHolder.ScreenToWorld(screen);
    }

    public static Vector3 Get()
    {
        instance.Recalculate();
        return instance.output;
    }
}
