using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class MinerialUI : MonoBehaviour
{
    
    
    [SerializeField] private TeamManager teamManager;
    [SerializeField] private TextMeshProUGUI minerialAmountTxt;


    private void Awake()
    {
        teamManager.OnMinerialValueChanged += TeamManager_OnMinerialValueChanged;
    }


    private void TeamManager_OnMinerialValueChanged(object sender, TeamManager.OnMinerialValueChangedEventArgs e)
    {
        minerialAmountTxt.text = e.currentValue.ToString();
    }


}
