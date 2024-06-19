using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PivotDoor_Controller : MobileInput
{
    [SerializeField] MobileInput_Manager mInputManager;

    protected Vector2 deltaSwipePos;
    protected bool onPress;

    protected Vector3 posRef;
    protected bool isSelected;
    
    void Awake()
    {
        deltaSwipePos = Vector2.zero;
        posRef = this.transform.position;
        isSelected = false;
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
        if (isSelected)
        {
            Vector3 newPos = new Vector3(-deltaSwipePos.y, 0) * 1f;
            if (this.transform.position.x >= posRef.x && this.transform.position.x <= 0.3)
            {
                this.transform.Translate(newPos, Space.Self);
            }
            else if (this.transform.position.x < posRef.x)
            {
                this.transform.Translate(new Vector3(0.0001f, 0), Space.Self);
            }
            else if (this.transform.position.x > 0.3)
            {
                this.transform.Translate(new Vector3(-0.0001f, 0), Space.Self);
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
                Debug.Log("Object Selected: " + hitObject.collider.gameObject.name + "; wanted: " + this.name);
                isSelected = false;
            }
        }
    }
}
