//using FishNet;
//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDirectionProvider : MonoBehaviour
{
    private static InputDirectionProvider instance;
    [SerializeField] private Variable<Transform> cameraTransform;
    private Vector2 output;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }

    public void OnMovement(InputAction.CallbackContext callbackContext)
    {
        //if (!base.IsOwner) return;
        //Debug.Log("PIPI");
        var wasd = callbackContext.ReadValue<Vector2>();
        var cameraRotation = cameraTransform.Value.eulerAngles;


        var worldInputDirection = wasd
            .normalized
            .Rotate(-cameraRotation.y);
        output = (worldInputDirection);
    }

    public static Vector2 Get()
    {
        return instance.output;
    }
}
