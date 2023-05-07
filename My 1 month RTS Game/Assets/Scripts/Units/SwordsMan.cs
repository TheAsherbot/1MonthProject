using System.Collections.Generic;

using TheAshBot.TwoDimentional;

using UnityEngine;

public class SwordsMan : _BaseUnit, ISelectable
{


    #region Variables

    public bool IsSelected
    {
        get;
        set;
    }
    [field: SerializeField]
    public List<HotBarSlotSO> HotBarSlotSOList
    {
        get;
        set;
    }

    #endregion


    #region Unity Functions

    private void Update()
    {
        
    }

    #endregion


    #region Input

    private void TestInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MoveInputPressed();
        }
    }

    private void MoveInputPressed()
    {
        Trigger_OnMove(Mouse2D.GetMousePosition2D());
    }

    #endregion


    #region Interfaces

    public void OnSlot1ButtonClicked()
    {
        // Attack
    }

    public void OnSlot2ButtonClicked()
    {
        // Potroll
    }

    public void OnSlot3ButtonClicked()
    {
        // Only Attack
    }

    public void OnSlot4ButtonClicked()
    {
        // Upgrade 1
    }

    public void OnSlot5ButtonClicked()
    {
        // Upgrade 2
    }

    #endregion

    
}
