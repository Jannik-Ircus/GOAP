using BehaviorTree;

public class CheckFirepitActive : BTNode
{
    private SurvivorAgentUpdaterBT agent;
    
    public CheckFirepitActive(SurvivorAgentUpdaterBT agent)
    {
        this.agent = agent;
    }

    public override BTNodeState Evaluate()
    {
        bool firepitIsActive = agent.IsFirepitActive();

        if(firepitIsActive)
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
