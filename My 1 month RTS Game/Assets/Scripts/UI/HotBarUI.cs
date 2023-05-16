using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

public class HotBarUI : MonoBehaviour
{


    [SerializeField] private Mouse mouse;
    [Space(6)]
    [SerializeField] private Button slot1;
    [SerializeField] private Button slot2;
    [SerializeField] private Button slot3;


    private ISelectable selected;
    private GameInputActions inputActions;


    private void Start()
    {
        mouse.OnSelectedChanged += Mouse_OnSelectedChanged;
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

    private void Mouse_OnSelectedChanged(object sender, Mouse.OnSelectedChangedEventArgs e)
    {
        selected = e.selected;

        try
        {
            slot1.image.sprite = selected.HotBarSlotSOList[0].image;
            slot2.image.sprite = selected.HotBarSlotSOList[1].image;
            slot3.image.sprite = selected.HotBarSlotSOList[2].image;
        }
        catch (System.IndexOutOfRangeException exception)
        {
            Debug.LogError(exception);
        }

        slot1.onClick.RemoveAllListeners();
        slot2.onClick.RemoveAllListeners();
        slot3.onClick.RemoveAllListeners();

        slot1.onClick.AddListener(e.selected.OnSlot1ButtonClicked);
        slot2.onClick.AddListener(e.selected.OnSlot2ButtonClicked);
        slot3.onClick.AddListener(e.selected.OnSlot3ButtonClicked);
    }
}
