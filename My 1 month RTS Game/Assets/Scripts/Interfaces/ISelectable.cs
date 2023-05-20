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
    public List<HotBarSlotSO> HotBarSlotSOList
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
    public bool UsesHotbar()
    {
        return true;
    }

    public void OnSlot1ButtonClicked();
    public void OnSlot2ButtonClicked();
    public void OnSlot3ButtonClicked();
}
