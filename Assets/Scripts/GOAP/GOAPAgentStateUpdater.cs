using UnityEngine;

public abstract class GOAPAgentStateUpdater : MonoBehaviour
{
    public abstract void StartAgentStates(GOAPAgent agent);
    public abstract void UpdateAgentStates(GOAPAgent agent);
}
