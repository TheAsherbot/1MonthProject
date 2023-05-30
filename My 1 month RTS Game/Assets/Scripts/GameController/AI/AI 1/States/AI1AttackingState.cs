public class AI1AttackingState : AI1_BaseState
{


    public AI1AttackingState(GameRTSController controller, TeamManager teamManager, TeamManager enemyTeamManager) : base(controller, teamManager, enemyTeamManager)
    {
        this.controller = controller;
        this.teamManager = teamManager;
        this.enemyTeamManager = enemyTeamManager;
    }


    public override AI1_BaseState HandleState()
    {
        return this;
    }


}
