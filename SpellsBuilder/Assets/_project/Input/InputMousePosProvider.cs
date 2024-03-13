//using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputMousePosProvider : NetworkBehaviour
{
    private Vector3 output;
    private InputAction.CallbackContext lastCallback;

    public void OnMousePosChange(InputAction.CallbackContext callbackContext)
    {
        if (!base.IsOwner) return;
        lastCallback = callbackContext;
        //Recalculate();
    }

    public void Recalculate()
    {
        var screen = lastCallback.ReadValue<Vector2>();
        output = CameraHolder.ScreenToWorld(screen);
    }

    public Vector3 Get()
    {
        Recalculate();
        return output;
    }
}
