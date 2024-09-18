using System.Collections.Generic;

public class GOAPNode
{
    public GOAPNode parent;
    public float cost;
    public Dictionary<string, int> state;
    public GOAPAction action;

    public GOAPNode(GOAPNode parent, float cost, Dictionary<string, int> worldStates, GOAPAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(worldStates);
        this.action = action;
    }

    public GOAPNode(GOAPNode parent, float cost, Dictionary<string, int> worldStates, Dictionary<string, int> agentStates, GOAPAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(worldStates);
        foreach (KeyValuePair<string, int> b in agentStates)
        {
            if (!this.state.ContainsKey(b.Key))
            {
                this.state.Add(b.Key, b.Value);
            }
        }
        this.action = action;
    }
}
