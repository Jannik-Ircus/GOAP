using BehaviorTree;

public class CheckBerryStorageFull : BTNode
{
    private SurvivorAgentUpdaterBT agent;

    public CheckBerryStorageFull(SurvivorAgentUpdaterBT agent)
    {
        this.agent = agent;
    }

    public override BTNodeState Evaluate()
    {
        int berryStorage = agent.GetBerryStorageAmount();

        if(berryStorage <= 9)
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
