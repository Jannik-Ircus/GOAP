
using BehaviorTree;

public class CheckWoodStorageFull : BTNode
{
    private SurvivorAgentUpdaterBT agent;

    public CheckWoodStorageFull(SurvivorAgentUpdaterBT agent)
    {
        this.agent = agent;
    }

    public override BTNodeState Evaluate()
    {
        int woodStorage = agent.GetWoodStorageAmount();

        if (woodStorage <= 9)
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
