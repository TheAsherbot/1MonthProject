using System.Collections;
using System.Collections.Generic;

using TheAshBot;
using TheAshBot.TwoDimentional;

using UnityEngine;
using UnityEngine.InputSystem;

public class GmaeRTSController : MonoBehaviour
{


    [Header("Selecting")]
    private Vector2 startPosition;
    private List<ISelectable> selectedList;


    [Header("Visual")]
    [SerializeField] private Transform selectedAreaTransform;


    [Header("Collition")]
    [SerializeField] private LayerMask unitLayerMask;


    [Header("Input")]
    private bool isHoldingSelect;
    private GameInputActions inputActions;


    private void Awake()
    {
        selectedList = new List<ISelectable>();
        selectedAreaTransform.gameObject.SetActive(false);
    }

    private void Start()
    {
        inputActions = new GameInputActions();
        inputActions.Game.Enable();
        inputActions.Game.Select.started += Select_started;
        inputActions.Game.Select.canceled += Select_canceled;
        inputActions.Game.Action1.started += Action1_started;
    }

    private void Update()
    {
        if (isHoldingSelect)
        {
            Vector3 currentMaousePosition = Mouse2D.GetMousePosition2D();
            Vector3 lowerLeft = new Vector3(Mathf.Min(startPosition.x, currentMaousePosition.x), Mathf.Min(startPosition.y, currentMaousePosition.y));
            Vector3 UpperRight = new Vector3(Mathf.Max(startPosition.x, currentMaousePosition.x), Mathf.Max(startPosition.y, currentMaousePosition.y));
            selectedAreaTransform.position = lowerLeft;
            selectedAreaTransform.localScale = UpperRight - lowerLeft;
        }
    }


    private void Select_started(InputAction.CallbackContext obj)
    {
        isHoldingSelect = true;
        selectedAreaTransform.gameObject.SetActive(true);

        startPosition = Mouse2D.GetMousePosition2D();

    }

    private void Select_canceled(InputAction.CallbackContext obj)
    {
        isHoldingSelect = false;
        selectedAreaTransform.gameObject.SetActive(false);

        Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, Mouse2D.GetMousePosition2D(), unitLayerMask);

        RemoveAllFromSelectedList();
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.TryGetComponent(out ISelectable selectable))
            {
                selectedList.Add(selectable);
                selectable.Select();
            }
        }

        this.Log(selectedList.Count);
    }

    private void RemoveAllFromSelectedList()
    {
        foreach (ISelectable selectable in selectedList)
        {
            selectable.Unselect();
        }
        selectedList.Clear();
    }

    private void Action1_started(InputAction.CallbackContext obj)
    {
        
    }

}
