using BehaviorTree;

public class CheckWarmth : BTNode
{
    private SurvivorAgentUpdaterBT agent;

    public CheckWarmth(SurvivorAgentUpdaterBT agent)
    {
        this.agent = agent;
    }

    public override BTNodeState Evaluate()
    {
        bool isCold = agent.GetIsCold();

        if (isCold)
        {
            state = BTNodeState.SUCCESS;
            return state;
        }
        else
        {
            state = BTNodeState.FAILURE;
            return state;
        }
    }
}
