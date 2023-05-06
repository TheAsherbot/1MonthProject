using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public bool IsSelected
    {
        get;
        set;
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
