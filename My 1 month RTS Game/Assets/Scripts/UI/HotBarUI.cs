using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

public class HotBarUI : MonoBehaviour
{


    [SerializeField] private GameRTSController rtsController;
    [Space(6)]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite HotBarSprite;
    [SerializeField] private Sprite HotBarWithRocksSprite;
    [Space(6)]
    [SerializeField] private UIButton slot1;
    [SerializeField] private UIButton slot2;
    [SerializeField] private UIButton slot3;


    private ISelectable selected;
    private GameInputActions inputActions;


    private void Start()
    {
        rtsController.OnSellect += Mouse_OnSelectedChanged;
        inputActions = new GameInputActions();
        inputActions.GameUI.Enable();
        inputActions.GameUI.HotBar1.performed += HotBar1_performed;
        inputActions.GameUI.HotBar2.performed += HotBar2_performed;
        inputActions.GameUI.HotBar3.performed += HotBar3_performed;
    }

    private void HotBar1_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (selected == null) return;

        selected.OnSlot1ButtonClicked();
    }

    private void HotBar2_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (selected == null) return;

        selected.OnSlot2ButtonClicked();
    }

    private void HotBar3_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (selected == null) return;

        selected.OnSlot3ButtonClicked();
    }

    private void Mouse_OnSelectedChanged(object sender, GameRTSController.OnSellectEventArgs eventArgs)
    {
        if (eventArgs.allSelected.Count > 1 || eventArgs.allSelected.Count == 0)
        {
            // To many to use the hotbar
            SetHotbarVisual(false);
            return;
        }
        else if (!eventArgs.allSelected[0].UsesHotbar())
        {
            // Does not use the hot bar
            SetHotbarVisual(false);
            return;
        }
        else
        {
            // uses the hotbar
            SetHotbarVisual(true);
        }
        
        selected = eventArgs.allSelected[0];

        try
        {
            slot1.SetSprite(selected.HotBarSlotSOList[0].image);
            slot2.SetSprite(selected.HotBarSlotSOList[1].image);
            slot3.SetSprite(selected.HotBarSlotSOList[2].image);
        }
        catch (System.IndexOutOfRangeException exception)
        {
            Debug.LogError(exception);
        }

        slot1.SetAllEventsToNull();
        slot2.SetAllEventsToNull();
        slot3.SetAllEventsToNull();

        slot1.OnMouseEndClickUI += selected.OnSlot1ButtonClicked;
        slot2.OnMouseEndClickUI += selected.OnSlot2ButtonClicked;
        slot3.OnMouseEndClickUI += selected.OnSlot3ButtonClicked;

        slot1.OnMouseEnterUI += () =>
        {
            TooltopScreenSpaceUI.ShowTooltip(selected.HotBarSlotSOList[0].toolTip);
        };
        slot2.OnMouseEnterUI += () => TooltopScreenSpaceUI.ShowTooltip(selected.HotBarSlotSOList[1].toolTip);
        slot3.OnMouseEnterUI += () => TooltopScreenSpaceUI.ShowTooltip(selected.HotBarSlotSOList[2].toolTip);

        slot1.OnMouseExitUI += () => TooltopScreenSpaceUI.HideTooltip();
        slot2.OnMouseExitUI += () => TooltopScreenSpaceUI.HideTooltip();
        slot3.OnMouseExitUI += () => TooltopScreenSpaceUI.HideTooltip();

    }

    private void Slot1_OnMouseEndClickUI()
    {
        throw new System.NotImplementedException();
    }

    private void SetHotbarVisual(bool usesHotBar)
    {
        if (usesHotBar)
        {
            backgroundImage.sprite = HotBarSprite;
            slot1.gameObject.SetActive(true);
            slot2.gameObject.SetActive(true);
            slot3.gameObject.SetActive(true);
        }
        else
        {
            backgroundImage.sprite = HotBarWithRocksSprite;
            slot1.gameObject.SetActive(false);
            slot2.gameObject.SetActive(false);
            slot3.gameObject.SetActive(false);
        }
    }
}
