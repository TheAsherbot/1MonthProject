using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class MinerialUI : MonoBehaviour
{
    
    
    [SerializeField] private TextMeshProUGUI minerialAmountTxt;


    private void Awake()
    {
        TeamManager.PlayerInstance.OnMinerialValueChanged += TeamManager_OnMinerialValueChanged;
    }


    private void TeamManager_OnMinerialValueChanged(object sender, TeamManager.OnMinerialValueChangedEventArgs e)
    {
        minerialAmountTxt.text = e.currentValue.ToString();
    }


}
