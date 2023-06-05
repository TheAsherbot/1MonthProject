using TheAshBot;
using TheAshBot.ProgressBarSystem;

using UnityEngine;

public class TownHallSpawningProgressVisuals : MonoBehaviour
{


    [SerializeField] private Color barColor = Color.white;


    private int maxProgress = 100;
    private float spawmUnitTimer;
    private float updateProgressTimer;
    private float updateProgressTimerMax = 0.25f;
    private ProgressSystem progressSystem;


    private bool isSpawingUnit;
    private UnitSO unitBeingSpawned;
    private TownHall townHall;



    #region Unit Functions

    private void Awake()
    {
        townHall = GetComponent<TownHall>();
    }

    private void Start()
    {
        townHall.OnStartSpawningUnit += TownHall_OnStartSpawningUnit;
        townHall.OnFinishedSpawningUnit += TownHall_OnFinishedSpawningUnit;
    }

    private void Update()
    {
        if (!isSpawingUnit) return;


        spawmUnitTimer += Time.deltaTime;
        updateProgressTimer += Time.deltaTime;

        if (updateProgressTimer > updateProgressTimerMax)
        {
            updateProgressTimer = 0;
            progressSystem.SetProgress(Mathf.RoundToInt(spawmUnitTimer / unitBeingSpawned.timeToSpawn * maxProgress));
        }
    }

    #endregion


    #region Events (Subscriptions)

    private void TownHall_OnStartSpawningUnit(object sender, TownHall.OnStartSpawningUnitEventArgs e)
    {
        unitBeingSpawned = e.unitSO;

        spawmUnitTimer = 0;
        updateProgressTimer = 0;

        isSpawingUnit = true;
        progressSystem = MakeProgeressBar();
        progressSystem.SetProgress(0);
    }

    private void TownHall_OnFinishedSpawningUnit(object sender, System.EventArgs e)
    {
        isSpawingUnit = false;
        progressSystem.SetProgressToMaxProgress();
    }

    #endregion


    private ProgressSystem MakeProgeressBar()
    {
        Vector3 offset = new Vector3(1.6f, 3f);
        Vector3 size = new Vector3(3, 0.3f);
        return ProgressBar.Create(maxProgress, transform, offset, size, barColor, Color.gray, new ProgressBar.Border { color = Color.black, thickness = 0.075f }, true, false, 13);
    }




}
