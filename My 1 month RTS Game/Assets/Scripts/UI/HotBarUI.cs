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
    [SerializeField] private Button slot4;
    [SerializeField] private Button slot5;


    private ISelectable selected;


    private void Start()
    {
        mouse.OnSelectedChanged += Mouse_OnSelectedChanged;
    }

    private void Update()
    {
        if (selected == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selected.OnSlot1ButtonClicked();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selected.OnSlot2ButtonClicked();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selected.OnSlot3ButtonClicked();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selected.OnSlot4ButtonClicked();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selected.OnSlot5ButtonClicked();
        }
    }

    private void Mouse_OnSelectedChanged(object sender, Mouse.OnSelectedChangedEventArgs e)
    {
        selected = e.selected;

        try
        {
            slot1.image.sprite = selected.HotBarSlotSOList[0].image;
            slot2.image.sprite = selected.HotBarSlotSOList[1].image;
            slot3.image.sprite = selected.HotBarSlotSOList[2].image;
            slot4.image.sprite = selected.HotBarSlotSOList[3].image;
            slot5.image.sprite = selected.HotBarSlotSOList[4].image;
        }
        catch (System.IndexOutOfRangeException exception)
        {
            Debug.LogError(exception);
        }

        slot1.onClick.AddListener(e.selected.OnSlot1ButtonClicked);
        slot2.onClick.AddListener(e.selected.OnSlot2ButtonClicked);
        slot3.onClick.AddListener(e.selected.OnSlot3ButtonClicked);
        slot4.onClick.AddListener(e.selected.OnSlot4ButtonClicked);
        slot5.onClick.AddListener(e.selected.OnSlot5ButtonClicked);
    }
}
