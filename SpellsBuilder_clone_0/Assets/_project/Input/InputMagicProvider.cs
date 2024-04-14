using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class InputMagicProvider : MonoBehaviour
{
    //[SerializeField] private MagicInputHandler handler;
    public Action<CastTarget> CastStarted;
    public Action CastStopped;
    public Action<ElementType> ElementPressed;

    public void OnFirstSpell(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            HandleSpeMagicInput(ElementType.Roots);
        }
    }

    public void OnSecondSpell(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            HandleSpeMagicInput(ElementType.Leaves);
        }
    }

    public void OnThirdSpell(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            HandleSpeMagicInput(ElementType.Sun);
        }
    }

    public void OnFourthSpell(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            HandleSpeMagicInput(ElementType.Moon);
        }
    }

    public void OnFifthSpell(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            HandleSpeMagicInput(ElementType.Void);
        }
    }

    public void OnSixthSpell(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            HandleSpeMagicInput(ElementType.Stars);
        }
    }

    public void OnCastNormal(InputAction.CallbackContext context)
    {

        if (context.started && context.interaction is HoldInteraction)
        {
            Debug.Log("hold started");
            HandleCastStart(CastTarget.Normal);
        }
        if (context.performed && context.interaction is HoldInteraction)
        {
            Debug.Log("hold performed");
        }
        if (context.canceled && context.interaction is HoldInteraction)
        {
            Debug.Log("hold canceled");
            HandleCastStop();
        }

        //if (context.started && context.interaction is TapInteraction)
        //{
        //    Debug.Log("tap started");
        //    HandleCastStart(CastTarget.Normal);
        //}
        //if (context.performed && context.interaction is TapInteraction)
        //{
        //    Debug.Log("tap performed");
        //    HandleCastStop();
        //}
        //if (context.canceled && context.interaction is TapInteraction)
        //{
        //    Debug.Log("tap canceled");
        //    HandleCastStop();
        //}
    }

    private void HandleSpeMagicInput(ElementType type)
    {
        ElementPressed?.Invoke(type);
    }

    private void HandleCastStart(CastTarget target)
    {
        CastStarted?.Invoke(target);
    }

    private void HandleCastStop()
    {
        CastStopped?.Invoke();
    }
}

public enum CastTarget
{
    Normal,
    Self,
    Aoe,
    Weapon
}


