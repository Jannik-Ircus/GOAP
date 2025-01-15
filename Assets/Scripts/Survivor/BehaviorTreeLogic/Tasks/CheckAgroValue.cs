using BehaviorTree;

public class CheckAgroValue : BTNode
{
    private SurvivorEnemyBTUpdater agent;

    public CheckAgroValue(SurvivorEnemyBTUpdater agent)
    {
        this.agent = agent;
    }

    public override BTNodeState Evaluate()
    {
        int agroValue = agent.GetAgroValue();
        if(agent == null)
        {
            state = BTNodeState.FAILURE;
            return state;
        }
        if (agroValue < 5)
        {
            state = BTNodeState.FAILURE;
            return state;
        }
        else
        {
            state = BTNodeState.SUCCESS;
            return state;
        }
    }
}
