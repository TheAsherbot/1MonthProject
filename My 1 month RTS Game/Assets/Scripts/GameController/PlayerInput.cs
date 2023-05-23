using UnityEngine;
using UnityEngine.InputSystem;

using TheAshBot.TwoDimentional;

[RequireComponent(typeof(GameRTSController))]
public class PlayerInput : MonoBehaviour
{


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


    [Header("Visual")]
    [SerializeField] private Transform selectedAreaTransform;

    private Vector2 startPosition;
    private bool isHoldingSelect;


    private void Start()
    {
        GetComponent<GameRTSController>();
        inputActions = new GameInputActions();
        inputActions.Game.Enable();
        inputActions.Game.Select.started += Select_started;
        inputActions.Game.Select.canceled += Select_canceled;
        inputActions.Game.Action1.canceled += Action1_canceled;
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


    #region Events (Subscriptions)

    private void Select_started(InputAction.CallbackContext obj)
    {
        if (!IsGamePlaying) return;

        controller.StartSelecting(Mouse2D.GetMousePosition2D());

        startPosition = Mouse2D.GetMousePosition2D();
        isHoldingSelect = true;
    }

    private void Select_canceled(InputAction.CallbackContext obj)
    {
        if (!IsGamePlaying) return;

        controller.StopSelecting(Mouse2D.GetMousePosition2D());

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
