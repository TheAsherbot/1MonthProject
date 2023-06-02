using System;

using TheAshBot;

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
        this.Log("Trigger_OnMove at " + movePoint);
        OnMove?.Invoke(this, new OnMoveEventArgs
        {
            movePoint = movePoint,
        });
        if (OnMove == null)
        {
            this.Log("OnMove == null");
        }
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
