using BehaviorTree;

public class CheckHunger : BTNode
{
    private SurvivorAgentUpdaterBT agent;

    public CheckHunger(SurvivorAgentUpdaterBT agent)
    {
        this.agent = agent;
    }

    public override BTNodeState Evaluate()
    {

        bool isHungry = agent.GetIsHungry();

        if(isHungry)
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
