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

    public void Select();
    public void Unselect();
}
