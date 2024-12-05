using BehaviorTree;

public class CheckWoodInInventory : BTNode
{
    private SurvivorAgentUpdaterBT agent;

    public CheckWoodInInventory(SurvivorAgentUpdaterBT agent)
    {
        this.agent = agent;
    }

    public override BTNodeState Evaluate()
    {
        int woodInInventory = agent.currentWood;

        if (woodInInventory < 1)
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
