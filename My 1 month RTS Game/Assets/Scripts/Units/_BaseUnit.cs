using System;

using TheAshBot.TwoDimentional;

using UnityEngine;

public class _BaseUnit : MonoBehaviour
{


    public event EventHandler<OnMoveInputPressedEventArgs> OnMoveInputPressed;
    public class OnMoveInputPressedEventArgs : EventArgs
    {
        public Vector2 mousePosition;
    }


    protected void Trigger_OnMoveInputPressed(Vector2 mousePosition)
    {
        OnMoveInputPressed?.Invoke(this, new OnMoveInputPressedEventArgs
        {
            mousePosition = Mouse2D.GetMousePosition2D(),
        });
    }

}
