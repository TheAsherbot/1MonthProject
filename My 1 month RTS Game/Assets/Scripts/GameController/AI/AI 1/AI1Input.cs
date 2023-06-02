using System.Collections;
using System.Collections.Generic;

using TheAshBot;

using UnityEngine;
using UnityEngine.UIElements;

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


    [SerializeField] private GameRTSController controller;
    private AI1_BaseState state;


    private void Start()
    {
        elaspedTime = TimePerAction;
        state = new AI1GatherMinerialsState(controller, TeamManager.AIInstance, TeamManager.PlayerInstance);
    }

    private void Update()
    {
        elaspedTime += Time.deltaTime;

        if (elaspedTime >= TimePerAction)
        {
            elaspedTime = 0;
            state = state.HandleState();
            //this.Log(state.ToString());
        }
    }



}
