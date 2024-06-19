using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Animations;

public class Linear_Controller : MobileInput
{
    [SerializeField] MobileInput_Manager mInputManager;
    [SerializeField] protected Axis localAxis;
    [SerializeField] protected float moveLimit;

    protected Vector2 deltaSwipePos;
    protected bool onPress;

    protected Vector3 posRef, newPos;
    protected bool isSelected;
    
    protected void Awake()
    {
        deltaSwipePos = Vector2.zero;
        posRef = this.transform.position;
        isSelected = false;
    }

    protected void OnEnable()
    {
        mInputManager.Subscribe(this);
    }
    protected void OnDisable()
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
                Move();
                break;
            case TouchPhase.Hold:
                Move();
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
        Selection(position);
    }

    protected void Move()
    {
        if (isSelected && localAxis != Axis.None && moveLimit != 0)
        {
            float movement = -deltaSwipePos.y * 1f;
            switch (localAxis)
            {
                case Axis.X:
                    newPos = movement * Vector3.right;
                    break;
                case Axis.Y:
                    newPos = movement * Vector3.up;
                    break;
                case Axis.Z:
                    newPos = movement * Vector3.forward;
                    break;
                default:
                    break;
            }

            if ((this.transform.position - posRef).magnitude < moveLimit)
            {
                this.transform.Translate(newPos, Space.Self);

                Vector3 diffPos = (this.transform.position - posRef);
                if (diffPos.magnitude >= moveLimit || (diffPos.x + diffPos.y + diffPos.z) < 0)
                {
                    this.transform.Translate(-newPos, Space.Self);
                }
            }
        }
    }
    protected void Selection(Vector2 position)
    {
        Vector2 screenPos = new Vector2(position.x * Display.main.renderingWidth, position.y * Display.main.renderingHeight);
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if(Physics.Raycast(ray, out RaycastHit hitObject))
        {
            if ( hitObject.collider.gameObject.name == this.name )
            {
                isSelected = true;
            }
            else
            {
                //Debug.Log("Object Selected: " + hitObject.collider.gameObject.name + "; wanted: " + this.name);
                isSelected = false;
            }
        }
    }
}
