using System.Collections;
using System.Collections.Generic;

using TheAshBot;

using UnityEngine;

public class AI1Input : MonoBehaviour
{


    [SerializeField] private int actionsPerSecond;
    private float TimePerAction
    {
        get
        {
            return 1f / actionsPerSecond;
        }
    }
    private float elaspedTime;


    [SerializeField] private TownHall startingTownHall;
    [SerializeField] private GameRTSController controller;
    private AI1_BaseState state;


    private void Start()
    {
        elaspedTime = TimePerAction;
        state = new AI1GatherMinerialsState(controller, TeamManager.AIInstance, TeamManager.PlayerInstance);

        StartCoroutine(LateStart());
    }

    private void Update()
    {
        elaspedTime += Time.deltaTime;

        if (elaspedTime >= TimePerAction)
        {
            elaspedTime = 0;
            state = state.HandleState();
            // this.Log(state.ToString());
        }
    }

    private IEnumerator LateStart()
    {
        yield return null;

        List<ISelectable> allUnitsAsIselectablList = new List<ISelectable>();
        foreach (_BaseUnit unit in TeamManager.AIInstance.GetListOfAllUnits())
        {
            allUnitsAsIselectablList.Add((ISelectable)unit);
        }

        controller.Select(allUnitsAsIselectablList);

        GridObject minerialGridObject = AI1GatherMinerialsState.GetClosestMinerialToTownHall(startingTownHall, 10);

        controller.MoveSelected(GridManager.Instance.grid.GetWorldPosition(minerialGridObject.X, minerialGridObject.Y));
    }



}
