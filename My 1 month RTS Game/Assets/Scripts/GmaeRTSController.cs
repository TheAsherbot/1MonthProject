using System;
using System.Collections.Generic;

using TheAshBot.TwoDimentional;

using UnityEditor.SearchService;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GmaeRTSController : MonoBehaviour
{

    public enum Teams
    {
        Player,
        AI,
    }

    #region Events

    public event EventHandler<OnSellectEventArgs> OnSellect;
    public class OnSellectEventArgs : EventArgs
    {
        public List<ISelectable> allSelected;
        public List<ISelectable> newSelected;
    }

    #endregion


    #region Variables

    [Header("Team")]
    [SerializeField] private Teams team;


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

    #endregion


    #region Unity Functions

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

    #endregion


    #region Subscription

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
        List<ISelectable> listOfNewSellectedItems = new List<ISelectable>();
        bool hasUnits = false;
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (team == Teams.Player)
            {
                if (collider2D.TryGetComponent(out ISelectable selectable) && collider2D.TryGetComponent(out IsOnPlayerTeam isOnPlayerTeam))
                {
                    listOfNewSellectedItems.Add(selectable);
                    selectedList.Add(selectable);
                    if (selectable is _BaseUnit unit) hasUnits = true;
                }
            }
            else if (team == Teams.AI)
            {
                if (collider2D.TryGetComponent(out ISelectable selectable) && collider2D.TryGetComponent(out IsOnAITeam isOnAITeam))
                {
                    listOfNewSellectedItems.Add(selectable);
                    selectedList.Add(selectable);
                    if (selectable is _BaseBuilding building) hasUnits = false;
                }
            }
        }

        List<ISelectable> removeList = new List<ISelectable>();
        foreach (ISelectable selectable in selectedList)
        {
            if (hasUnits)
            {
                if (selectable is _BaseBuilding building)
                {
                    removeList.Add(selectable);
                    continue;
                }
                selectable.Select();
            }
        }
        foreach (ISelectable selected in removeList)
        {
            selectedList.Remove(selected);
        }

        OnSellect?.Invoke(this, new OnSellectEventArgs
        {
            allSelected = selectedList,
            newSelected = listOfNewSellectedItems,
        });
    }

    private void Action1_started(InputAction.CallbackContext callbackContext)
    {
        Vector2 moveToPosition = Mouse2D.GetMousePosition2D();
        List<Vector2> targetPositionList = GetPositionListAround(moveToPosition, new float[] { 1, 2, 3, 4, 5 }, new int[] { 5, 10, 20, 40, 80 });

        int targetPositionListIndex = 0;
        foreach (ISelectable selectable in selectedList)
        {
            if (selectable is IMoveable moveable)
            {
                moveable.Move(targetPositionList[targetPositionListIndex]);
                targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
            }
        }
    }

    #endregion


    #region Selection

    private void RemoveAllFromSelectedList()
    {
        foreach (ISelectable selectable in selectedList)
        {
            selectable.Unselect();
        }
        selectedList.Clear();
    }

    #endregion


    #region Move Units

    private List<Vector2> GetPositionListAround(Vector2 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector2> positionList = new List<Vector2>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    private List<Vector2> GetPositionListAround(Vector2 startPosition, float distance, int positionCount)
    {
        List<Vector2> positionList = new List<Vector2>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360 / positionCount);
            Vector2 direction = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector2 position = startPosition + direction * distance;
            positionList.Add(position);
        }
        return positionList;
    }

    private Vector2 ApplyRotationToVector(Vector3 vector, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vector;
    }

    #endregion


}
