using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput_Manager : MobileInput_System
{
    protected Vector2 initPos;

    private List<MobileInput> _inputObservers = new List<MobileInput>();
    private TouchPhase tapPhase;
    private TouchPhase swipePhase;

    protected override void Awake()
    {
        base.Awake();
        tapPhase = global::TouchPhase.NoTouch;
        swipePhase = global::TouchPhase.NoTouch;
        initPos = Vector2.zero;
    }


    // ----------------------------------
    // -------- Observer Manager --------
    // ----------------------------------
    public void Subscribe(MobileInput input)
    {
        _inputObservers.Add(input);
    }
    public void Unsubscribe(MobileInput input)
    {
        _inputObservers.Remove(input);
    }


    // -------------------------------------
    // -------- Overrided functions --------
    // -------------------------------------
    protected override void OnSwipe(Vector2 position)
    {
        if (((position - initPos).magnitude > .05f || tapPhase == TouchPhase.Swipe) && tapPhase != TouchPhase.NoTouch)
        {
            if (tapPhase != TouchPhase.Swipe) swipePhase = tapPhase;
            tapPhase = TouchPhase.Swipe;
            Swipe(initPos, position, swipePhase);
        }
        else swipePhase = TouchPhase.NoTouch;
    }

    protected override void OnPress(Vector2 position)
    {
        initPos = position;
        tapPhase = TouchPhase.Press;
        Press(position);
    }
    protected override void OnPressCancel(Vector2 position)
    {
        if (swipePhase == TouchPhase.Press)
        {
            initPos = Vector2.zero;
            tapPhase = TouchPhase.NoTouch;
            CancelPress(position);
        }
    }
    protected override void OnTap(Vector2 position)
    {
        if (tapPhase != TouchPhase.Multi)
        {
            initPos = position;
            tapPhase = TouchPhase.Tap;
            Tap(position);
        }
    }

    protected override void OnTapHold(Vector2 position)
    {
        if (tapPhase != TouchPhase.Swipe)
        {
            initPos = position;
            tapPhase = TouchPhase.Hold;
            HoldPress(position);
        }
    }
    protected override void OnTapMulti(Vector2 position)
    {
        if (tapPhase != TouchPhase.Multi)
        {
            initPos = position;
            tapPhase = TouchPhase.Multi;
            DoublePress(position);
        }
    }
    protected override void OnTapHoldCancel(Vector2 position)
    {
        if (tapPhase == TouchPhase.Hold || swipePhase == TouchPhase.Hold || tapPhase == TouchPhase.Multi || swipePhase == TouchPhase.Multi)
        {
            initPos = Vector2.zero;
            tapPhase = TouchPhase.NoTouch;
            CancelPress(position);
        }
        
    }


    // ------------------------------------
    // -------- Gestures Functions --------
    // ------------------------------------
    protected virtual void Swipe(Vector2 initPosition, Vector2 endPosition, TouchPhase phase)
    {
        _inputObservers.ForEach(observer =>
        {
            observer.Swipe(initPosition, endPosition, phase);
        });
    }
    protected virtual void Press(Vector2 position)
    {
        _inputObservers.ForEach(observer =>
        {
            observer.Press(position);
        });
    }
    protected virtual void Tap(Vector2 position)
    {
        _inputObservers.ForEach(observer =>
        {
            observer.Tap(position);
        });
    }
    protected virtual void HoldPress(Vector2 position)
    {
        _inputObservers.ForEach(observer =>
        {
            observer.HoldPress(position);
        });
    }
    protected virtual void DoublePress(Vector2 position)
    {
        _inputObservers.ForEach(observer =>
        {
            observer.DoublePress(position);
        });
    }
    protected virtual void CancelPress(Vector2 position)
    {
        _inputObservers.ForEach(observer =>
        {
            observer.CancelPress(position);
        });
    }
}
