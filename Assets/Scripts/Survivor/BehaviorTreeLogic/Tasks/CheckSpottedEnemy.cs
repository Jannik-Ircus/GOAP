using BehaviorTree;

public class CheckSpottedEnemy : BTNode
{
    private SurvivorAgentUpdaterBT agent;

    public CheckSpottedEnemy(SurvivorAgentUpdaterBT agent)
    {
        this.agent = agent;
    }

    public override BTNodeState Evaluate()
    {
        bool spottedEnemy = agent.GetEnemySpotted();

        if (spottedEnemy)
        {
            state = BTNodeState.SUCCESS;
            return state;
        } else
        {
            state = BTNodeState.FAILURE;
            return state;
        }
    }
}
