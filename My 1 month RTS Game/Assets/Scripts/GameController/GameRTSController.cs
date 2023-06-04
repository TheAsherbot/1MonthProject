using System;
using System.Collections.Generic;

using TheAshBot;
using TheAshBot.TwoDimentional;

using UnityEditor.Rendering.Universal;

using UnityEngine;

public class GameRTSController : MonoBehaviour
{


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
    private List<ISelectable> selectedList;

    #endregion


    #region Unity Functions

    private void Awake()
    {
        selectedList = new List<ISelectable>();
    }

    #endregion


    #region Input (Public)

    public void Select(List<ISelectable> selectedList)
    {
        RemoveAllFromSelectedList();
        List<ISelectable> newISelecableList = new List<ISelectable>();
        bool hasUnits = false;
        foreach (ISelectable selectable in selectedList)
        {
            Component selectableAsComponent = selectable as Component;

            if (team == Teams.PlayerTeam)
            {
                if (selectableAsComponent.TryGetComponent(out IsOnPlayerTeam isOnPlayerTeam))
                {
                    newISelecableList.Add(selectable);
                    if (selectableAsComponent.TryGetComponent(out _BaseUnit unit)) hasUnits = true;
                }
            }
            else if (team == Teams.AITeam)
            {
                newISelecableList.Add(selectable);
                if (selectableAsComponent.TryGetComponent(out _BaseUnit unit)) hasUnits = true;
            }
        }

        this.selectedList = newISelecableList;

        // Unselecing buildings if units are selected
        if (hasUnits)
        {
            List<ISelectable> removeList = new List<ISelectable>();

            foreach (ISelectable selectable in this.selectedList)
            {
                if (selectable is _BaseBuilding)
                {
                    removeList.Add(selectable);
                    continue;
                }
                selectable.Select();
            }

            foreach (ISelectable selected in removeList)
            {
                this.selectedList.Remove(selected);
            }
        }

        OnSellect?.Invoke(this, new OnSellectEventArgs
        {
            allSelected = this.selectedList,
            newSelected = newISelecableList,
        });
    }

    public void MoveSelected(Vector2 movePosition)
    {
        Vector2 moveToPosition = movePosition;
        List<Vector2> targetPositionList = GetPositionListAround(moveToPosition, new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new int[] { 5, 10, 20, 40, 80, 160, 320, 640, 1280, 2560 });

        bool moveAllToPoint = false;
        if (Mouse2D.TryGetObjectAtMousePosition(out GameObject hit))
        {
            if (hit.TryGetComponent(out _BaseIsOnTeam baseIsOnTeam))
            {
                if (team == Teams.PlayerTeam)
                {
                    if (baseIsOnTeam is IsOnAITeam)
                    {
                        moveAllToPoint = true;
                    }
                }
                else if (team == Teams.AITeam)
                {
                    if (baseIsOnTeam is IsOnPlayerTeam)
                    {
                        moveAllToPoint = true;
                    }
                }
            }
        }
        else if (GridManager.Instance.grid.GetGridObject(movePosition).tilemapSprite == GridObject.TilemapSprite.Minerials)
        {
            moveAllToPoint = true;
        }

        int targetPositionListIndex = 0;
        foreach (ISelectable selectable in selectedList)
        {
            if (selectable is IMoveable moveable)
            {
                if (moveAllToPoint)
                {
                    moveable.Move(movePosition);
                }
                else
                {
                    moveable.Move(targetPositionList[targetPositionListIndex]);
                    targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                }
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
