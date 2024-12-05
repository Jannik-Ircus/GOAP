
using BehaviorTree;

public class CheckWoodStorage : BTNode
{
    private SurvivorAgentUpdaterBT agent;

    public CheckWoodStorage(SurvivorAgentUpdaterBT agent)
    {
        this.agent = agent;
    }

    public override BTNodeState Evaluate()
    {
        int woodStorage = agent.GetWoodStorageAmount();

        if (woodStorage >= 1)
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
