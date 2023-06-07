using System;

using Sirenix.OdinInspector;
using Sirenix.Serialization;

using TheAshBot;

using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public class _BaseUnit : MonoBehaviour
{


    private enum Startvation
    {
        Full,
        Hungry,
        Starving,
    }


    public event EventHandler OnReachedDestination;
    public event EventHandler OnStopMoveing;
    public event EventHandler<OnMoveEventArgs> OnMove;
    public class OnMoveEventArgs : EventArgs
    {
        public Vector2 movePoint;
    }


    [SerializeField] private SpriteRenderer visual;
    private Startvation startvation = Startvation.Full;


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


    public void Starve()
    {
        switch (startvation)
        {
            case (Startvation.Full):
                startvation = Startvation.Hungry;
                visual.color = Color.yellow;
                break;
            case (Startvation.Hungry):
                startvation = Startvation.Starving;
                visual.color = Color.red;
                break;
            case (Startvation.Starving):
                Destroy(gameObject);
                break;
        }
    }


}
