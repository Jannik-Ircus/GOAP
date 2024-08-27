using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPAgent : MonoBehaviour
{
    public Dictionary<string, int> agentStates;
    public GOAPWorldStates perceivedWorldStates;
    public List<GOAPAction> actions;
    public Dictionary<string, int> goals;
}
