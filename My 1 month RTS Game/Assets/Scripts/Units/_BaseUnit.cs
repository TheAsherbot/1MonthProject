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

    protected void Start()
    {
        if (TryGetComponent(out IsOnPlayerTeam isOnPlayerTeam))
        {
            //UnitSelections.Instance.AddToUnitList(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (TryGetComponent(out IsOnPlayerTeam isOnPlayerTeam))
        {
            //UnitSelections.Instance.RemoveFromUnitList(gameObject);
        }
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
