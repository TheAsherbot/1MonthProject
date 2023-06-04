using UnityEngine;
using UnityEngine.InputSystem;

using TheAshBot.TwoDimentional;
using System.Collections.Generic;
using System.Linq;
using TheAshBot;

[RequireComponent(typeof(GameRTSController))]
public class PlayerInput : MonoBehaviour
{


    #region Variables

    [Header("Logic")]
    private GameRTSController controller;
    private GameInputActions inputActions;

    private bool IsGamePlaying
    {
        get
        {
            return Time.deltaTime != 0;
        }
    }


    [Header("Collition")]
    [SerializeField] private LayerMask unitLayerMask;


    [Header("Visual")]
    [SerializeField] private Transform selectedAreaTransform;

    private Vector2 startPosition;
    private bool isHoldingSelect;

    #endregion


    #region Unity Functions

    private void Awake()
    {
        selectedAreaTransform.gameObject.SetActive(false);
        controller = GetComponent<GameRTSController>();
        inputActions = new GameInputActions();
    }

    private void OnEnable()
    {
        inputActions.Game.Enable();
        inputActions.Game.Select.started += Select_started;
        inputActions.Game.Select.canceled += Select_canceled;
        inputActions.Game.Action1.canceled += Action1_canceled;
    }

    private void OnDisable()
    {
        inputActions.Game.Disable();
        inputActions.Game.Select.started -= Select_started;
        inputActions.Game.Select.canceled -= Select_canceled;
        inputActions.Game.Action1.canceled -= Action1_canceled;
    }

    private void Update()
    {
        if (!IsGamePlaying) return;

        if (isHoldingSelect)
        {
            Vector3 currentMaousePosition = Mouse2D.GetMousePosition2D();
            Vector3 lowerLeft = new Vector3(Mathf.Min(startPosition.x, currentMaousePosition.x), Mathf.Min(startPosition.y, currentMaousePosition.y));
            Vector3 UpperRight = new Vector3(Mathf.Max(startPosition.x, currentMaousePosition.x), Mathf.Max(startPosition.y, currentMaousePosition.y));
            selectedAreaTransform.position = lowerLeft;
            selectedAreaTransform.localScale = UpperRight - lowerLeft;
        }
    }

    #endregion


    #region Events (Subscriptions)

    private void Select_started(InputAction.CallbackContext obj)
    {
        if (Mouse2D.IsMouseOverUIWithIgnores<UI>()) return;

        if (!IsGamePlaying) return;

        selectedAreaTransform.gameObject.SetActive(true);

        startPosition = Mouse2D.GetMousePosition2D();
        isHoldingSelect = true;
    }

    private void Select_canceled(InputAction.CallbackContext obj)
    {
        if (!IsGamePlaying) return;

        selectedAreaTransform.gameObject.SetActive(false);

        Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, Mouse2D.GetMousePosition2D(), unitLayerMask);

        List <ISelectable> selectableList = new List<ISelectable>();
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.TryGetComponent(out ISelectable selectable)) selectableList.Add(selectable);
        }

        controller.Select(selectableList);

        startPosition = Vector2.zero;
        isHoldingSelect = false;
    }

    private void Action1_canceled(InputAction.CallbackContext obj)
    {
        if (!IsGamePlaying) return;

        controller.MoveSelected(Mouse2D.GetMousePosition2D());
    }

    #endregion


}
