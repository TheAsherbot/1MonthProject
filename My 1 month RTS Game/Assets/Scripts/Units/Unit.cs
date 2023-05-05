using System;

using TheAshBot.TwoDimentional;

using UnityEngine;

public class Unit : _BaseUnit, ISelectable
{




    public bool IsSelected
    {
        get;
        set;
    }



    private void Update()
    {
        if (IsSelected)
        {
            HandelInput();
        }
    }


    private void HandelInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Trigger_OnMoveInputPressed(Mouse2D.GetMousePosition2D());
        }
    }

    public void Select()
    {
        IsSelected = true;
    }

    public void Unselect()
    {
        IsSelected = false;
    }


}
