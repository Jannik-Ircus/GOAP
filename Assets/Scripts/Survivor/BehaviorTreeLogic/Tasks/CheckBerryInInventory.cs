using BehaviorTree;

public class CheckBerryInInventory : BTNode
{
    private SurvivorAgentUpdaterBT agent;

    public CheckBerryInInventory(SurvivorAgentUpdaterBT agent)
    {
        this.agent = agent;
    }

    public override BTNodeState Evaluate()
    {
        int berryInInventory = agent.currentBerries;
        if(agent.GetClosestBerry() == null)
        {
            state = BTNodeState.FAILURE;
            return state;
        }
        if (berryInInventory < 1)
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
