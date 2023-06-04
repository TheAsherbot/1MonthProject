using UnityEngine;

public class TownHallVisuals : MonoBehaviour
{


    private TownHall townHall;


    private void Awake()
    {
        townHall = GetComponent<TownHall>();
    }

    private void Start()
    {
        townHall.OnStartSpawningUnit += TownHall_OnStartSpawningUnit;
        townHall.OnFinishedSpawningUnit += TownHall_OnFinishedSpawningUnit;
    }


    private void TownHall_OnStartSpawningUnit(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void TownHall_OnFinishedSpawningUnit(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }


}
