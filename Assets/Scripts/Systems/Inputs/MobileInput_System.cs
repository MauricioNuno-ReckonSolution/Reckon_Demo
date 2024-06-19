using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public abstract class MobileInput_System : MonoBehaviour
{
    protected static InputAction swipe_Action;
    protected static InputAction press_Action;
    protected static InputAction tapHold_Action;

    protected static InputAction swipeT1_Action;
    protected static InputAction pressT1_Action;
    protected static InputAction tapHoldT1_Action;

    protected float tapTime;

    protected virtual void Awake()
    {
        // -----------------------------------------------
        // -------- Link Input Actions to devices --------
        // -----------------------------------------------
        swipe_Action = new InputAction("SwipePointerAction", binding: "<Touchscreen>/touch0/position");
        press_Action = new InputAction("PressPointerAction", binding: "<Touchscreen>/touch0/press", interactions: "Press,Tap");
        tapHold_Action = new InputAction("HoldPointerAction", binding: "<Touchscreen>/touch0/press", interactions: "Hold");

        swipeT1_Action = new InputAction("SwipeT1Action", binding: "<Touchscreen>/touch1/position");
        pressT1_Action = new InputAction("PressT1Action", binding: "<Touchscreen>/touch1/press", interactions: "Press,Tap");
        tapHoldT1_Action = new InputAction("HoldT1Action", binding: "<Touchscreen>/touch1/press", interactions: "Hold");


        // ------------------------------
        // -------- Lambda Calls --------
        // ------------------------------
        swipe_Action.performed += ctx =>
        {
            if (ctx.control.device is Pointer device)
            {
                OnSwipe(NormalizeResolution(device.position.ReadValue()));
            }
        };
        press_Action.performed += ctx =>
        {
            Debug.Log("Inter Type: \'" + ctx.interaction.GetType().Name + "\'");
            if (ctx.control.device is Pointer device)
            {
                switch (ctx.interaction.GetType().Name)
                {
                    case "PressInteraction":
                        if (Time.timeSinceLevelLoad - tapTime > InputSystem.settings.multiTapDelayTime)
                        {
                            OnPress(NormalizeResolution(device.position.ReadValue()));
                            OnPress(NormalizeResolution(device.position.ReadValue()), device.name);
                        }
                        else
                        {
                            OnTapMulti(NormalizeResolution(device.position.ReadValue()));
                        }
                        break;

                    case "TapInteraction":
                        tapTime = Time.timeSinceLevelLoad;
                        OnTap(NormalizeResolution(device.position.ReadValue()));
                        break;

                    default:
                        break;
                }
            }
        };
        press_Action.canceled += ctx =>
        {
            if (ctx.control.device is Pointer device)
            {
                switch (ctx.interaction.GetType().Name)
                {
                    case "PressInteraction":
                        OnPressCancel(NormalizeResolution(device.position.ReadValue()));
                        break;

                    case "TapInteraction":
                        OnTapCancel(NormalizeResolution(device.position.ReadValue()));
                        break;

                    default:
                        break;
                }
            }
        };
        tapHold_Action.performed += ctx =>
        {
            Debug.Log("InterHold Type: \'" + ctx.interaction.GetType().Name + "\'");
            if (ctx.control.device is Pointer device)
            {
                if (Time.timeSinceLevelLoad - tapTime > InputSystem.settings.multiTapDelayTime)
                {
                    OnTapHold(NormalizeResolution(device.position.ReadValue()));
                }
            }
        };
        tapHold_Action.canceled += ctx =>
        {
            if (ctx.control.device is Pointer device)
            {
                OnTapHoldCancel(NormalizeResolution(device.position.ReadValue()));
            }
        };

        swipeT1_Action.performed += ctx =>
        {
            if (ctx.control.device is Pointer device)
            {
                OnSwipeT1(NormalizeResolution(device.position.ReadValue()));
            }
        };
        pressT1_Action.performed += ctx =>
        {
            Debug.Log("InterT1 Type: \'" + ctx.interaction.GetType().Name + "\'");
            if (ctx.control.device is Pointer device)
            {
                switch (ctx.interaction.GetType().Name)
                {
                    case "PressInteraction":
                        if (Time.timeSinceLevelLoad - tapTime > InputSystem.settings.multiTapDelayTime)
                        {
                            OnPressT1(NormalizeResolution(device.position.ReadValue()));
                            OnPressT1(NormalizeResolution(device.position.ReadValue()), device.name);
                        }
                        else
                        {
                            OnTapMultiT1(NormalizeResolution(device.position.ReadValue()));
                        }
                        break;

                    case "TapInteraction":
                        tapTime = Time.timeSinceLevelLoad;
                        OnTapT1(NormalizeResolution(device.position.ReadValue()));
                        break;

                    default:
                        break;
                }
            }
        };
        pressT1_Action.canceled += ctx =>
        {
            if (ctx.control.device is Pointer device)
            {
                switch (ctx.interaction.GetType().Name)
                {
                    case "PressInteraction":
                        OnPressCancelT1(NormalizeResolution(device.position.ReadValue()));
                        break;

                    case "TapInteraction":
                        OnTapCancelT1(NormalizeResolution(device.position.ReadValue()));
                        break;

                    default:
                        break;
                }
            }
        };
        tapHoldT1_Action.performed += ctx =>
        {
            Debug.Log("InterHoldT1 Type: \'" + ctx.interaction.GetType().Name + "\'");
            if (ctx.control.device is Pointer device)
            {
                if (Time.timeSinceLevelLoad - tapTime > InputSystem.settings.multiTapDelayTime)
                {
                    OnTapHoldT1(NormalizeResolution(device.position.ReadValue()));
                }
            }
        };
        tapHoldT1_Action.canceled += ctx =>
        {
            if (ctx.control.device is Pointer device)
            {
                OnTapHoldCancelT1(NormalizeResolution(device.position.ReadValue()));
            }
        };
    }

    protected Vector2 NormalizeResolution(Vector2 position)
    {
        return new Vector2(position.x / Display.main.renderingWidth, position.y / Display.main.renderingHeight);
    }


    // -------------------------------------------------
    // -------- Enable/Disable of Input Actions --------
    // -------------------------------------------------
    protected virtual void OnEnable()
    {
        swipe_Action.Enable(); press_Action.Enable(); tapHold_Action.Enable(); swipeT1_Action.Enable(); pressT1_Action.Enable(); tapHoldT1_Action.Enable();
    }
    protected virtual void OnDisable()
    {
        swipe_Action.Disable(); press_Action.Disable(); tapHold_Action.Disable(); swipeT1_Action.Disable(); pressT1_Action.Disable(); tapHoldT1_Action.Disable();
    }
    protected virtual void OnDestroy()
    {
        swipe_Action.Dispose(); press_Action.Dispose(); tapHold_Action.Dispose(); swipeT1_Action.Dispose(); pressT1_Action.Dispose(); tapHoldT1_Action.Dispose();
    }


    // ----------------------------------------
    // -------- Default Function Calls --------
    // ----------------------------------------
    protected virtual void OnSwipe(Vector2 position) { }
    protected virtual void OnPress(Vector2 position) { }
    protected virtual void OnPress(Vector2 position, string deviceName) { }
    protected virtual void OnPressCancel(Vector2 position) { }
    protected virtual void OnTap(Vector2 position) { }
    protected virtual void OnTapCancel(Vector2 position) { }
    protected virtual void OnTapHold(Vector2 position) { }
    protected virtual void OnTapHoldCancel(Vector2 position) { }
    protected virtual void OnTapMulti(Vector2 position) { }

    protected virtual void OnSwipeT1(Vector2 position) { }
    protected virtual void OnPressT1(Vector2 position) { }
    protected virtual void OnPressT1(Vector2 position, string deviceName) { }
    protected virtual void OnPressCancelT1(Vector2 position) { }
    protected virtual void OnTapT1(Vector2 position) { }
    protected virtual void OnTapCancelT1(Vector2 position) { }
    protected virtual void OnTapHoldT1(Vector2 position) { }
    protected virtual void OnTapHoldCancelT1(Vector2 position) { }
    protected virtual void OnTapMultiT1(Vector2 position) { }
}
