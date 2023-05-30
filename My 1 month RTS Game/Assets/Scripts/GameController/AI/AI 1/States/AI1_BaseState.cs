public abstract class AI1_BaseState
{


    protected GameRTSController controller;

    protected TeamManager teamManager;
    protected TeamManager enemyTeamManager;



    public AI1_BaseState(GameRTSController controller, TeamManager teamManager, TeamManager enemyTeamManager)
    {
        this.controller = controller;
        this.teamManager = teamManager;
        this.enemyTeamManager = enemyTeamManager;
    }


    public abstract AI1_BaseState HandleState();
    

}
