using UnityEngine;

public abstract class MobileInput : MonoBehaviour
{
    public virtual void Swipe(Vector2 startPosition, Vector2 endPosition, TouchPhase phase) { }
    public virtual void Press(Vector2 position) { }
    public virtual void Tap(Vector2 position) { }
    public virtual void HoldPress(Vector2 position) { }
    public virtual void HoldTap(Vector2 position) { }
    public virtual void DoublePress(Vector2 position) { }
    public virtual void DoubleTap(Vector2 position) { }
    public virtual void CancelPress(Vector2 position) { }

    public virtual void Pitch(Vector2 startDelta, Vector2 endDelta, TouchPhase phase) { }
    public virtual void Displace(Vector2 startDelta, Vector2 endDelta, TouchPhase phase) { }
    public virtual void Rotate(Vector2 startDelta, Vector2 endDelta, TouchPhase phase) { }
    public virtual void CancelComplex() { }
}

public enum TouchPhase { NoTouch, Press, Tap, Hold, Multi, Swipe }