using System;

using TheAshBot.TwoDimentional;

using UnityEngine;

public class _BaseUnit : MonoBehaviour
{


    public event EventHandler OnReachedDestination;
    public event EventHandler OnStopMoveing;
    public event EventHandler<OnMoveEventArgs> OnMove;
    public class OnMoveEventArgs : EventArgs
    {
        public Vector2 movePoint;
    }


    protected void Trigger_OnMove(Vector2 movePoint)
    {
        OnMove?.Invoke(this, new OnMoveEventArgs
        {
            movePoint = movePoint,
        });
    }
    
    protected void Trigger_OnStopMoveing()
    {
        OnStopMoveing?.Invoke(this, EventArgs.Empty);
    }
    
    public void Trigger_OnReachedDestination()
    {
        OnReachedDestination?.Invoke(this, EventArgs.Empty);
    }

}
