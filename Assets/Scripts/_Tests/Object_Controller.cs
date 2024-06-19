using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Object_Controller : MobileInput
{
    [SerializeField] MobileInput_Manager mInputManager;

    protected Vector2 deltaSwipePos;
    protected bool onPress;
    
    void Awake()
    {
        deltaSwipePos = Vector2.zero;
    }

    protected virtual void OnEnable()
    {
        mInputManager.Subscribe(this);
    }
    protected virtual void OnDisable()
    {
        mInputManager.Unsubscribe(this);
    }

    public override void Swipe(Vector2 initpos, Vector2 endPos, TouchPhase phase)
    {
        if (onPress)
        {
            deltaSwipePos = endPos - initpos;
            onPress = false;
        }
        else deltaSwipePos = endPos - deltaSwipePos;

        switch (phase)
        {
            case TouchPhase.Press:
                Vector3 newPos = new Vector3(deltaSwipePos.x, 0, deltaSwipePos.y) * 10f;
                this.transform.Translate(newPos, Space.World);
                break;
            case TouchPhase.Hold:
                this.transform.localScale = this.transform.localScale + Vector3.one * deltaSwipePos.y * 5f;
                break;
            case TouchPhase.Multi:
                this.transform.Rotate(new Vector3(deltaSwipePos.y, -deltaSwipePos.x) * 50f);
                break;
            default:
                break;
        }

        deltaSwipePos = endPos;
    }

    public override void Press(Vector2 position)
    {
        onPress = true;
    }
    public override void Tap(Vector2 position)
    {
        Debug.Log("Tap: " + position);
        deltaSwipePos = position;
    }
    public override void HoldPress(Vector2 position)
    {
        Debug.Log("Hold: " + position);
        deltaSwipePos = position;
    }
    public override void DoublePress(Vector2 position)
    {
        Debug.Log("Multi: " + position);
        deltaSwipePos = position;
    }
    public override void CancelPress(Vector2 position)
    {
        Debug.Log("Cancel Press: " + position);
    }
}
