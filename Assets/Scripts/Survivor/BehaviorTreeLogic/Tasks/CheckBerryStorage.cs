using BehaviorTree;

public class CheckBerryStorage : BTNode
{
    private SurvivorAgentUpdaterBT agent;

    public CheckBerryStorage(SurvivorAgentUpdaterBT agent)
    {
        this.agent = agent;
    }

    public override BTNodeState Evaluate()
    {
        int berryStorage = agent.GetBerryStorageAmount();

        if(berryStorage>=1)
        {
            state = BTNodeState.SUCCESS;
            return state;
        }else
        {
            state = BTNodeState.FAILURE;
            return state;
        }
    }
}
